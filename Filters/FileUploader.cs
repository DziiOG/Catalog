using Catalog.Contracts;
using Catalog.Helpers;
using Catalog.Intallers.Services;
using Catalog.Services.AWS.S3;
using Catalog.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.StaticFiles;

namespace Catalog.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class FileUploader : Attribute, IAsyncActionFilter
    {
        private readonly List<FileFieldAndContentType> _fieldValidations =
            new List<FileFieldAndContentType>();

        public FileUploader(string[] fieldValidations)
        {
            this._fieldValidations = fieldValidations
                .Select(element =>
                {
                    String[] spearator = { ":" };
                    Int32 count = 3;
                    String[] splittedElement = element.Split(
                        spearator,
                        count,
                        StringSplitOptions.RemoveEmptyEntries
                    );
                    string key = splittedElement[0];
                    string contentType = splittedElement[1];
                    string bucketName = splittedElement[2];
                    Console.WriteLine($"{key}");
                    FileFieldAndContentType obj = new FileFieldAndContentType()
                    {
                        ContentType = contentType,
                        FileFIeld = key,
                        BucketName = bucketName
                    };
                    return obj;
                })
                .ToList();
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            IStorageService _serviceStorage =
                context.HttpContext.RequestServices.GetRequiredService<IStorageService>();
            AmazonClientSettings _awsCrendentials =
                context.HttpContext.RequestServices.GetRequiredService<AmazonClientSettings>();

            if (context.HttpContext.Request.Form.Files.Count > 0)
            {
                using (
                    IEnumerator<IFormFile>? files =
                        context.HttpContext.Request.Form.Files.GetEnumerator()
                )
                {
                    bool error = false;
                    while (files.MoveNext())
                    {
                        IFormFile currentFile = files.Current;
                        string fieldKey = currentFile.Name;

                        // get field content
                        try
                        {
                            Console.WriteLine(_fieldValidations.Count);
                            _fieldValidations.Select(x =>
                            {
                                Console.WriteLine(x.FileFIeld);
                                return x;
                            });
                            int fileContentTypeIndex = _fieldValidations.FindIndex(element =>
                            {
                                Console.WriteLine($"{element.FileFIeld} {fieldKey}");

                                return element.FileFIeld == fieldKey;
                            });

                            FileFieldAndContentType fileContentType = _fieldValidations[
                                fileContentTypeIndex
                            ];

                            if (fileContentType != null)
                            {
                                String[] spearator = { ", " };
                                Int32 count = 3;
                                String[] splittedElementArray = fileContentType.ContentType.Split(
                                    spearator,
                                    count,
                                    StringSplitOptions.RemoveEmptyEntries
                                );

                                string currentFileName = currentFile.FileName;
                                string fileKey =
                                    $"CF-{Guid.NewGuid() + Path.GetExtension(currentFileName)}";

                                await using var memoryStr = new MemoryStream();
                                await currentFile.CopyToAsync(memoryStr);

                                S3Object s3Obj = new S3Object()
                                {
                                    Name = fileKey,
                                    BucketName = fileContentType.BucketName,
                                    InputStream = memoryStr
                                };

                                FileExtensionContentTypeProvider? provider =
                                    new FileExtensionContentTypeProvider();

                                if (
                                    !provider.TryGetContentType(
                                        currentFile.FileName,
                                        out string? contentType
                                    )
                                )
                                {
                                    contentType = "";
                                }

                                bool isValidFileMime = Misc.Contains(
                                    new List<string>() { contentType },
                                    splittedElementArray.ToList()
                                );

                                if (isValidFileMime)
                                {
                                    var result = await _serviceStorage.UploadFileAsync(
                                        s3Obj,
                                        _awsCrendentials,
                                        contentType
                                    );
                                }
                                else
                                {
                                    error = true;
                                    context.ModelState.AddModelError(
                                        "Bad Request",
                                        $"Field::{fieldKey} has in valid file type "
                                    );
                                    break;
                                }
                            }
                            else
                            {
                                error = true;
                                context.ModelState.AddModelError(
                                    "Bad Request",
                                    $"Unprocessible entities field or field not required"
                                );
                                break;
                            }
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            error = true;
                            context.ModelState.AddModelError(
                                "Bad Request",
                                "Unprocessible entities field or field not required"
                            );
                            break;
                        }
                        catch (Exception e)
                        {
                            error = true;
                            context.ModelState.AddModelError("Bad Request", e.Message);
                            break;
                        }

                        // error
                    }
                    if (!error)
                    {
                        await next();
                    }
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
            }
            else
            {
                await next();
            }
        }
    }
}
