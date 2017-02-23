using System;
using Newtonsoft.Json;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class JwtEncodingService : IJwtEncodingService
    {
        public string EncodeObject(object objToBeEncoded)
        {
            var json = JsonConvert.SerializeObject(objToBeEncoded);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            var encoded = EncodeBytes(bytes);
            return encoded;
        }

        public T DecodeObject<T>(string encodedObject)
        {
            encodedObject = encodedObject.Replace('-', '+'); // 62nd char of encoding
            encodedObject = encodedObject.Replace('_', '/'); // 63rd char of encoding
            switch (encodedObject.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: encodedObject += "=="; break; // Two pad chars
                case 3: encodedObject += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }

            var bytes = Convert.FromBase64String(encodedObject);
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            var deserializedObject = JsonConvert.DeserializeObject<T>(json);
            return deserializedObject;
        }

        public string EncodeBytes(byte[] bytesToEncoded)
        {
            var encoded = Convert.ToBase64String(bytesToEncoded);
            encoded = encoded.Split('=')[0]; // Remove any trailing '='s
            encoded = encoded.Replace('+', '-'); // 62nd char of encoding
            encoded = encoded.Replace('/', '_'); // 63rd char of encoding
            return encoded;
        }
    }
}