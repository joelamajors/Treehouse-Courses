using System.Collections.Generic;
using System.Linq;

namespace BirdWatcher
{
    public class BirdSearch
    {
        public string CommonName { get; set; }
        public List<string> Colors { get; set; }
        public string Country { get; set; }
        public string Size { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public static class BirdSearchExtension 
    {
        public static IEnumerable<Bird> Search(this IEnumerable<Bird> source, BirdSearch search)
        {
            return source.Where(s => search.CommonName == null || s.CommonName.Contains(search.CommonName))
                         .Where(s => search.Country == null || s.Habitats.Any(h => h.Country.Contains(search.Country)))
                         .Where(s => search.Size == null || s.Size == search.Size)
                         .Where(s => search.Colors.Any(c => c == s.PrimaryColor) || 
                                     search.Colors.Any(c => c == s.SecondaryColor) ||
                                     search.Colors.Join(s.TertiaryColors,
                                                        sc => cs,
                                                        tc => tc,
                                                        (sc, tc) => (sc)).Any())
                         // now all of our colors are in our filter
                         // now to deal with the Page and PageSize parameters using Skip and Take.
                         .Skip(search.Page * search.PageSize) // will skip ahead as many elements as returned to make sure nothing is missed for the user
                         .Take(search.PageSize);
            // now that's all of our search logic in one LINQ expression
            // just ignore all the errors, i guess...
        }
    }
}