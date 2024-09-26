namespace NamedPipeWrapper.Json.Json
{
    internal class NewtonsoftJsonService : IJsonService
    {
        public NewtonsoftJsonService()
        {
            Settings = new Newtonsoft.Json.JsonSerializerSettings();
        }
        public Newtonsoft.Json.JsonSerializerSettings Settings { get; }
        public T Deserialize<T>(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value, Settings);
        }

        public string Serialize(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value, Settings);
        }
    }
}