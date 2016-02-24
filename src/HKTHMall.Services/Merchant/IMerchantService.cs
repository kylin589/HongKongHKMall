using System.Collections.Generic;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Merchant;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.Merchant
{
    public interface IMerchantService : IDependency
    {
        ResultModel Search(SearchMerchantModel model);
        ResultModel Add(AddMerchantModel model);
        ResultModel Update(UpdateMerchantModel model);
        ResultModel Delete(IList<long> ids);
        ResultModel GetMerchantByMerchantId(long id);
    }
}