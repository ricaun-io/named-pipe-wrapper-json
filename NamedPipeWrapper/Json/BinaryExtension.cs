using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NamedPipeWrapper.Json
{
    /// <summary>
    /// BinaryExtension
    /// </summary>
    public static class BinaryExtension
    {
        /// <summary>
        /// JsonDeserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binaryFormatter"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(this BinaryFormatter binaryFormatter, MemoryStream memoryStream)
        {
            var value = binaryFormatter.Deserialize(memoryStream);

            if (JsonExtension.IsTypeJson(typeof(T)))
                return value.ToString().JsonDeserialize<T>();

            return (T)value;
        }

        /// <summary>
        /// JsonSerialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binaryFormatter"></param>
        /// <param name="memoryStream"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] JsonSerialize<T>(this BinaryFormatter binaryFormatter, MemoryStream memoryStream, T value)
        {
            object valueObject = value;
            if (JsonExtension.IsTypeJson(typeof(T)))
                valueObject = value.JsonSerialize();

            binaryFormatter.Serialize(memoryStream, valueObject);

            return memoryStream.ToArray();
        }
    }
}