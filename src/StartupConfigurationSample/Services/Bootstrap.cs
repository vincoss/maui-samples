using StartupConfigurationSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;


namespace StartupConfigurationSample.Services
{
    public class Bootstrap : IBootstrap
    {
        private readonly IDatabaseService _databaseService;

        public Bootstrap(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void Run()
        {
        }
    }
}
