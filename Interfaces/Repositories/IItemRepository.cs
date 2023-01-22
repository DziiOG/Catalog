using Catalog.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Interfaces.Repositories
{
    public interface IItemRepository
    {
        Task<Item>? GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();

        Task CreateItemAsync(Item item);

        Task UpdateItemAsync(Item item);

        Task DeleteItemAsync(Guid id);
    }
}
