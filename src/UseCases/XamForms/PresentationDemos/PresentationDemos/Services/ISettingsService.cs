using System.Threading.Tasks;

namespace PresentationDemos.Services
{
    public interface ISettingsService
    {
        Task<int> GetMaxTodos();
        Task SaveMaxTodos(int max);
    }
}