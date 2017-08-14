using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerDaemon
{
    class StringsConfiguration
    {
        public static string QuequeBitacora
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.QuequeBitacora"];
            }
        }
        public static string QuequeNotifications
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.QuequeNotifications"];
            }
        }
    }
}
