using BrCms.Core.Infrastructure;
using HKTHMall.Domain.WebModel.Models.Version;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Version
{
    public interface IVersionService : IDependency
    {
        APP_VersionInfo GetMaxVersionNo(string packageName, string versionNO);
    }
}
