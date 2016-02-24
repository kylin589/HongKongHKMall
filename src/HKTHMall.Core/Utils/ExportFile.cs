using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace HKTHMall.Core.Utils
{
    public class ExportFile
    {
        /// <summary>
        /// 导出EXCEL 刘宏文
        /// </summary>
        /// <param name="FileName">文件名称</param>
        /// <param name="buffer">文件的流</param>
        public static void ExportExcel(string FileName, byte[] buffer)
        {
            // 设置编码和附件格式
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Charset = "UTF-8";
            if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("firefox") > -1)
            {
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", FileName));
            }
            else
            {
                //ie下的
                HttpContext.Current.Response.AppendHeader("Content-Disposition",
               "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
            }
            HttpContext.Current.Response.BinaryWrite(buffer);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 导出PDF 刘宏文
        /// </summary>
        /// <param name="FileName">文件名称</param>
        /// <param name="buffer">文件的流</param>
        public static void ExportPDF(string FileName, byte[] buffer)
        {
            // 设置编码和附件格式
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/octet-stream"; //"application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Charset = "UTF-8";
            if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("firefox") > -1)
            {
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", FileName));
            }
            else
            {
                //ie下的
                HttpContext.Current.Response.AppendHeader("Content-Disposition",
               "attachment;filename=" + HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
            }
            HttpContext.Current.Response.BinaryWrite(buffer);
            HttpContext.Current.Response.End();
        }

        public static void HtmlToPdf(string outputPath, string fileName, string url)
        {
            string savepath = string.Format(outputPath + "\\" + Guid.NewGuid().ToString() + ".pdf");//最终保存
            string dir = System.IO.Path.GetDirectoryName(savepath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            try
            {
                if (!string.IsNullOrEmpty(url) || !string.IsNullOrEmpty(savepath))
                {
                    Process p = new Process();
                    string resource = HttpContext.Current.Server.MapPath("/Pdf");
                    string dllstr = string.Format(resource + "\\wkhtmltopdf.exe");
                    if (System.IO.File.Exists(dllstr))
                    {
                        p.StartInfo.FileName = dllstr;
                        p.StartInfo.Arguments = " \"" + url + "\"  \"" + savepath + "\"";
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardInput = true;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.RedirectStandardError = true;
                        p.StartInfo.CreateNoWindow = true;
                        p.Start();
                        p.WaitForExit();
                        try
                        {
                            FileStream fs = new FileStream(savepath, FileMode.Open);
                            byte[] file = new byte[fs.Length];
                            fs.Read(file, 0, file.Length);
                            fs.Close();
                            File.Delete(savepath);
                            ExportFile.ExportPDF(fileName + ".pdf", file);
                        }
                        catch (Exception ee)
                        {
                            throw new Exception(ee.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public static void HtmlToPdf(string fileName, string url)
        {
            string savepath = HttpContext.Current.Server.MapPath("/Pdf/" + fileName + ".pdf"); //string.Format(outputPath + "\\" + Guid.NewGuid().ToString() + ".pdf");//最终保存
            string dir = System.IO.Path.GetDirectoryName(savepath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            try
            {
                if (!string.IsNullOrEmpty(url) || !string.IsNullOrEmpty(savepath))
                {
                    Process p = new Process();
                    string resource = HttpContext.Current.Server.MapPath("/Pdf");
                    string dllstr = string.Format(resource + "\\wkhtmltopdf.exe");
                    if (System.IO.File.Exists(dllstr))
                    {
                        p.StartInfo.FileName = dllstr;
                        p.StartInfo.Arguments = " \"" + url + "\"  \"" + savepath + "\"";
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardInput = true;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.RedirectStandardError = true;
                        p.StartInfo.CreateNoWindow = true;
                        p.Start();
                        p.WaitForExit();
                        try
                        {
                            FileStream fs = new FileStream(savepath, FileMode.Open);
                            byte[] file = new byte[fs.Length];
                            fs.Read(file, 0, file.Length);
                            fs.Close();
                            File.Delete(savepath);
                            ExportFile.ExportPDF(fileName + ".pdf", file);
                        }
                        catch (Exception ee)
                        {
                            throw new Exception(ee.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
