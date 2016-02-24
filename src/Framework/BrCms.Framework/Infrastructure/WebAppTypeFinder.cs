using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace BrCms.Framework.Infrastructure
{
    /// <summary>
    ///     Provides information about types in the current web application.
    ///     Optionally this class can look at all assemblies in the bin folder.
    /// </summary>
    public class WebAppTypeFinder : AppDomainTypeFinder, ITypeFinder
    {
        #region Fields

        private bool _binFolderAssembliesLoaded;

        #endregion

        #region Methods

        /// <summary>
        ///     Gets a physical disk path of \Bin directory
        /// </summary>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string GetBinDirectory()
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HttpRuntime.BinDirectory;
            }
            //not hosted. For example, run either in unit tests
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public override IList<Assembly> GetAssemblies()
        {
            if (!this._binFolderAssembliesLoaded)
            {
                this._binFolderAssembliesLoaded = true;
                var binPath = this.GetBinDirectory();
                //binPath = _webHelper.MapPath("~/bin");
                this.LoadMatchingAssemblies(binPath);
            }

            return base.GetAssemblies();
        }

        #endregion
    }
}