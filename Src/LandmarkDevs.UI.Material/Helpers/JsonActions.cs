#region

using System.IO;
using System.Text;
using Newtonsoft.Json;

#endregion

namespace LandmarkDevs.UI.Material.Helpers
{
    /// <summary>
    ///     Class JsonActions.
    /// </summary>
    public static class JsonActions
    {
        /// <summary>
        ///     Converts the object to a JSON string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        public static string ConvertToJsonString<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            });
            return json;
        }

        /// <summary>
        ///     Converts the JSON string to an object of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns>T.</returns>
        public static T ConvertFromJsonString<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        ///     Saves to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <param name="obj">The object.</param>
        public static void SaveToJson<T>(string filePath, T obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            });
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    stream.Position = 0;
                    stream.CopyTo(fs);
                }
            }
        }

        /// <summary>
        ///     Reads the file to JSON and converts it to the specified object type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns>T.</returns>
        public static T ReadJson<T>(string filePath)
        {
            using (var stream = new StreamReader(filePath))
            {
                var jsonData = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
        }
    }
}