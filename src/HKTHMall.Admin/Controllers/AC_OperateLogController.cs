
using HKSJ.Common.FastDFS;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.UploadFile;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Services.LoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;
using HKTHMall.Admin.common;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class AC_OperateLogController : HKBaseController
    {
         private IAC_OperateLogService _acOperateLogService;
         public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath");
         public static readonly string ImageHeader = HKSJ.Common.ConfigHelper.GetConfigString("ImageHeader");
         private UploadFile uf = new UploadFile();
         public AC_OperateLogController(IAC_OperateLogService acOperateLogService)
        {
            this._acOperateLogService = acOperateLogService;
        }
        //列表页
        // GET: /AC_OperateLog/
        public ActionResult Index()
        {
            return View();
        }

        

        public JsonResult List(SearchAC_OperateLogModel logmodel)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            logmodel.PagedIndex =logmodel.PagedIndex==0? 0:logmodel.PagedIndex;
            logmodel.PagedSize = logmodel.PagedSize == 0 ? 10 : logmodel.PagedSize;

            //加一天是为查询最后一天的数据
            logmodel.EndOperateTime = logmodel.EndOperateTime == null ? DateTime.Now.AddDays(1) : logmodel.EndOperateTime.Value.AddDays(1);

            //系统操作日志
            var result = this._acOperateLogService.GetAC_OperateLogList(logmodel);
            List<AC_OperateLogModel> ds = result.Data;
            var data = new
            {
                rows = ds,
                total =result.Data.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create(int? id)
        {
            AC_OperateLogModel model = null;
            if (id.HasValue)
            {
                var result = this._acOperateLogService.GetAC_OperateLogById(id.Value);
                if (result.Data != null)
                {

                    model = result.Data[0];
                }


            }
            if (model == null)
            {
                model = new AC_OperateLogModel();
               
            }
            return View(model);
        }


        public string DeleteAC_OperateLog(int? OperateID)
        {
            AC_OperateLogModel model = new AC_OperateLogModel();
            if (OperateID.HasValue)
            {
                model.OperateID = OperateID.Value;
                var result = this._acOperateLogService.DeleteAC_OperateLog(model).Data;
                return result == true ? "Delete success！" : "Delete failed！";
            }

            return "Delete failed！";
            
        }

        public JsonResult DaYinAC_OperateLog(SearchAC_OperateLogModel logmodel)
        {
            var resultModel = new ResultModel();
            try
            {
                
                logmodel.PagedIndex = logmodel.PagedIndex == 0 ? 0 : logmodel.PagedIndex;
                logmodel.PagedSize = logmodel.PagedSize == 0 ? 10 : logmodel.PagedSize;

                //加一天是为查询最后一天的数据
                logmodel.EndOperateTime = logmodel.EndOperateTime == null ? DateTime.Now.AddDays(1) : logmodel.EndOperateTime.Value.AddDays(1);

                //系统操作日志
                var result = this._acOperateLogService.GetAC_OperateLogList(logmodel);
                List<AC_OperateLogModel> ds1 = result.Data;


                ObjesToPdf.CovertPdfObject(ds1, "log" + DateTime.Now.ToString("yyMMddhhmmss"), 6);
                
                resultModel.IsValid = true;
                resultModel.Messages = new List<string> { "Print success" };
            }
            catch (Exception ex)
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { ex.Message };
               
            }
            

            return Json(resultModel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ImgUpload()
        {
            
            return View();
        }

        /// <summary>
        /// 上传图片（商品）
        /// </summary>
        /// <returns></returns>

        public string GetUpLoad()
        {
            var postFile = Request.Files["upLoad"];
            
            var fd = uf.UploadFileCommon(postFile);

            var imgurl = fd.name;

            return imgurl;
        }
	}


}