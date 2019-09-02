using System.Threading.Tasks;

namespace FundaQueries.Services
{
    public class ThreadService : IThreadService
    {
        public async Task Sleep(int ms)
        {
            await Task.Delay(60000);
        }
    }
}
