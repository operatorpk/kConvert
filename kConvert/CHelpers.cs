using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kConvert
{
    static class Helpers
    {
        //Class logger
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // get app string
        public static string GetAppName()
        {
            //return GetType(this).Namespace;
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }

        // get version string
        public static string GetAppVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
