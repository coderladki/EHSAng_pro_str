using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.Linq
{
    public static class LinqExtension
    {
        public static T WhereFirstOrDefault<T>(this List<T> list, Func<T, bool> predicate)
        {
            return list.Where(predicate).FirstOrDefault();
        }
        public static T WhereLastOrDefault<T>(this List<T> list, Func<T, bool> predicate)
        {
            return list.Where(predicate).LastOrDefault();
        }
        public static int IndexOf<T>(this List<T> list, Func<T, bool> predicate)
        {
            return list.IndexOf(list.WhereFirstOrDefault(predicate));
        }
        public static string ToJson<T>(this List<T> list)
        {
            var options = new JsonSerializerOptions();
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder. UnsafeRelaxedJsonEscaping;

            var result = JsonSerializer.Serialize<List<T>>(list, options);
            return result;
        }
    }

}
