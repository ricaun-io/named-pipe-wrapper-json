using System.IO;
using System.Text;

namespace ricaun.NamedPipeWrapper.Json
{
    /// <summary>
    /// JsonFormatter
    /// </summary>
    public class JsonFormatter : IJsonFormatter
    {
        /// <summary>
        /// JsonDeserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public T JsonDeserialize<T>(MemoryStream memoryStream)
        {
            // Reset the position of the memory stream to the beginning
            memoryStream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
            {
                var jsonString = reader.ReadToEnd();
                return jsonString.JsonDeserialize<T>();
            }
        }

        /// <summary>
        /// JsonSerialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memoryStream"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] JsonSerialize<T>(MemoryStream memoryStream, T value)
        {
            var jsonString = value.JsonSerialize();

            // Reset the position of the memory stream to the beginning
            memoryStream.Seek(0, SeekOrigin.Begin);

            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                writer.Write(jsonString);
                writer.Flush();
            }

            // Return the byte array from the memory stream
            return memoryStream.ToArray();
        }
    }
}