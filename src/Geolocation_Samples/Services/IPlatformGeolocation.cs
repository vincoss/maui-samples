using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Geolocation_Samples.Services
{
    public interface IPlatformGeolocation
    {
        Task<GeolocationDto> Get();
    }
}
