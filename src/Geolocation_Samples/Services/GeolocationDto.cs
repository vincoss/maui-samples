using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Geolocation_Samples.Services
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
        public string AcquireDuration { get; set; }
        public bool IsMock { get; set; }
        public string Error { get; set; }

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
}
