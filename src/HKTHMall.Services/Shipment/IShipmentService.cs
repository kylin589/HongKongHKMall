using System.Collections.Generic;
using Autofac.Extras.DynamicProxy2;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Shipment;
using HKTHMall.Domain.Models;
using BrCms.Framework.Collections;

namespace HKTHMall.Services.Shipment
{
    [Intercept(typeof(ServiceIInterceptor))]
    public interface IShipmentService : IDependency
    {
        ResultModel AddShipment(YF_FareTemplateModel model);
        ResultModel UpdateShipment(YF_FareTemplateModel model);
        ResultModel DeleteShipment(IList<int> Ids);
        ResultModel GetFareTemplateById(int id,int languageId);
        ResultModel SearchShipment(SearchShipmentModel model);
        ResultModel SetDefault(int id);
        ResultModel GetAllFareTemp();
        ResultModel GetFareTemp(long id);
        ResultModel AddFareTemp(YF_FareTempModel model);
        ResultModel UpdateFareTemp(YF_FareTempModel model);

        ResultModel FareTempPaged(Paged page);
    }
}