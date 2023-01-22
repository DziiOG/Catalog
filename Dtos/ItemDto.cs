using System;

namespace Catalog.Dtos
{
    public record ItemDto
    {
        // init allows for modification for the first time and rejects subsequent updates
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public decimal Price { get; init; }

        public DateTimeOffset CreatedDate { get; init; }
    }
}
