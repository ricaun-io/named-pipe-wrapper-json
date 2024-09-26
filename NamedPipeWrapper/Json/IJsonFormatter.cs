using System.IO;

namespace ricaun.NamedPipeWrapper.Json
{
    /// <summary>
    /// IJsonFormatter
    /// </summary>
    public interface IJsonFormatter
    {
        /// <summary>
        /// JsonDeserialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public T JsonDeserialize<T>(MemoryStream memoryStream);
        /// <summary>
        /// JsonSerialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memoryStream"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] JsonSerialize<T>(MemoryStream memoryStream, T value);
    }
}