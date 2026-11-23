using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_AppWithWebServer_EmbedIO.Services
{
    public class GeolocationDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public double? Accuracy { get; set; }
        public double? VerticalAccuracy { get; set; }
        public double? Speed { get; set; }
        public double? Course { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string? AcquireDuration { get; set; }
        public bool IsMock { get; set; }
        public string? Error { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{nameof(Latitude)}: {Latitude}, ");
            sb.AppendLine($"{nameof(Longitude)}: {Longitude}, ");
            sb.AppendLine($"{nameof(Altitude)}: {Altitude ?? 0}, ");
            sb.AppendLine($"{nameof(Accuracy)}: {Accuracy ?? 0}, ");
            sb.AppendLine($"{nameof(VerticalAccuracy)}: {VerticalAccuracy ?? 0}, ");
            sb.AppendLine($"{nameof(Speed)}: {Speed ?? 0}, ");
            sb.AppendLine($"{nameof(Course)}: {Course ?? 0}, ");
            sb.AppendLine($"{nameof(Timestamp)}: {Timestamp}, ");
            sb.AppendLine($"{nameof(AcquireDuration)}: {AcquireDuration},");
            sb.AppendLine($"{nameof(IsMock)}: {IsMock},");
            sb.AppendLine($"{nameof(Error)}: {Error}");

            return sb.ToString();
        }
    }

    public interface IPlatformGeolocation
    {
        Task<GeolocationDto> Get();
    }

    public class PlatformGeolocation : IPlatformGeolocation
    {
        public async Task<GeolocationDto> Get()
        {
            var start = DateTime.UtcNow;

            var dto = await MainThread.InvokeOnMainThreadAsync<GeolocationDto>(async () =>
            {
                var localDto = new GeolocationDto();

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
                        Map(location, localDto);
                    }
                }
                catch (Exception ex)
                {
                    /*
                        NOTE: ID_CAP_LOCATION access denied, if location is disabled on the device. Example windows
                    */

                    Console.WriteLine(ex); // TODO: logger
                    localDto.Error = ex.Message;
                }

                return localDto;
            });

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
