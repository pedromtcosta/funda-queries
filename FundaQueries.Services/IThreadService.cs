using System.Threading.Tasks;

namespace FundaQueries.Services
{
    public interface IThreadService
    {
        Task Sleep(int ms);
    }
}
