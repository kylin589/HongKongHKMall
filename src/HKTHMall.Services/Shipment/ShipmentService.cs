using System;
using System.Collections.Generic;
using System.Linq;
using BrCms.Framework.Collections;
using Castle.Core.Internal;
using HKTHMall.Domain.AdminModel.Models.Shipment;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.Shipment
{
    public class ShipmentService : BaseService, IShipmentService
    {
        public ResultModel AddShipment(YF_FareTemplateModel model)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();

                var fare = db.YF_FareTemplate.Insert(model);
                foreach (var YF_FareTemplateAreaCountryModel in model.YF_FareTemplateAreaCountryModels)
                {
                    YF_FareTemplateAreaCountryModel.FareTemplateID = fare.FareTemplateID;
                }

                if (model.YF_FareTemplateAreaCountryModels.Count > 0)
                {
                    db.YF_FareTemplateAreaCountry.Insert(model.YF_FareTemplateAreaCountryModels);
                }

                return result;
            });
        }

        public ResultModel SetDefault(int id)
        {
           return this._database.RunQuery(db =>
            {
                var result = new ResultModel();
                db.YF_FareTemplate.UpdateAll(IsDefault: 0);
                db.YF_FareTemplate.UpdateByFareTemplateID(FareTemplateID: id, IsDefault: 1);
                return result;
            });
        }

        public ResultModel GetFareTemplateById(int id, int languageId)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();
                //result.Data = db.YF_FareTemplate
                //    .FindAllByFareTemplateID(id)
                //    .LeftJoin(db.ShipmentTemplate.As("ShipmentTemplateModels"), FareTemplateID: db.YF_FareTemplate.FareTemplateID)
                //    .WithMany(db.ShipmentTemplate)
                //    .ToList<YF_FareTemplateModel>();
                YF_FareTemplateModel fare = db.YF_FareTemplate.Get(id);
                fare.YF_FareTemplateAreaCountryModels = db.YF_FareTemplateAreaCountry.FindAllByFareTemplateID(id)
                    .ToList<YF_FareTemplateAreaCountryModel>();
                foreach (var YF_FareTemplateAreaCountryModel in fare.YF_FareTemplateAreaCountryModels)
                {
                    IList<int> ids = new List<int>();
                    if (!string.IsNullOrEmpty(YF_FareTemplateAreaCountryModel.CityIds))
                    {
                        YF_FareTemplateAreaCountryModel.CityIds.Split(',').ForEach(m =>
                        {
                            try
                            {
                                ids.Add(Convert.ToInt32(m));
                            }
                            catch
                            {
                                // ignored
                            }
                        });
                        
                        if (ids.Count > 0)
                        {
                            dynamic lang;
                            IList<string> citys = db.THArea
                            .Query()
                            .Where(db.THArea.THAreaID == ids.ToArray())
                            .LeftJoin(db.THArea_lang, out lang)
                            .On(db.THArea.THAreaID == db.THArea_lang.THAreaID && db.THArea_lang.LanguageID == languageId)
                            .Select(lang.AreaName)
                            .ToScalarList<string>();

                            YF_FareTemplateAreaCountryModel.CityNames = string.Join(",", citys);
                        }
                    }
                }
                result.Data = fare;
                return result;
            });
        }

        public ResultModel UpdateShipment(YF_FareTemplateModel model)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();
                db.YF_FareTemplate.Update(model);

                IList<YF_FareTemplateAreaCountryModel> oldShipmentTemplates =
                    db.YF_FareTemplateAreaCountry.FindAllByFareTemplateID(model.FareTemplateID)
                    .ToList<YF_FareTemplateAreaCountryModel>();

                var updateShipmentTemplates = model.YF_FareTemplateAreaCountryModels
                    .Where(m => m.ShipmentTemplateId != 0)
                    .ToList();

                var ids = oldShipmentTemplates
                    .Where(m => updateShipmentTemplates.All(um => um.ShipmentTemplateId != m.ShipmentTemplateId))
                    .Select(m => m.ShipmentTemplateId)
                    .ToList();

                if (ids.Any())
                {
                    db.YF_FareTemplateAreaCountry.DeleteAll(db.YF_FareTemplateAreaCountry.ShipmentTemplateId == ids);
                }

                if (updateShipmentTemplates.Any())
                {
                    db.YF_FareTemplateAreaCountry.Update(updateShipmentTemplates);
                }

                var inserShips = model.YF_FareTemplateAreaCountryModels
                    .Where(m => m.ShipmentTemplateId == 0)
                    .ToList();

                foreach (var shipmentTemplateModel in inserShips)
                {
                    shipmentTemplateModel.FareTemplateID = model.FareTemplateID;
                }

                if (inserShips.Any())
                {
                    db.YF_FareTemplateAreaCountry.Insert(inserShips);
                }

                return result;
            });
        }

        public ResultModel DeleteShipment(IList<int> Ids)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();
                result.IsValid = true;
                
                foreach (var item in Ids)
                {
                    db.YF_FareTemp.DeleteByFareTempID(item);
                }
                //foreach (var id in Ids)
                //{
                //    var fare = db.YF_FareTemplate.FindByFareTemplateID(id);
                //    db.YF_FareTemplateAreaCountry.DeleteAll(db.YF_FareTemplateAreaCountry.FareTemplateID == fare.FareTemplateID);
                //    db.YF_FareTemplate.Delete(fare);
                //}
                return result;
            });
        }

        public ResultModel SearchShipment(SearchShipmentModel model)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();

                var q = db.YF_FareTemplate.Query()
                    .LeftJoin(db.YF_FareTemplateAreaCountry, FareTemplateID: db.YF_FareTemplate.FareTemplateID)
                    .With(db.YF_FareTemplateAreaCountry)
                    .OrderByDescending(db.YF_FareTemplate.FareTemplateID);

                result.Data = new SimpleDataPagedList<YF_FareTemplateModel>(q, model.PagedIndex, model.PagedSize);

                return result;
            });
        }


        public ResultModel GetAllFareTemp()
        {
            return new ResultModel()
            {
                IsValid = true,
                Data = base._database.Db.YF_FareTemp.All().ToList<YF_FareTempModel>()
            };
        }
        public ResultModel FareTempPaged(Paged page)
        {
            return new ResultModel()
            {
                IsValid = true,
                Data = new SimpleDataPagedList<YF_FareTempModel>(base._database.Db.YF_FareTemp.All(),page.PagedIndex,page.PagedSize)
            };
        }
        public ResultModel GetFareTemp(long id)
        {
           
            return new ResultModel()
            {
                IsValid = true,
                Data = base._database.Db.YF_FareTemp.Get(id)
            };
        }
        public ResultModel AddFareTemp(YF_FareTempModel model)
        {
             return new ResultModel()
             {
                 IsValid=true,
                 Data = base._database.Db.YF_FareTemp.Insert(model)                 
             };
        }
        public ResultModel UpdateFareTemp(YF_FareTempModel model)
        {
            return new ResultModel()
            {
                IsValid = true,
                Data = base._database.Db.YF_FareTemp.Update(model)
            };
        }


    }
}