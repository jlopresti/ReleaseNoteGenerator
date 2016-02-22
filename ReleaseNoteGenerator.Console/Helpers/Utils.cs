using Newtonsoft.Json;

namespace ReleaseNoteGenerator.Console.Helpers
{
    public static class JsonExtentensions
    {
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default(T);

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException ex)
            {
                return default(T);
            }   
        }
    }
}
