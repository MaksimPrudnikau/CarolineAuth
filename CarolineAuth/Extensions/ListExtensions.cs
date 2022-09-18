using CarolineAuth.Models;

namespace CarolineAuth.Extensions;

public static class ListExtensions
{
    public static bool ContainsIndex(this IList<Row> source, int? id)
    {
        if (id is null)
            return false;

        return source.Any(row => row.Id == id);
    }
    
    public static void RemoveInc(this IList<Row> source, int id)
    {
        source.RemoveAt(id - 1);
        for (var i = 0; i < source.Count; ++i)
        {
            source[i].Id = i + 1;
        }
    }
}