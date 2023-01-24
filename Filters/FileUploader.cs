using System.Net;
using Catalog.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Catalog.Extensions;

namespace Catalog.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class FileUploader : Attribute, IAsyncActionFilter
    {
        private readonly List<string> roles = new List<string>();

        public FileUploader(string[] accessRoles)
        {
            this.roles = accessRoles.ToList();
        }

        public Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            throw new NotImplementedException();
        }
    }
}
