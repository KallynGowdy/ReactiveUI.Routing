using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareNavigation.Services
{
    public interface IPhotosService
    {
        Task<Photo[]> GetPhotosAsync();
        Task SharePhotoAsync(Photo photo);
    }
}
