using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace StartupConfigurationSample.Interfaces
{
    public interface IHttpService
    {
        Task<int> GetAsync();
    }
}
