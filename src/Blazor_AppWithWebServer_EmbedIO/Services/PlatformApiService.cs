using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_AppWithWebServer_EmbedIO.Services
{
    public interface IPlatformApiService
    {
        Task<string?> TakePhotoAsync();
    }

    public class PlatformApiService : IPlatformApiService
    {
        private readonly IPath _path;

        public PlatformApiService(IPath path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public async Task<string> TakePhotoAsync()
        {
            string filePath = null;

            try
            {
                filePath = await MainThread.InvokeOnMainThreadAsync<string>(async () =>
                {
                    string localFilePath = null;

                    try
                    {
                        if (MediaPicker.IsCaptureSupported == false)
                        {
                            return localFilePath;
                        }

                        FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                        // TODO: Not working on Windows

                        if (photo != null) // User might cancel take photo.
                        {
                            localFilePath = Path.Combine(_path.GetGalleryFolder(), photo.FileName);
                          
                            using Stream sourceStream = await photo.OpenReadAsync();
                            using FileStream localFileStream = File.OpenWrite(localFilePath);

                            await sourceStream.CopyToAsync(localFileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO:
                        localFilePath = ex.Message;
                    }
                    return localFilePath;
                });
            }
            catch (Exception ex)
            {
                // TODO:
                filePath = ex.Message;
            }

            return filePath;
        }
    }
}
