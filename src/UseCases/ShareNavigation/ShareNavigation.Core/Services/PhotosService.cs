using System;
using System.Threading.Tasks;

namespace ShareNavigation.Services
{
    public class PhotosService : IPhotosService
    {
        public Task<Photo[]> GetPhotosAsync()
        {
            throw new NotImplementedException();
        }

        public Task SharePhotoAsync(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}