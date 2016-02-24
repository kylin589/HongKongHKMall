using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public class Yh_groupmarkService : BaseService, IYh_groupmarkService
    {


        public Domain.Models.ResultModel GetGroupMarkByUserId(long userId)
        {
            var userBrank = _database.Db.YH_GroupMark;
            var result = new ResultModel();
            result.Data = userBrank.FindAll(userBrank.UserID == userId).ToList<YH_GroupMark>();
            return result;
        }

        public ResultModel AddGroupMark(YH_GroupMark model)
        {
            ResultModel result = new ResultModel();
            var count = _database.Db.YH_GroupMark.GetCount(_database.Db.YH_GroupMark.UserID == model.UserID &&
                _database.Db.YH_GroupMark.GroupID == model.GroupID && _database.Db.YH_GroupMark.GroupType == model.GroupType);
            if (count > 0)
            {
                result.IsValid = false;
                result.Messages = new List<string> { "该数据已存在." };
            }
            else
            {
                result.Data = _database.Db.YH_GroupMark.Insert(model);
                result.Messages = new List<string> { "添加数据成功." };
            }
            return result;
        }


        public ResultModel DeleteGroupMark(long userId, int groupType)
        {
            ResultModel result = new ResultModel();
            result.Data = _database.Db.YH_GroupMark.Delete(UserID: userId, GroupType: groupType);
            return result;
        }
    }
}
