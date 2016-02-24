using HKTHMall.Core.Utils;
using HKTHMall.Domain.WebModel.Models.Version;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Version
{
    public class VersionService : BaseService, IVersionService
    {
        public APP_VersionInfo GetMaxVersionNo(string packageName, string versionNO)
        {
          
            var q = base._database.Db.APP_VersionInfo.FindAllByPackageName(Utils.ChkSQL(packageName));
            q = q.OrderByDescending(base._database.Db.APP_VersionInfo.VersionNO);
            List<APP_VersionInfo> list = q.ToList<APP_VersionInfo>();
            if (list.Count>0)
            {
                APP_VersionInfo info = new APP_VersionInfo();
                info = list[0];
                //if (list[0].IsForceUpdate == 1)
                //{
                //    return list[0];
                //}
                //if (list[0].VersionNO.Replace(".", string.Empty) != Utils.ChkSQL(versionNO))
                //{
                //    info.IsForceUpdate = 1;                   
                //}
                return info;   
            }
            return null;     
        }
    }
}
