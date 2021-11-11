using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Essentials;

namespace Geolocation_Samples.Services
{
    public class PlatformGeolocation : IPlatformGeolocation
    {
        public async Task<GeolocationDto> Get()
        {
            var start = DateTime.UtcNow;
            var dto = new GeolocationDto();
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    // potential long running method
                    var request = new GeolocationRequest(GeolocationAccuracy.Best)
                    {
                        Timeout = TimeSpan.FromSeconds(5)
                    };
                    location = await Geolocation.GetLocationAsync(request);
                }

                if (location != null)
                {
                    Map(location, dto);
                }
            }
            catch (Exception ex)
            {
                /*
                    NOTE: ID_CAP_LOCATION access denied, if location is disabled on tthe device. Example windows
                */

                Console.WriteLine(ex); // TODO: logger
                dto.Error = ex.Message;
            }
            dto.AcquireDuration = (DateTime.UtcNow - start).ToString();
            return dto;
        }

        private void Map(Location location, GeolocationDto dto)
        {
            dto.Latitude = location.Latitude;
            dto.Longitude = location.Longitude;
            dto.Altitude = location.Altitude;
            dto.Accuracy = location.Accuracy;
            dto.VerticalAccuracy = location.VerticalAccuracy;
            dto.Speed = location.Speed;
            dto.Course = location.Course;
            dto.Timestamp = location.Timestamp;
            //https://stackoverflow.com/questions/42569245/detect-or-prevent-if-user-uses-fake-location
            dto.IsMock = location.IsFromMockProvider;
        }
    }
}
