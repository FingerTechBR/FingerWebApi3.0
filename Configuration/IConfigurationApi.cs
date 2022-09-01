
using Newtonsoft.Json;

namespace WebApiFingertec3._0.Abstraction
{
    public static class JsonExtensions
    {
        public static T ToObject<T>(this string jsonText)
        {
            return JsonConvert.DeserializeObject<T>(jsonText);
        }

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
