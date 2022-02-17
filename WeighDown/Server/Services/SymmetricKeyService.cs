﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WeighDown.Server.Services
{
    public class SymmetricKeyService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public SymmetricKeyService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public SymmetricSecurityKey GetSymmetricKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SymmetricKey"]));
            //if (_env.EnvironmentName == Environments.Development)
            //{
            //    return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SymmetricKey"]));
            //}
            //else
            //{
            //    return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("SymmetricKey")));
            //}
        }
    }
}
