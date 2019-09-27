using ApiBackend.Transversal.DTOs.PLC.RequestDTO;
using System.Threading.Tasks;

namespace ATFaceME.Xamarin.Core.Agents
{
    public interface IAgentClient
    {
        Task<TOutput> CallHttp<TInput, TOutput>(string endpoint, HttpVerbs method, string token, TInput content, string skipToken);
    }
}
