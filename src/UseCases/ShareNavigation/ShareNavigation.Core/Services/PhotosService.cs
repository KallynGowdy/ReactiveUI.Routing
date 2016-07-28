using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareNavigation.Services
{
    public class PhotosService : IPhotosService
    {
        private static readonly List<Photo> photos = new List<Photo>()
        {
            new Photo()
            {
                PhotoUrl = "http://www.nasa.gov/sites/default/files/thumbnails/image/hs-2015-29-a-xlarge_web.jpg"
            }
        };

        public Task<Photo[]> GetPhotosAsync()
        {
            return Task.FromResult(photos.ToArray());
        }

        public Task SharePhotoAsync(Photo photo)
        {
            if (photo == null) throw new ArgumentNullException(nameof(photo));
            photos.Add(photo);
            return Task.FromResult(0);
        }
    }
}