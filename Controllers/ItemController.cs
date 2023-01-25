using Catalog.Interfaces.TokenAuthorization;
using Catalog.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Catalog.Contracts;
using Catalog.Filters;
using Catalog.Models;
using Catalog.Dtos;
using Catalog.Extensions;

namespace Catalog.Controllers
{
    [ApiController]
    [Route(ApiRoutes.CatalogRoutes.CatalogBase)]
    [TokenAuthenticationFilter]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository repository;

        private readonly ITokenManager tokenManager;

        public ItemsController(IItemRepository repository, ITokenManager tokenManager)
        {
            this.repository = repository;
            this.tokenManager = tokenManager;
        }

        // GET /items
        [HttpGet]
        [RedisCached(duration: 60)]
        [AccessRestriction(new string[] { "GROWER" })]
        public async Task<Response<IEnumerable<ItemDto>?>?> GetItemsAsync()
        {
            IEnumerable<ItemDto>? items = (await repository.GetItemsAsync()).Select(
                item => item.AsDto()
            );
            UserInfo? b = (UserInfo?)HttpContext.Items["LoggedInUser"];
            Response<IEnumerable<ItemDto>?>? response = new Response<IEnumerable<ItemDto>?>()
            {
                data = items,
                statusCode = 200,
                message = "Items fetched successfully"
            };
            return response;
        }

        // GET /items/{id}
        [HttpGet(ApiRoutes.CatalogRoutes.ById)]
        public async Task<ActionResult<Response<ItemDto>>> GetItemAsync(Guid id)
        {
            Item? item = await repository.GetItemAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return Ok(
                new Response<ItemDto>()
                {
                    data = item.AsDto(),
                    statusCode = 200,
                    message = "Items fetched successfully"
                }
            );
        }

        [HttpPost("files")]
        [FileUploader(
            new string[]
            {
                "receipt:image/png, image/jpg, image/jpeg:completefarmer/app/test",
                "receipt2:image/png, image/jpg, image/jpeg:completefarmer/app/test"
            }
        )]
        public ActionResult UploadDummyFile([FromForm] IFormCollection form)
        {
            IDictionary<string, List<string>>? files = (IDictionary<string, List<string>>?)
                HttpContext.Items["UploadedFiles"];

            Console.WriteLine(files.ToString());
            foreach (KeyValuePair<string, List<string>> kvp in files)
            {
                foreach (var item in kvp.Value)
                {
                    Console.WriteLine(item);
                }
            }

            return Ok("success");
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);

            // returns a 201 response with the new created item by calling a reference method that would return the new created data
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        // PUT /items/{id}
        [HttpPut(ApiRoutes.CatalogRoutes.ById)]
        public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            Item? existingItem = await repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            // taking the existing copy and creating a new copy with Name and Price modified
            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await repository.UpdateItemAsync(updatedItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete(ApiRoutes.CatalogRoutes.ById)]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}
