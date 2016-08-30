using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ITI.PrimarySchool.DAL.Tests
{
    public static class TestHelpers
    {
        static IConfiguration _configuration;

        public static string ConnectionString
        {
            get
            {
                return Configuration[ "ConnectionStrings:PrimarySchoolDB" ];
            }
        }

        static IConfiguration Configuration
        {
            get
            {
                if( _configuration == null )
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath( Directory.GetCurrentDirectory() )
                        .AddJsonFile( "appsettings.json", optional: true )
                        .AddEnvironmentVariables()
                        .Build();
                }

                return _configuration;
            }
        }
    }
}
