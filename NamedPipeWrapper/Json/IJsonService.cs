namespace NamedPipeWrapper.Json
{
    /// <summary>
    /// Represents a service for JSON serialization and deserialization.
    /// </summary>
    public interface IJsonService
    {
        /// <summary>
        /// Deserializes the specified JSON string into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="value">The JSON string to deserialize.</param>
        /// <returns>The deserialized object of type T.</returns>
        T Deserialize<T>(string value);

        /// <summary>
        /// Serializes the specified object into a JSON string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>The JSON string representation of the serialized object.</returns>
        string Serialize(object value);
    }
}