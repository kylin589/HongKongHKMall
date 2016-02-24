using System;
using System.Configuration;
using System.IO;
using BrCms.Core.Infrastructure;
using log4net.Config;

namespace BrCms.Framework
{
    public class Startup : IStartupTask
    {
        public void Execute()
        {
            var filename = ConfigurationManager.AppSettings["log4net.Config"];
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin", ""), filename).Replace(@"\Debug", "");
            XmlConfigurator.Configure(new FileInfo(path));
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
