using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using BrCms.Framework.Configuration;
using BrCms.Framework.Logging;
using BrCms.Framework.Plugin;

[assembly: PreApplicationStartMethod(typeof(PluginManager), "Initialize")]
namespace BrCms.Framework.Plugin
{
    public class PluginManager
    {
        private static readonly ILogger log = NullLogger.Instance;
        public static void Initialize()
        {
            if (!SiteConfig.Configuration.PluginEnabled) return;
            const string pluginVirtualDirectory = "Modules";

            var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pluginVirtualDirectory);

            var pluginFolder = Directory.CreateDirectory(pluginPath);

            if (!pluginFolder.Exists)
                pluginFolder.Create();

            var filterList = new List<string>();

            pluginFolder.GetFiles("*.dll", SearchOption.AllDirectories).ToList().ForEach(p =>
            {
                if (!filterList.Contains(p.Name))
                {
                    filterList.Add(p.Name);
                    var bytes = File.ReadAllBytes(p.FullName);
                    Assembly.Load(bytes);
                }
                else
                {
                    Debug.WriteLine("{0}重复存在。", p.Name);
                    log.Information("{0}重复存在。", p.Name);
                }
            });
            
            var fsw = new FileSystemWatcher(pluginFolder.FullName, "*.dll")
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                               | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                IncludeSubdirectories = true
            };

            fsw.Changed += OnProcess;
            fsw.Created += OnProcess;
            fsw.Deleted += OnProcess;
            fsw.Renamed += OnProcess;

            fsw.EnableRaisingEvents = true;
        }

        private static void OnProcess(object sender, FileSystemEventArgs e)
        {
            var watcher = (FileSystemWatcher)sender;
            watcher.EnableRaisingEvents = false;
            HttpRuntime.UnloadAppDomain();
        }
    }
}
