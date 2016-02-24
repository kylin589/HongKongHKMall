using HKTHMall.Admin.Models;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.Admin.common
{
    public class GetBannerAndBannerProduct
    {
        private CategoryService _categoryService=new CategoryService();

        

        #region banner
        /// <summary>
        /// 根据位置（分类,某页面的某部分对应名称）ID获取对应名称
        /// </summary>
        /// <param name="PlaceCode">位置ID（页面某部分对应ID）</param>
        /// <param name="IdentityStatus">标识ID（页面对应ID）</param>
        /// <returns></returns>
        public string GetPlaceCodeName(int PlaceCode, int IdentityStatus)
        {
            var name = "";
            if (IdentityStatus == 2)
            {
                switch (PlaceCode)
                {
                    case 1:
                        name = "F1 Shoes and shoes";
                        break;
                    case 2:
                        name = "2F  A protective dressing";
                        break;
                    //case 3:
                    //    name = " 首页banner轮转2";
                    //    break;
                    //case 4:
                    //    name = " 首页banner轮转3";
                    //    break;
                    //case 5:
                    //    name = "";//预留
                    //    break;
                    default:
                        break;
                }
            }

            return name;
        }

        /// <summary>
        /// 根据标识ID（页面id）获取对应名称
        /// </summary>
        /// <param name="IdentityStatus">标识ID（页面id）</param>
        /// <returns>标识名称（页面名称）</returns>
        public string GetIdentityStatusName(int IdentityStatus)
        {
            var name = "";

            switch (IdentityStatus)
            {
                case 1:
                    name = "Banner home carousel";
                    break;
                case 2:
                    name = "Home floor banner";
                    break;
                case 3:
                    name = "Banner carousel channel classification";
                    break;
                case 4:
                    name = "Classified channel floor banner";
                    break;
                case 5:
                    name = "";//预留
                    break;
                case 6:
                    name = "";//预留
                    break;
                case 7:
                    name = "Ad on the right";//
                    break;
                case 8:
                    name = "";//预留
                    break;
                default:
                    break;
            }


            return name;
        }

        /// <summary>
        /// 根据标识ID返回所在的所有位置信息
        /// </summary>
        /// <param name="PlaceCode">位置ID（页面某部分对应ID）</param>
        /// <param name="IdentityStatus">标识ID（页面对应ID）</param>
        /// <returns></returns>
        public List<BannerPlaceCodeModel> GetPlaceCodeNameList(int IdentityStatus, int[] PlaceCode)
        {
            List<BannerPlaceCodeModel> models = new List<BannerPlaceCodeModel>();
            foreach (var item in PlaceCode)
            {
                BannerPlaceCodeModel model = new BannerPlaceCodeModel();
                model.ID = item;
                model.IdentityStatus = IdentityStatus;
                model.IdentityStatusName = GetIdentityStatusName(IdentityStatus);
                model.PlaceCode = item;
                model.PlaceCodeName = GetPlaceCodeName(item, IdentityStatus);
                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// 根据标识获取页面信息
        /// </summary>
        /// <param name="IdentityStatus">标识ID（页面ID）</param>
        /// <returns></returns>
        public List<IdentityModel> GetIdentityNameList(int[] IdentityStatus)
        {
            List<IdentityModel> models = new List<IdentityModel>();
            foreach (var item in IdentityStatus)
            {
                IdentityModel model = new IdentityModel();
                model.IdentityStatus = item;
                model.IdentityStatusName = GetIdentityStatusName(item);
                models.Add(model);
            }

            return models;
        } 
        #endregion

        #region bannerProduct广告商品
        /// <summary>
        /// 根据位置（分类,某页面的某部分对应名称）ID获取对应名称
        /// </summary>
        /// <param name="PlaceCode">位置ID（页面某部分对应ID）</param>
        /// <param name="IdentityStatus">标识ID（页面对应ID）</param>
        /// <returns></returns>
        public string GetBannerProducPlaceCodeName(int PlaceCode, int IdentityStatus)
        {
            var name = "";
            if (IdentityStatus == 2)
            {
                switch (PlaceCode)
                {
                    case 1:
                        name = "Home banner rotation";
                        break;
                    case 2:
                        name = "Home banner rotation one";
                        break;
                    case 3:
                        name = " Home banner rotation two";
                        break;
                    case 4:
                        name = " Home banner rotation three";
                        break;
                    case 5:
                        name = "";//预留
                        break;
                    default:
                        break;
                }
            }

            return name;
        }

        /// <summary>
        /// 根据标识ID（页面id）获取对应名称
        /// </summary>
        /// <param name="IdentityStatus">标识ID（页面id）</param>
        /// <returns>标识名称（页面名称）</returns>
        public string GetBannerProducIdentityStatusName(int IdentityStatus)
        {
            var name = "";

            switch (IdentityStatus)
            {
                case 1:
                    name = "Recommend goods to the right of the home page.";
                    break;
                case 2:
                    name = "Home floor commodity";
                    break;
                case 3:
                    name = "App floor commodity";
                    break;
                case 4:
                    name = "";
                    break;
                case 5:
                    name = "";//预留
                    break;
                default:
                    break;
            }


            return name;
        }

        /// <summary>
        /// 根据标识ID返回所在的所有位置信息
        /// </summary>
        /// <param name="PlaceCode">位置ID（页面某部分对应ID）</param>
        /// <param name="IdentityStatus">标识ID（页面对应ID）</param>
        /// <returns></returns>
        public List<BannerPlaceCodeModel> GetBannerProducPlaceCodeNameList(int IdentityStatus, int[] PlaceCode)
        {
            List<BannerPlaceCodeModel> models = new List<BannerPlaceCodeModel>();
            foreach (var item in PlaceCode)
            {
                BannerPlaceCodeModel model = new BannerPlaceCodeModel();
                model.ID = item;
                model.IdentityStatus = IdentityStatus;
                model.IdentityStatusName = GetBannerProducIdentityStatusName(IdentityStatus);
                model.PlaceCode = item;
                model.PlaceCodeName = GetBannerProducPlaceCodeName(item, IdentityStatus);
                models.Add(model);
            }



            return models;
        }

        /// <summary>
        /// 根据标识ID返回所在的所有位置信息
        /// </summary>
        /// <param name="languageId">语言ID</param>
        /// <param name="parentId">父ID</param>
        /// <returns></returns>
        public List<BannerPlaceCodeModel> GetBannerProducPlaceCodeNameList(int languageId, int parentId, int IdentityStatus)
        {
            //获取商品分类信息
            List<CategoryModel> CategoryModellist = this._categoryService.GetCategoriesByParentIdAuditState(languageId, parentId).Data;
            //Result res= this._categoryService.GetCategoriesByCategoryToTree(1);
            List<BannerPlaceCodeModel> models = new List<BannerPlaceCodeModel>();
            foreach (var item in CategoryModellist)
            {
                 
                BannerPlaceCodeModel model = new BannerPlaceCodeModel();
                model.ID = item.CategoryId.Value;
                model.IdentityStatus = IdentityStatus;
                model.IdentityStatusName = GetBannerProducIdentityStatusName(IdentityStatus);
                model.PlaceCode = item.CategoryId.Value;
                model.PlaceCodeName = item.CategoryName; //GetCategoryName(item.Category_Lang as List<CategoryLanguageModel>, item.CategoryId);
                models.Add(model);
            }
            return models;
        }

        public string GetCategoryName(List<CategoryLanguageModel> model, int CategoryId)
        {
            var CategoryName = string.Empty;
            foreach (var item in model)
            {
                if (item.LanguageID == 1 && item.LanguageID == CategoryId)
                {
                    CategoryName = item.CategoryName; break;
                }
            }

            return CategoryName;
        }

        /// <summary>
        /// 根据标识获取页面信息
        /// </summary>
        /// <param name="IdentityStatus">标识ID（页面ID）</param>
        /// <returns></returns>
        public List<IdentityModel> GetBannerProducIdentityNameList(int[] IdentityStatus)
        {
            List<IdentityModel> models = new List<IdentityModel>();
            foreach (var item in IdentityStatus)
            {
                IdentityModel model = new IdentityModel();
                model.IdentityStatus = item;
                model.IdentityStatusName = GetBannerProducIdentityStatusName(item);
                models.Add(model);
            }

            return models;
        }  


        #endregion
    }
}