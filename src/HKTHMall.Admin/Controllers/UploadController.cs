using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using HKSJ.Common.FastDFS;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Admin.common;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    ///     上传控制器
    /// </summary>
    public class UploadController : HKBaseController
    {
        private string StorageRoot
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Files/")); }
            //Path should! always end with '/'
        }

        [HttpPost]
        public ActionResult UploadImage()
        {
            var result = new ResultModel
            {
                IsValid = false
            };

            try
            {
                var postFile = this.Request.Files[0]; //上传文件对象

                if (postFile != null)
                {
                    var size = postFile.ContentLength.ToString();
                }
                if (postFile != null)
                {
                    var stream = postFile.InputStream;

                    var bt = new byte[postFile.ContentLength];
                    var ms = new MemoryStream(bt);

                    stream.Read(bt, 0, bt.Length);
                    stream.Close();

                    var t = postFile.ContentType;
                    string fileExt = System.IO.Path.GetExtension(postFile.FileName);
                    if (fileExt.Length>0)
                    {
                        fileExt = fileExt.Substring(1,fileExt.Length-1);
                    }

                    var imageTypes = "image/pjpeg,image/jpeg,image/gif,image/png,image/x-png/apk/ipa";

                    if (imageTypes.IndexOf(t, StringComparison.Ordinal) < 0)
                    {
                        result.Messages.Add("Please upload jpg, gif or png format picture");
                    }
                    else if (bt.Length > 1024 * 1024)  //wuyf 添加else
                    {
                        result.Messages.Add("Upload pictures of the size of the control in the range of 1M!");
                    }
                    else
                    {
                        var fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, bt, fileExt);
                        result.IsValid = true;
                        result.Data = fileName;

                    }
                }
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Messages.Add("upload failed");
                var opera = string.Empty;

                opera = "UploadController图片上传错误 ex:" + ex.Message;
                LogPackage.InserAC_OperateLog(opera, "UploadController图片上传");
                //todo 日志记录
            }
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult UploadImages()
        {
            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in this.Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = this.Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    this.UploadWholeFile(this.Request, statuses);
                }
                else
                {
                    this.UploadPartialFile(headers["X-File-Name"], this.Request, statuses);
                }

                return this.Json(statuses);
                //var result = this.Json(statuses);
                //result.ContentType = "text/plain";

                //return result;
            }

            return this.Json(r);
        }

        private void UploadPartialFile(string fileName, HttpRequestBase request,
            List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Files.Count != 1)
                throw new HttpRequestValidationException(
                    "Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;

            var fullName = Path.Combine(this.StorageRoot, Path.GetFileName(fileName));

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(new ViewDataUploadFilesResult
            {
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = "/Home/Download/" + fileName,
                delete_url = "/Home/Delete/" + fileName,
                thumbnail_url = @"data:image/png;base64," + this.EncodeFile(fullName),
                delete_type = "GET"
            });
        }

        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            for (var i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                var fullPath = Path.Combine(this.StorageRoot, Path.GetFileName(file.FileName));

                if (!Directory.Exists(this.StorageRoot))
                {
                    Directory.CreateDirectory(this.StorageRoot);
                }

                file.SaveAs(fullPath);

                statuses.Add(new ViewDataUploadFilesResult
                {
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = "/Home/Download/" + file.FileName,
                    delete_url = "/Home/Delete/" + file.FileName,
                    thumbnail_url = @"data:image/png;base64," + this.EncodeFile(fullPath),
                    delete_type = "GET"
                });
            }
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        [HttpGet]
        public void Delete(string filename)
        {
            var filePath = Path.Combine(this.Server.MapPath("~/Files"), filename);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadFileds()
        {
            var result = new ResultModel
            {
                IsValid = false
            };

            try
            {
                int APPsize =Convert.ToInt32( HKSJ.Common.ConfigHelper.GetConfigString("APPsize"));
                if (Request.Files != null)
                {
                    HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                    if (hpf.ContentLength == 0)
                    {
                        result.Messages.Add("Please upload a content file");
                        result.IsValid = false;
                        return this.Json(result, JsonRequestBehavior.AllowGet);
                    }
                    //判断文件是否合法
                    string fileExt = System.IO.Path.GetExtension(hpf.FileName);
                    int apptype = Convert.ToInt32(Request["AppType"].Trim());
                    if (apptype == 1)
                    {
                        if (fileExt.ToUpper() != ".IPA")
                        {
                            result.Messages.Add("IOS platform, please upload the file suffix is.IPA");
                            result.IsValid = false;
                            return this.Json(result, JsonRequestBehavior.AllowGet);

                        }
                        
                    }
                    else
                    {
                        if (fileExt.ToUpper() != ".APK")
                        {
                            result.Messages.Add("Android platform, please upload the file suffix is.APK");
                            result.IsValid = false;
                            return this.Json(result, JsonRequestBehavior.AllowGet);

                        }
                    }
                    if (hpf.ContentLength > 1024 * 1024 * APPsize)  
                    {
                        result.Messages.Add("Upload pictures of the size of the control in the range of " + 1024 * 1024 * APPsize + "byte!");
                    }

                    string size = hpf.ContentLength.ToString();
                    System.IO.Stream s = hpf.InputStream;
                    byte[] bt = new byte[hpf.ContentLength];
                    System.IO.MemoryStream mes = new System.IO.MemoryStream(bt);
                    s.Read(bt, 0, bt.Length);
                    s.Close();
                    string fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, bt, fileExt.Replace(".", ""));

                    result.IsValid = true;
                    result.Data = fileName;
                    result.Messages.Add(size + "byte");

                }
            }
            catch (Exception ex)
            {
                var opera = string.Empty;
                opera = "UploadController文件上传错误 开始记录:";
                LogPackage.InserAC_OperateLog(opera, "UploadController文件上传");
                result.IsValid = false;
                result.Messages.Add("upload failed");
                

                opera = "UploadController文件上传错误 ex:" + ex.Message;
                LogPackage.InserAC_OperateLog(opera, "UploadController文件上传");
                //todo 日志记录
            }
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        //上传文件
        [HttpPost]
        [ValidateInput(false)]
        
        public ActionResult UploadFiles1()
        {
            var result = new ResultModel
            {
                IsValid = false
            };

            if (Request.Files != null)
            {

                //foreach (string file in Request.Files)
                //{
                    HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                    if (hpf.ContentLength == 0)
                    { }
                    //判断文件是否合法
                    string fileExt = System.IO.Path.GetExtension(hpf.FileName);
                    int apptype = Convert.ToInt32(Request["AppType"].Trim());
                    if (apptype == 1)
                    {
                        if (fileExt.ToUpper() != ".IPA")
                        {
                            result.Messages.Add("IOS platform, please upload the file suffix is.IPA");
                            result.IsValid = false;
                            return this.Json(result, JsonRequestBehavior.AllowGet);

                        }

                    }
                    else
                    {
                        if (fileExt.ToUpper() != ".APK")
                        {
                            result.Messages.Add("Android platform, please upload the file suffix is.APK");
                            result.IsValid = false;
                            return this.Json(result, JsonRequestBehavior.AllowGet);

                        }
                    }

                    string size = hpf.ContentLength.ToString();
                    System.IO.Stream s = hpf.InputStream;
                    byte[] bt = new byte[hpf.ContentLength];
                    System.IO.MemoryStream mes = new System.IO.MemoryStream(bt);
                    s.Read(bt, 0, bt.Length);
                    s.Close();

                    try
                    {
                        string fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, bt, fileExt.Replace(".", ""));
                        result.IsValid = true;
                        result.Data = fileName;
                        result.Messages.Add(size + "byte");
                    }
                    catch (Exception ex)
                    {
                        result.IsValid = false;
                        result.Messages.Add("upload failed");
                        var opera = string.Empty;

                        opera = "UploadController文件上传错误 ex:" + ex.Message;
                        LogPackage.InserAC_OperateLog(opera, "UploadController文件上传");
                    }
                //}
            }
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}