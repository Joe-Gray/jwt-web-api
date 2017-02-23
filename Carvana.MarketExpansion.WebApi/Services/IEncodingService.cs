namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IEncodingService
    {
        string EncodeObject(object objToBeEncoded);
        T DecodeObject<T>(string encodedObject);
    }
}