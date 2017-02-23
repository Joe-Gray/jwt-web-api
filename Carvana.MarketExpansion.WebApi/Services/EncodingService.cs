using System;
using Newtonsoft.Json;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class EncodingService : IEncodingService
    {
        public string EncodeObject(object objToBeEncoded)
        {
            var json = JsonConvert.SerializeObject(objToBeEncoded);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            var encoded = Convert.ToBase64String(bytes);
            return encoded;
        }

        public T DecodeObject<T>(string encodedObject)
        {
            var bytes = Convert.FromBase64String(encodedObject);
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            var deserializedObject = JsonConvert.DeserializeObject<T>(json);
            return deserializedObject;
        }
    }
}