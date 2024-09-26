using ricaun.NamedPipeWrapper.Json.Json;
using System;

namespace ricaun.NamedPipeWrapper.Json
{
    /// <summary>
    /// JsonExtension
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// IJsonService
        /// </summary>
        public static IJsonService JsonService { get; set; } = CreateJsonService();

        /// <summary>
        /// Create JsonService
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// If NewtonsoftJsonService is available, use it.
        /// </remarks>
        private static IJsonService CreateJsonService()
        {
            try
            {
                return new NewtonsoftJsonService();
            }
            catch { };
            return new JsonService();
        }

        /// <summary>
        /// JsonDeserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(this string value)
        {
            return JsonService.Deserialize<T>(value);
        }

        /// <summary>
        /// JsonSerialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string JsonSerialize<T>(this T value)
        {
            return JsonService.Serialize(value);
        }
    }
}