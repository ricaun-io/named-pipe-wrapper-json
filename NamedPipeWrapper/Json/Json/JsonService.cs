namespace NamedPipeWrapper.Json.Json
{
#if NETFRAMEWORK
    using System.Web.Script.Serialization;
    internal class JsonService : IJsonService
    {
        public JsonService()
        {
            JavaScriptSerializer = new JavaScriptSerializer();
        }
        public JavaScriptSerializer JavaScriptSerializer { get; set; }
        public T Deserialize<T>(string value)
        {
            return JavaScriptSerializer.Deserialize<T>(value);
        }

        public string Serialize(object value)
        {
            return JavaScriptSerializer.Serialize(value);
        }
    }
#endif
#if NET
    using System.Text.Json;
    internal class JsonService : IJsonService
    {
        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value);
        }

        public string Serialize(object value)
        {
            return JsonSerializer.Serialize(value);
        }
    }
#endif
}