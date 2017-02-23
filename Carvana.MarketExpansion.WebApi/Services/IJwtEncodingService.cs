namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IJwtEncodingService
    {
        string EncodeObject(object objToBeEncoded);
        T DecodeObject<T>(string encodedObject);
        string EncodeBytes(byte[] bytesToEncoded);
    }
}