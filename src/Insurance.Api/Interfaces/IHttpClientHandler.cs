using System.Threading.Tasks;

namespace Insurance.Api.Interfaces
{
    public interface IHttpClientHandler
    {
        Task<T> GetResponse<T>(string requestUri) where T : class;
    }
}
