using VideogamesPOS.Models;

namespace VideogamesPOS.Utilities
{
    public static class VideogameQueryExtensions
    {
        public static IQueryable<Videogame> OrderByFrom(
            this IQueryable<Videogame> q, string? sort, string? dir)
        {
            var desc = string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);

            return (sort?.ToLower()) switch
            {
                "rating" => desc ? q.OrderByDescending(v => v.Rating) : q.OrderBy(v => v.Rating),
                "price" => desc ? q.OrderByDescending(v => v.Price) : q.OrderBy(v => v.Price),
                "release" => desc ? q.OrderByDescending(v => v.ReleaseDate) : q.OrderBy(v => v.ReleaseDate),
                "stock" => desc ? q.OrderByDescending(v => v.Stock) : q.OrderBy(v => v.Stock),
                "name" => desc ? q.OrderByDescending(v => v.Name) : q.OrderBy(v => v.Name),
                _ => q.OrderBy(v => v.Name)
            };
        }
    }
}
