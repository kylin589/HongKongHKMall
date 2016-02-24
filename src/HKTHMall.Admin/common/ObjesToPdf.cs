using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//******************************************
//引入的命名空间
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Threading;



using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;


using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Services.Orders;
using HKTHMall.Services.Users;
using Autofac;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.YH;
using HKTHMall.Core.Utils;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Admin.common;
using HKTHMall.Domain.Enum;




//******************************************

namespace HKTHMall.Admin.common
{
    public class ObjesToPdf
    {
        static IOrderService orderService = BrCms.Framework.Infrastructure.BrEngineContext.Current.Resolve<IOrderService>();

        private static System.Drawing.Bitmap bitmap;
        private static string url;
        private static int w = 1006, h = 2400;//A4纸张对应的分辨率大概就是760*900

        public static void setBitmap()
        {
            using (WebBrowser wb = new WebBrowser())
            {
                wb.Width = w;
                //wb.Height = h;
                wb.ScrollBarsEnabled = false;
                wb.Navigate(url);
                //确保页面被解析完全
                while (wb.ReadyState != WebBrowserReadyState.Complete)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                h = wb.Document.Body.ScrollRectangle.Height;
                wb.Height = h + 2;
                bitmap = new System.Drawing.Bitmap(w, h);
                wb.DrawToBitmap(bitmap, new System.Drawing.Rectangle(1, 0, w, h));
                wb.Dispose();
            }
        }
        public static void CreatPdf(string httpurl, string orderId)
        {
            url = httpurl;//"http://192.168.16.222:8805/AC_User/SelectDaYin?orderId=5728519968";
            Thread thread = new Thread(new ThreadStart(setBitmap));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            while (thread.IsAlive)
                Thread.Sleep(100);
            //"E:\\个人项目\\PDF打印\\WebApplication1\\WebApplication1\\pdf\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bmp"
            bitmap.Save(System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + orderId + ".bmp"));

            Document doc = new Document(new iTextSharp.text.Rectangle(760, Convert.ToInt16(h * 0.75) + 10), 0, 0, 10, 0);
            //Document doc = new Document(PageSize.A4, 10, 10, 10, 10);//左右上下
            MemoryStream ms = new MemoryStream();
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                writer.CloseStream = false;
                doc.Open();



                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp);
                img.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                //img.ScalePercent(70f);//560 630 75 配对 760, 1500

                img.ScalePercent(75f);
                //img.ScaleToFit(800f, 900f);//50000f, 1700f
                doc.Add(img);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                doc.Close();
                var strFileName = orderId + ".pdf";
                var path = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath")) + strFileName;

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    ms.Position = 0;
                    byte[] bit = new byte[ms.Length];
                    ms.Read(bit, 0, (int)ms.Length);
                    fs.Write(bit, 0, bit.Length);
                }
                String FullFileName = path; //HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; //System.Web.HttpContext.Current.Server.MapPath(strFileName);
                FileInfo DownloadFile = new FileInfo(FullFileName);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
                    + System.Web.HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));//DownloadFile.FullName
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                //ViewPdf(ms);
            }
        }


        private static void CreatPdf()
        {
            Document doc = new Document(PageSize.A4, 10, 10, 10, 10);//左右上下
            MemoryStream ms = new MemoryStream();
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                writer.CloseStream = false;
                doc.Open();
                url = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("htmlPath"));
                Thread thread = new Thread(new ThreadStart(setBitmap));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                while (thread.IsAlive)
                    Thread.Sleep(100);
                bitmap.Save(System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bmp"));

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp);

                //img.ScalePercent(50);//560 630 75
                //img. = 3500;
                img.ScaleToFit(10000f, 1200f);//50000f, 1700f
                doc.Add(img);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                doc.Close();
                var strFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                var path = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath")) + strFileName;

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    ms.Position = 0;
                    byte[] bit = new byte[ms.Length];
                    ms.Read(bit, 0, (int)ms.Length);
                    fs.Write(bit, 0, bit.Length);
                }
                String FullFileName = path; //HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; //System.Web.HttpContext.Current.Server.MapPath(strFileName);
                FileInfo DownloadFile = new FileInfo(FullFileName);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
                    + System.Web.HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));//DownloadFile.FullName
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                //ViewPdf(ms);
            }
        }

        private void ViewPdf(Stream fs)
        {
            
            //Response.Clear();
            
            //Response.AddHeader("Content-Disposition", "attachment;FileName=out.pdf");
            //Response.AddHeader("Content-Length", fs.Length.ToString());
            //Response.ContentType = "application/pdf";
            //long fileLength = fs.Length;
            //int size = 10240;//10K一--分块下载，10K为1块
            //byte[] readData = new byte[size];
            //if (size > fileLength)
            //    size = Convert.ToInt32(fileLength);
            //long fPos = 0;
            //bool isEnd = false;
            //while (!isEnd)
            //{
            //    if ((fPos + size) >= fileLength)
            //    {
            //        size = Convert.ToInt32(fileLength - fPos);
            //        isEnd = true;
            //    }
            //    readData = new byte[size];
            //    fs.Position = fPos;
            //    fs.Read(readData, 0, size);
            //    Response.BinaryWrite(readData);
            //    Response.OutputStream.Flush();
            //    fPos += size;
            //}
            //fs.Close();
            //Response.OutputStream.Close();
            //Response.End();//非常重要，没有这句的话，页面的HTML代码将会保存到文件中
            //Response.Close();
        }



        /// <summary>
        /// 打印PDF
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="PDFFileName">PDF名称</param>
        /// <param name="FontPath">字体路径（系统安装默认c:\\WINDOWS\\Fonts\\msyh.ttf）</param>
        /// <param name="FontSize">字体大小（6）</param>
        public static void CovertPdfObject(List<AC_OperateLogModel> list, string PDFFileName, float FontSize)
        {
            var FontPath = HKSJ.Common.ConfigHelper.GetConfigString("FontPath");



            if (list != null || list.Count > 0)
            {
                //在服务器端保存PDF时的文件名
                string strFileName = PDFFileName + ".pdf";
                var path = HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; //HttpContext.Current.Server.MapPath(strFileName);
                // GridView的所有数据全输出
                //pobjGrdv.AllowPaging = false;
                //**************************
                var properties = list[0].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                int countColumns = properties.Length;
                int countRows = list.Count;
                if (countRows != 0)
                {
                    //初始化一个目标文档类        
                    //Document document = new Document();
                    //竖排模式,大小为A4，四周边距均为25
                    Document document = new Document(PageSize.A4, 0, 0, 10, 0);
                    //横排模式,大小为A4，四周边距均为50
                    //Document doc = new Document(PageSize.A4.rotate(),50,50,50,50);
                    //调用PDF的写入方法流
                    //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
                    PdfWriter writer = PdfWriter.GetInstance(document,
                        new FileStream(path, FileMode.Create));
                    try
                    {
                        //创建PDF文档中的字体
                        BaseFont baseFont = BaseFont.CreateFont(
                          FontPath,
                          BaseFont.IDENTITY_H,
                          BaseFont.NOT_EMBEDDED);
                        //根据字体路径和字体大小属性创建字体
                        Font font = new Font(baseFont, FontSize);
                        // 添加页脚
                        //HeaderFooter footer = new HeaderFooter(new Phrase(footertxt), true);
                        //HeaderFooter footer = new HeaderFooter(new Phrase("-- ", font), new Phrase(" --", font));
                        //footer.Border = Rectangle.NO_BORDER;        // 不显示两条横线
                        //footer.Alignment = Rectangle.ALIGN_CENTER;  // 让页码居中
                        //document.Footer = footer;
                        //打开目标文档对象
                        document.Open();

                        //var p = new Paragraph("这是一个缩进演示：段落是一系列块和（或）短句。同短句一样，段落有确定的间距。用户还可以指定缩排；在边和（或）右边保留一定空白，段落可以左对齐、右对齐和居中对齐。添加到文档中的每一个段落将自动另起一行。说明：一个段落有一个且仅有一个间距，如果你添加了一个不同字体的短句或块，原来的间距仍然有效，你可以通过SetLeading来改变间距，但是段落中所有内容将使用新的中的间距。更改分割符 通常，当文本不能放在一行时，文本将被分割成不同的部分，iText首先会查找分割符，如果没有找到，文本将在行尾被截断。有一些预定的分割符如“ ”空格和“-”连字符，但是你可以使用setSplitCharacter方法来覆盖这些默认值。", font);

                        //p.Alignment = Element.ALIGN_JUSTIFIED;

                        //p.IndentationLeft = 12;

                        //p.IndentationRight = 24;

                        //p.FirstLineIndent = 24;

                        //document.Add(p);



                        //p = new Paragraph("段落是一系列块和（或）短句。同短句一样，段落有确定的间距。用户还可以指定缩排；在边和（或）右边保留一定空白，段落可以左对齐、右对齐和居中对齐。添加到文档中的每一个段落将自动另起一行。说明：一个段落有一个且仅有一个间距，如果你添加了一个不同字体的短句或块，原来的间距仍然有效，你可以通过SetLeading来改变间距，但是段落中所有内容将使用新的中的间距。更改分割符 通常，当文本不能放在一行时，文本将被分割成不同的部分，iText首先会查找分割符，如果没有找到，文本将在行尾被截断。有一些预定的分割符如“ ”空格和“-”连字符，但是你可以使用setSplitCharacter方法来覆盖这些默认值。", font);

                        //p.Alignment = Element.ALIGN_JUSTIFIED;
                        //document.Add(p);

                        //p = new Paragraph("---段落是一系列块和（或）短句。同短句一样，段落有确定的间距。用户还可以指定缩排；在边和（或）右边保留一定空白，段落可以左对齐、右对齐和居中对齐。添加到文档中的每一个段落将自动另起一行。说明：一个段落有一个且仅有一个间距，如果你添加了一个不同字体的短句或块，原来的间距仍然有效，你可以通过SetLeading来改变间距，但是段落中所有内容将使用新的中的间距。更改分割符 通常，当文本不能放在一行时，文本将被分割成不同的部分，iText首先会查找分割符，如果没有找到，文本将在行尾被截断。有一些预定的分割符如“ ”空格和“-”连字符，但是你可以使用setSplitCharacter方法来覆盖这些默认值。", font);

                        //p.Alignment = Element.ALIGN_JUSTIFIED;



                        //document.Add(p);

                        ////var c = new Chunk("");

                        //p = new Paragraph("订单列表", font);
                        //p.Alignment = Element.ALIGN_JUSTIFIED;

                        //p.IndentationLeft = 100;

                        //p.IndentationRight = 100;

                        //p.FirstLineIndent = 24;

                        //document.Add(p);

                        //**************************
                        //根据数据表内容创建一个PDF格式的表
                        PdfPTable table = new PdfPTable(countColumns);
                        //iTextSharp.text.Table table = new iTextSharp.text.Table(pobjGrdv.Columns.Count);
                        // 添加表头
                        // 设置表头背景色 
                        //table.DefaultCell.BackgroundColor = Color.GRAY;  // OK
                        //table.DefaultCell.BackgroundColor = 
                        //(iTextSharp.text.Color)System.Drawing.Color.FromName("#3399FF"); // NG
                        //table.DefaultCell.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY;
                        //table.DefaultCell.BackgroundColor = System.Drawing.Color.DodgerBlue;  
                        // 添加表头
                        //for (int j = 0; j < countColumns; j++)
                        //{
                        //    table.AddCell(new Phrase("测试", font));    // OK
                        //}
                        foreach (var item in properties)
                        {
                            table.AddCell(new Phrase(item.Name, font));
                        }
                        // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
                        table.HeaderRows = 1;
                        // 添加数据
                        // 设置表体背景色
                        //table.DefaultCell.BackgroundColor = Color.WHITE;
                        //遍历原gridview的数据行
                        for (int i = 0; i < countRows; i++)
                        {
                            var properties1 = list[i].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            foreach (var item in properties1)
                            {
                                if (item.PropertyType.ToString().ToLower().Contains("websolution.classlibrary.module")) { continue; }
                                //if (item.Name.StartsWith("_")) { continue; }
                                //if (item.Name.StartsWith("v_")) { continue; }
                                string strType = item.PropertyType.ToString().ToLower();
                                string strValue = "null";

                                if (item.GetValue(list[i], null) == null)
                                {
                                    strValue = "null";
                                }
                                else
                                {
                                    strValue = item.GetValue(list[i], null).ToString();
                                }
                                table.AddCell(new Paragraph(strValue, font));
                            }
                            //for (int j = 0; j < countColumns; j++)
                            //{
                            //    table.AddCell(new Phrase(list[i].ToString(), font));
                            //}
                        }
                        //在目标文档中添加转化后的表数据
                        document.Add(table);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        //关闭目标文件
                        document.Close();
                        //关闭写入流
                        writer.Close();
                    }
                    // 弹出提示框，提示用户是否下载保存到本地
                    try
                    {
                        //这里是你文件在项目中的位置,根目录下就这么写 
                        String FullFileName = HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; //System.Web.HttpContext.Current.Server.MapPath(strFileName);
                        FileInfo DownloadFile = new FileInfo(FullFileName);
                        System.Web.HttpContext.Current.Response.Clear();
                        System.Web.HttpContext.Current.Response.ClearHeaders();
                        System.Web.HttpContext.Current.Response.Buffer = false;
                        System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                        System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
                            + System.Web.HttpUtility.UrlEncode(DownloadFile.FullName, System.Text.Encoding.UTF8));
                        System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                        System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        System.Web.HttpContext.Current.Response.Flush();
                        System.Web.HttpContext.Current.Response.End();
                    }
                }
                else
                {
                    System.Web.HttpContext.Current.Response.Write
                        ("<script type='text/javascript'>alert('数据为空，不值得导出pdf！');</script>");
                }
            }
        }

        /// <summary>
        /// 订单打印
        /// </summary>
        /// <param name="orderId"></param>
        public static void OrderPdf(string orderId, int langType)
        {
            try
            {


                dynamic order;
                YH_MerchantInfoView YH_MerchantInfo;
                string orderStatusStr;
                string payTypeStr;
                Orderinfo(orderId, langType, out order, out YH_MerchantInfo, out orderStatusStr, out payTypeStr);


                //1中文,2英语,3泰语
                switch (langType)
                {
                    case 1:

                        break;
                    case 2:
                        //pdf 打印方法
                        
                        CovertPdfObject(order, YH_MerchantInfo, orderStatusStr, payTypeStr, 10, langType);
                        break;
                    case 3:
                        //pdf 打印方法
                        //CovertPdfObjectThai(order, YH_MerchantInfo, orderStatusStr, payTypeStr, 6, langType);
                        //CreatPdf();
                        //DownloadPdf();
                        CovertPdfObjectEnThai(order, YH_MerchantInfo, orderStatusStr, payTypeStr, 16, langType);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                var opera = string.Format("生成PDF:{0},操作结果:{1}", ex.Message, "失败");
                LogPackage.InserAC_OperateLog(opera, "PDF");

            }
        }

        public static void Orderinfo(string orderId, int langType, out dynamic order, out YH_MerchantInfoView YH_MerchantInfo, out string orderStatusStr, out string payTypeStr)
        {
            #region 订单信息和商家信息


            long orderIds = Convert.ToInt64(orderId);
            //订单信息
            order = orderService.GetOrderDetails(orderIds).Data[0];

            ////订单分页详情(商品的信息)
            //var OrderDetailslist = orderService.GetPagingOrderDetails(Convert.ToInt64(orderId), langType).Data;




            //商家信息
            List<YH_MerchantInfoView> list = orderService.GetYH_MerchantInfoByMerchantID(order.MerchantID, 2).Data;
            YH_MerchantInfo = new YH_MerchantInfoView();
            if (list.Count > 0)
            {
                YH_MerchantInfo = list[0];
            }
            orderStatusStr = ML_OrderStatus.GetLocalOrderStatusDescription(ACultureHelper.GetLanguageID, (OrderEnums.OrderStatus)order.OrderStatus);
            payTypeStr = "";
            #region MyRegion
            //if (order.OrderStatus == -1)
            //{
            //    if (langType==2)
            //    {
            //        orderStatusStr = "Invalid order";
            //    }
            //    else
            //    {
            //        orderStatusStr = "คำสั่งไม่ถูกต้อง";
            //    }
            //}
            //else if (order.OrderStatus == 2)
            //{

            //    if (langType == 2)
            //    {
            //        orderStatusStr = "Await payment";
            //    }
            //    else
            //    {
            //        orderStatusStr = "ยังไม่ชำระ";
            //    }
            //}
            //else if (order.OrderStatus == 3)
            //{

            //    if (langType == 2)
            //    {
            //        orderStatusStr = "Await delivery";
            //    }
            //    else
            //    {
            //        orderStatusStr = "ยังไม่จัดส่ง";
            //    }
            //}
            //else if (order.OrderStatus == 4)
            //{

            //    if (langType == 2)
            //    {
            //        orderStatusStr = "Await sign-off";
            //    }
            //    else
            //    {
            //        orderStatusStr = "ยังไม่ได้รับสินค้า";
            //    }
            //}
            //else if (order.OrderStatus == 5)
            //{

            //    if (langType == 2)
            //    {
            //        orderStatusStr = "Signed off";
            //    }
            //    else
            //    {
            //        orderStatusStr = "ได้รับสินค้าแล้ว";
            //    }
            //}
            //else if (order.OrderStatus == 6)
            //{

            //    if (langType == 2)
            //    {
            //        orderStatusStr = "Completed";
            //    }
            //    else
            //    {
            //        orderStatusStr = "เรียบร้อย";
            //    }
            //}
            //else if (order.OrderStatus == 7)
            //{

            //    if (langType == 2)
            //    {
            //        orderStatusStr = "Canceled";
            //    }
            //    else
            //    {
            //        orderStatusStr = "ยกเลิกแล้ว";
            //    }
            //}
            //else if (order.OrderStatus == 8)
            //{
            //    if (langType == 2)
            //    {
            //        orderStatusStr = "Deal closed";
            //    }
            //    else
            //    {
            //        orderStatusStr = "ปิดการซื้อขาย";
            //    }
            //}
            //else if (order.OrderStatus == 9)
            //{

            //    if (langType == 2)
            //    {
            //        orderStatusStr = "Canceled";
            //    }
            //    else
            //    {
            //        orderStatusStr = "คำสั่งไม่ถูกต้อง";
            //    }
            //}
            //else
            //{

            //    if (langType == 2)
            //    {
            //        orderStatusStr = "Unknown";
            //    }
            //    else
            //    {
            //        orderStatusStr = "ที่ไม่รู้จัก";
            //    }
            //} 
            #endregion

            if (order.PayChannel == 1)
            {
                if (langType == 2)
                {
                    payTypeStr = "Balance";
                }
                else
                {
                    payTypeStr = "ชำระเงินผ่านยอดคงเหลือ";
                }
            }
            else if (order.PayChannel == 2)
            {
                if (langType == 2)
                {
                    payTypeStr = "Pay by Paypal";
                }
                else
                {
                    payTypeStr = "ชำระเงินผ่านpaypal";
                }
            }
            else if (order.PayChannel == 3)
            {
                if (langType == 2)
                {
                    payTypeStr = "Pay by Credit card";
                }
                else
                {
                    payTypeStr = "ชำระเงินผ่านบัตรเครดิต";
                }
            }
            else if (order.PayChannel == 4)
            {
                if (langType == 2)
                {
                    payTypeStr = "Omise";
                }
                else
                {
                    payTypeStr = "Omise";
                }
            }
            else if (order.PayChannel == 5)
            {
                if (langType == 2)
                {
                    payTypeStr = "COD";
                }
                else
                {
                    payTypeStr = "เก็บเงินปลายทาง";
                }
            }
            else
            {
                if (langType == 2)
                {
                    payTypeStr = "Unknown";
                }
                else
                {
                    payTypeStr = "ที่ไม่รู้จัก";
                }
            }
            #endregion
        }

        

        public static void GetPdfsting(string conet)
        {
            //第一步，创建一个 iTextSharp.text.Document对象的实例：

            //Document document = new Document();

            //第二步，为该Document创建一个Writer实例：

            //PdfWriter.GetInstance(document, new FileStream("E:\\个人项目\\PDF打印\\WebApplication1\\WebApplication1\\Chap0101.pdf", FileMode.Create));

            //第三步，打开当前Document

            //document.Open();

            //第四步，为当前Document添加内容：

            //document.Add(new Paragraph(conet));

            //第五步，关闭Document

            //document.Close();
            string strFileName = "0000000.pdf";
            var path = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath")) + strFileName;
            // step 1
            Document document = new Document();
            // step 2
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
            // step 3
            document.Open();
            // step 4
            //XMLWorkerHelper.getInstance().parseXHtml(writer, document,
            //        new FileInputStream(HTML), Charset.forName("UTF-8"));
            var cell = new PdfPCell();
            document.Add(cell);
            // step 5
            document.Close();

        }

        /// <summary>
        /// 打印PDF(订单打印 英文)
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="PDFFileName">PDF名称</param>
        /// <param name="FontPath">字体路径（系统安装默认c:\\WINDOWS\\Fonts\\msyh.ttf）</param>
        /// <param name="FontSize">字体大小（6）</param>
        public static void CovertPdfObject(OrderModel model, YH_MerchantInfoView YH_MerchantInfoModel, string orderStatusStr, string payTypeStr, float FontSize, int langType)
        {
            //字体
            var FontPath = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("FontPath"));
            //logo
            var imagepath = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("ImgPath"));
            //条形码
            //var imagetxm = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("ImgPathtx"));


            if (model != null)
            {
                //在服务器端保存PDF时的文件名
                string strFileName = model.OrderID + ".pdf";

                //E:\版本\trunk\HKTHMall\src\HKTHMall.Admin\Pdf\5728473636.pdf
                var path = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath")) + strFileName; //HttpContext.Current.Server.MapPath(strFileName);

                //E:\版本\trunk\HKTHMall\src\HKTHMall.Admin\Pdf\5728473636.pdf
                //var path = HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; 
                // GridView的所有数据全输出
                //pobjGrdv.AllowPaging = false;
                //**************************
                var properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);


                //初始化一个目标文档类        
                //Document document = new Document();
                //竖排模式,大小为A4，四周边距均为25
                Document document = new Document(PageSize.A4, 0, 0, 10, 0);
                //横排模式,大小为A4，四周边距均为50
                //Document doc = new Document(PageSize.A4.rotate(),50,50,50,50);
                //调用PDF的写入方法流
                //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
                PdfWriter writer = PdfWriter.GetInstance(document,
                    new FileStream(path, FileMode.Create));
                try
                {
                    //创建PDF文档中的字体
                    BaseFont baseFont = BaseFont.CreateFont(
                      FontPath,
                      BaseFont.IDENTITY_H,
                      BaseFont.NOT_EMBEDDED);
                    //根据字体路径和字体大小属性创建字体
                    Font font = new Font(baseFont, FontSize);
                    // 添加页脚

                    //打开目标文档对象
                    document.Open();



                    //**************************
                    //根据数据表内容创建一个PDF格式的表
                    PdfPTable table = new PdfPTable(3);
                    PdfPCell cell;
                    //第一行
                    //第一列
                    cell = new PdfPCell(new Phrase("\n\n" + model.OrderDate.ToString("dd/MM/yyyy"), font));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;//控制线是否显示
                    table.AddCell(cell);


                    // 第二列
                    Image gif = Image.GetInstance(imagepath);

                    //商业打印机需要的图片最小分辨率为300dpi.为了达到这个效果，你可以将72dpi的图片缩小至原图片的24%.实际上你是将原来300像素的图片缩小为72像素：72/300 * 100 = 24%
                    gif.ScalePercent(33f);

                    cell = new PdfPCell(gif);

                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;//控制线是否显示
                    table.AddCell(cell);
                    ///Content/css/images/avatar5.png
                    //Image sourceImage = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("/Content/css/images/avatar5.png"));
                    //table.AddCell();

                    //第三列
                    cell = new PdfPCell(new Phrase("\n\nShipping Label/ Delivery Note ", font));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table.AddCell(cell);



                    //在目标文档中添加转化后的表数据
                    document.Add(table);


                    table = new PdfPTable(2);
                    //第二行
                    //第一列


                    //table.AddCell(new Phrase("Order Number >> \n------------------------", font));
                    PdfPTable table1 = new PdfPTable(1);

                    cell = new PdfPCell(new Phrase("Order Number >> \n------------------------\n\n", font));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table1.AddCell(cell);

                    //订单条形码 图片
                    //Image gif1 = Image.GetInstance(imagetxm);

                    Image gif1 = Image.GetInstance(Code.BarCode.GetBarCode.GetTxm(model.OrderID));
                    //商业打印机需要的图片最小分辨率为300dpi.为了达到这个效果，你可以将72dpi的图片缩小至原图片的24%.实际上你是将原来300像素的图片缩小为72像素：72/300 * 100 = 24%
                    gif1.ScalePercent(70f);
                    cell = new PdfPCell(gif1);
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table1.AddCell(cell);

                    //订单条形码下面的订单编码
                    Font font2 = new Font(baseFont, 10);
                    cell = new PdfPCell(new Phrase(model.OrderID.ToString() + "\n\n", font2));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table1.AddCell(cell);

                    cell = new PdfPCell(table1);
                    table.AddCell(cell);
                    //第二列
                    table.AddCell(new Phrase("Payment Type >> \n------------------------\n\n\n\n                   " + payTypeStr + "\n\n\n", font));

                    //第三行
                    //第一列
                    var str = "\n  Order Date : " + model.OrderDate.ToString("dd/MM/yyyy") + "\n\n  Customer : " + model.Receiver;
                    str += "\n\n";
                    str += "  Order No. : " + model.OrderID + "\n\n  Mobile phone : " + model.Mobile;
                    str += "\n\n";
                    str += "  Shipping address : " + model.DetailsAddress;
                    str += "\n\n";
                    cell = new PdfPCell(new Phrase(str, font));
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table.AddCell(cell);
                    //第二列
                    var str1 = "\n  Seller Name : " + YH_MerchantInfoModel.ShopName;
                    str1 += "\n\n";
                    str1 += "  Tel numbe : " + YH_MerchantInfoModel.Tel;
                    str1 += "\n\n";
                    
                    str1 += "  Address : " + YH_MerchantInfoModel.ShopAddress;

                    var p = new Paragraph(str1, font);
                   // p.Alignment = Element.ALIGN_JUSTIFIED;

                    //p.IndentationLeft = 12;

                    //p.IndentationRight = 24;

                    //p.FirstLineIndent = 44;
                    cell = new PdfPCell(p);

                    
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table.AddCell(cell);

                    Font font1 = new Font(baseFont, 12);
                    cell = new PdfPCell(new Phrase("We're looking forward to your visit again. \n\n", font1));
                    cell.Colspan = 2;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);

                    //document.Add(table);

                    //第四行
                    //第一列
                    var str2 = "\n\n  Recipient :                  ";
                    str2 += "\n\n\n";
                    str2 += "  Date of Receipt :                   ";
                    str2 += "\n\n";
                    str2 += "  ▪ Recipient has received all items in the order.";
                    str2 += "\n\n";
                    str2 += "  ▪ Items are good and subject to all Terms of Payment.";
                    str2 += "\n\n";
                    str2 += "  ▪ Ownership of the items in this document belongs to";
                    str2 += "\n\n";
                    str2 += "  Seller even if they have been in the hands of Recipient";
                    str2 += "\n\n";
                    str2 += "  who hasn't paid the bills.";
                    table.AddCell(new Phrase(str2, font));
                    //第二列
                    var str3 = "";
                    str3 += "\n\n";
                    str3 += "  ▪ Items with (*) symbol are tax-free.";
                    str3 += "\n\n";
                    str3 += "  ▪ (Y) Return items must be 100% intact.";
                    str3 += "\n\n";
                    str3 += "  ▪ (N) Reserve the right of return: Customer ";
                    str3 += "\n\n";
                    str3 += "  shall check whether the packaging of goods ";
                    str3 += "\n\n";
                    str3 += "  is intact upon receipt and has the right to ";
                    str3 += "\n\n";
                    str3 += "  reject the goods if it's broken.";
                    str3 += "\n\n";
                    str3 += "  ▪ Return of goods is available within 7 days ";
                    str3 += "\n\n";
                    str3 += "upon receipt.";
                    str3 += "\n\n";
                    str3 += "  ▪ If there is no extra charge, customer is ";
                    str3 += "\n\n";
                    str3 += "  required to pay the bills as stated in the ";
                    str3 += "\n\n";
                    str3 += "  order.";
                    str3 += "\n\n";
                    table.AddCell(new Phrase(str3, font));
                    document.Add(table);

                    //第五行 商品列表
                    table = new PdfPTable(5);
                    //表头

                    cell = new PdfPCell(new Phrase("No.\n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);

                    table.AddCell(cell);

                    //table.AddCell(new Phrase("Product picture", font));

                    cell = new PdfPCell(new Phrase("Item name.\n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Unit price\n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Qty\n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    //table.AddCell(new Phrase("Discount", font));

                    cell = new PdfPCell(new Phrase("Sub-total\n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    table.HeaderRows = 1;
                    //数据第一行

                    decimal sumSubTotal = 0;//总价格
                    int sumQuantity = 0;//总数量
                    //订单分页详情(商品的信息)
                    var list = orderService.GetPagingOrderDetails(Convert.ToInt64(model.OrderID), langType).Data as List<OrderDetailsModel>;
                    for (int i = 0; i < list.Count; i++)
                    {
                        OrderDetailsModel ordermodel = list[i];

                        cell = new PdfPCell(new Paragraph((i + 1).ToString(), font));

                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                        //var path1 = ImagePath + ordermodel.PicUrl;
                        //Image gif1 = Image.GetInstance(path1);
                        //gif1.ScalePercent(9f);
                        //cell = new PdfPCell(gif1);

                        //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        //table.AddCell(cell);


                        cell = new PdfPCell(new Paragraph(ordermodel.ProductName, font));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        cell = new PdfPCell(new Paragraph(ordermodel.SalesPrice.ToString(), font));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        cell = new PdfPCell(new Paragraph(ordermodel.Quantity.ToString(), font));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                        //table.AddCell(new Paragraph("测试", font));

                        cell = new PdfPCell(new Paragraph(ordermodel.SubTotal.ToString(), font));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);


                        sumSubTotal += ordermodel.SubTotal;
                        sumQuantity += ordermodel.Quantity;
                    }

                    document.Add(table);

                    //第六行
                    table = new PdfPTable(1);
                    var str4 = "";

                    str4 += "Total qty : " + sumQuantity + "                      Freight : 0                      Total amount : " + sumSubTotal;
                    str4 += "\n\n";
                    cell = new PdfPCell(new Phrase(str4, font1));

                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);

                    var str5 = "";
                    str5 += "  ▪ We deeply appreciate you shopping on Huika Mall (www.0066mall.com) and look forward to your ";
                    str5 += "\n\n";
                    str5 += "  ▪ visit again.";
                    str5 += "\n\n";
                    str5 += "  ▪ For details about product inspection, return policy and process, please go to Help Center at";
                    str5 += "\n\n";
                    str5 += "  ▪ www.0066mall.com.";
                    str5 += "\n\n";
                    str5 += "  ▪ Contact us at 02-695-0278, from 08:00 to 21:00 on monday to Friday ";
                    str5 += "\n\n";
                    PdfPCell cell1 = new PdfPCell(new Phrase(str5, font));

                    cell1.BackgroundColor = new BaseColor(241, 241, 241);

                    table.AddCell(cell1);

                    document.Add(table);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    //关闭目标文件
                    document.Close();
                    //关闭写入流
                    writer.Close();
                }
                // 弹出提示框，提示用户是否下载保存到本地
                try
                {
                    //这里是你文件在项目中的位置,根目录下就这么写 
                    String FullFileName = path; //HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; //System.Web.HttpContext.Current.Server.MapPath(strFileName);
                    //String FullFileName = "http://localhost/Pdf/" + strFileName;//本地IIS测试用
                    FileInfo DownloadFile = new FileInfo(FullFileName);
                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.Buffer = false;
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
                        + System.Web.HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));//DownloadFile.FullName
                    System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.End();
                }

            }
        }


        /// <summary>
        /// 打印PDF(订单打印 英文泰文混合)
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="PDFFileName">PDF名称</param>
        /// <param name="FontPath">字体路径（系统安装默认c:\\WINDOWS\\Fonts\\msyh.ttf）</param>
        /// <param name="FontSize">字体大小（6）</param>
        public static void CovertPdfObjectEnThai(OrderModel model, YH_MerchantInfoView YH_MerchantInfoModel, string orderStatusStr, string payTypeStr, float FontSize, int langType)
        {
            //字体
            var FontPath = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("FontPathThai"));
            //logo
            var imagepath = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("ImgPath"));
            //条形码
            //var imagetxm = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("ImgPathtx"));


            if (model != null)
            {
                //在服务器端保存PDF时的文件名
                string strFileName = model.OrderID + ".pdf";

                //E:\版本\trunk\HKTHMall\src\HKTHMall.Admin\Pdf\5728473636.pdf
                var path = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath")) + strFileName; //HttpContext.Current.Server.MapPath(strFileName);

                //E:\版本\trunk\HKTHMall\src\HKTHMall.Admin\Pdf\5728473636.pdf
                //var path = HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; 
                // GridView的所有数据全输出
                //pobjGrdv.AllowPaging = false;
                //**************************
                var properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);


                //初始化一个目标文档类        
                //Document document = new Document();
                //竖排模式,大小为A4，四周边距均为25
                Document document = new Document(PageSize.A4, 0, 0, 10, 0);
                //横排模式,大小为A4，四周边距均为50
                //Document doc = new Document(PageSize.A4.rotate(),50,50,50,50);
                //调用PDF的写入方法流
                //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
                PdfWriter writer = PdfWriter.GetInstance(document,
                    new FileStream(path, FileMode.Create));
                try
                {
                    //创建PDF文档中的字体
                    BaseFont baseFont = BaseFont.CreateFont(
                      FontPath,
                      BaseFont.IDENTITY_H,
                      BaseFont.NOT_EMBEDDED);
                    //根据字体路径和字体大小属性创建字体
                    Font font = new Font(baseFont, FontSize);
                    // 添加页脚

                    //打开目标文档对象
                    document.Open();



                    //**************************
                    //根据数据表内容创建一个PDF格式的表
                    PdfPTable table = new PdfPTable(3);
                    PdfPCell cell;
                    //第一行
                    //第一列
                    cell = new PdfPCell(new Phrase("\n\n" + model.OrderDate.ToString("dd/MM/yyyy"), font));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;//控制线是否显示
                    table.AddCell(cell);


                    // 第二列
                    Image gif = Image.GetInstance(imagepath);

                    //商业打印机需要的图片最小分辨率为300dpi.为了达到这个效果，你可以将72dpi的图片缩小至原图片的24%.实际上你是将原来300像素的图片缩小为72像素：72/300 * 100 = 24%
                    gif.ScalePercent(33f);

                    cell = new PdfPCell(gif);

                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;//控制线是否显示
                    table.AddCell(cell);
                    ///Content/css/images/avatar5.png
                    //Image sourceImage = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("/Content/css/images/avatar5.png"));
                    //table.AddCell();

                    //第三列
                    cell = new PdfPCell(new Phrase("\n\nShipping Label/ Delivery Note ", font));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table.AddCell(cell);



                    //在目标文档中添加转化后的表数据
                    document.Add(table);


                    table = new PdfPTable(2);
                    //第二行
                    //第一列


                    //table.AddCell(new Phrase("Order Number >> \n------------------------", font));
                    PdfPTable table1 = new PdfPTable(1);

                    cell = new PdfPCell(new Phrase("Order Number >> \n------------------------\n\n", font));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table1.AddCell(cell);

                    //订单条形码 图片
                    //Image gif1 = Image.GetInstance(imagetxm);

                    Image gif1 = Image.GetInstance(Code.BarCode.GetBarCode.GetTxm(model.OrderID));
                    //商业打印机需要的图片最小分辨率为300dpi.为了达到这个效果，你可以将72dpi的图片缩小至原图片的24%.实际上你是将原来300像素的图片缩小为72像素：72/300 * 100 = 24%
                    gif1.ScalePercent(70f);
                    cell = new PdfPCell(gif1);
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table1.AddCell(cell);

                    //订单条形码下面的订单编码
                    Font font2 = new Font(baseFont, 16);
                    cell = new PdfPCell(new Phrase(model.OrderID.ToString() + "\n\n", font2));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table1.AddCell(cell);

                    cell = new PdfPCell(table1);
                    table.AddCell(cell);
                    //第二列
                    table.AddCell(new Phrase("Payment Type >> \n------------------------\n\n                   " + payTypeStr + "\n", font));

                    //第三行
                    //第一列
                    IUserAddressService userAddressService = BrCms.Framework.Infrastructure.BrEngineContext.Current.Resolve<IUserAddressService>();
                    //省市区
                    var userAddress = userAddressService.GetTHAreaAreaName(model.THAreaID, langType).Data;

                    var str = "\n  วันที่สั่งซื้อ : " + model.OrderDate.ToString("dd/MM/yyyy") + "\n  ลูกค้า : " + model.Receiver;
                    str += "\n";
                    str += "  เลขที่ใบสั่งซื้อ : " + model.OrderID + "\n  โทรศัพท์มือถือ : " + model.Mobile;
                    str += "\n";
                    str += "  ที่อยู่ : " + userAddress + model.DetailsAddress;
                    str += "\n";
                    cell = new PdfPCell(new Phrase(str, font));
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table.AddCell(cell);
                    //第二列
                    var str1 = "\n  Seller Name : " + YH_MerchantInfoModel.ShopName;
                    str1 += "\n";
                    str1 += "  Tel numbe : " + YH_MerchantInfoModel.Tel;
                    str1 += "\n";

                    str1 += "  Address : " + YH_MerchantInfoModel.ShopAddress;

                    var p = new Paragraph(str1, font);
                    // p.Alignment = Element.ALIGN_JUSTIFIED;

                    //p.IndentationLeft = 12;

                    //p.IndentationRight = 24;

                    //p.FirstLineIndent = 44;
                    cell = new PdfPCell(p);


                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table.AddCell(cell);

                    Font font1 = new Font(baseFont, 16);
                    cell = new PdfPCell(new Phrase("We're looking forward to your visit again. \n\n", font1));
                    cell.Colspan = 2;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);

                    //document.Add(table);

                    //第四行
                    //第一列
                    var str2 = "\n  Recipient :                  ";
                    str2 += "\n\n";
                    str2 += "  Date of Receipt :                   ";
                    str2 += "\n\n";
                    str2 += "  1. ผู้รับสินค้าควรตรวจสอบสินค้าตามรายการทั้งหมด";
                    str2 += "\n";
                    str2 += "  ว่าอยู่ในสภาพเรียบร้อยก่อนรับสินค้าและยอมชำระ";
                    str2 += "\n";
                    str2 += "  เงินตามเงื่อนไขทุกประการ";
                    str2 += "\n";
                    str2 += "  2. สินค้าตามเอกสารฉบันนี้ แม้จะได้ส่งมอบแก่ผู้ซื้อ";
                    str2 += "\n";
                    str2 += "  แล้วยังคงเป็นทรัพย์สินของผู้ขายจนกว่าผู้ซื้อจะ";
                    str2 += "\n";
                    str2 += "  ได้ชำระเงินเสร็จเรียบร้อยแล้ว";
                    //str2 += "\n";
                    //str2 += "  ▪ Recipient has received all items in the order.";
                    //str2 += "\n";
                    //str2 += "  ▪ Items are good and subject to all Terms of Payment.";
                    //str2 += "\n";
                    //str2 += "  ▪ Ownership of the items in this document belongs to";
                    //str2 += "\n";
                    //str2 += "  Seller even if they have been in the hands of Recipient";
                    str2 += "\n\n";
                    //str2 += "  who hasn't paid the bills.";
                    table.AddCell(new Phrase(str2, font));
                    //第二列
                    var str3 = "";
                    
                    str3 += "\n";
                    str3 += "  3. สามารถคืนสินค้าได้ภายใน7วันหรือเปลี่ยนสินค้าได้";
                    str3 += "\n";
                    str3 += "  ภายใน15วันนับจากวันที่ผู้ซื้อได้รับสินค้าแต่ด้วย";
                    str3 += "\n";
                    str3 += "  สาเหตุส่วนบุคคลและสินค้าอยู่ในสภาพเรียบร้อยอนุญาต";
                    str3 += "\n";
                    str3 += "  ให้คืนสินค้าโดยลูกค้ายอมออกค่าจัดส่งเองแต่ไม่อนุญาต";
                    str3 += "\n";
                    str3 += "  ให้เปลียนสินค้า";
                    //str3 += "\n\n";
                    //str3 += "  แต่ด้วยสาเหตุส่วนบุคคลและสินค้าอยู่ในสภาพเรียบร้อย อนุญาตให้คืนสินค้าโดยลูกค้ายอมออกค่าจัดส่งเอง แต่ไม่อนุญาตให้เปลียนสินค้า ";
                    str3 += "\n";
                    str3 += "  4. อนุญาตให้เปลียน-คืนสินค้าโดยไม่มีค่าใช้จ่าย";
                    str3 += "\n";
                    str3 += "  เพิ่มเติมใดๆในกรณีที่สินค้าเสียหายก่อนถึงมือ";
                    str3 += "\n";
                    str3 += "  ลูกค้าและ/หรืออยู่ในบรรจุภัณฑ์ที่ชำรุดหรือ";
                    str3 += "\n";
                    str3 += "  สินค้าไม่ตรงกับคำบรรยายบนเว็บไซต์";
                    str3 += "\n\n";
                    //str3 += "  reject the goods if it's broken.";
                    //str3 += "\n\n";
                    //str3 += "  ▪ Return of goods is available within 7 days ";
                    //str3 += "\n\n";
                    //str3 += "upon receipt.";
                    //str3 += "\n\n";
                    //str3 += "  ▪ If there is no extra charge, customer is ";
                    //str3 += "\n\n";
                    //str3 += "  required to pay the bills as stated in the ";
                    //str3 += "\n\n";
                    //str3 += "  order.";
                    //str3 += "\n\n";
                    table.AddCell(new Phrase(str3, font));
                    document.Add(table);

                    //第五行 商品列表
                    float[] widths = { 8f, 51f, 15f, 8f, 18f };
                    table = new PdfPTable(widths);
                    //表头
                    Font font3 = new Font(baseFont, 14);
                    cell = new PdfPCell(new Phrase("No. \n", font3));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    

                    table.AddCell(cell);

                    //table.AddCell(new Phrase("Product picture", font));

                    cell = new PdfPCell(new Phrase("Item Name \n", font3));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Unit price \n", font3));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Qty. \n", font3));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    //table.AddCell(new Phrase("Discount", font));

                    cell = new PdfPCell(new Phrase("Sub-total \n", font3));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    table.HeaderRows = 1;
                    //数据第一行

                    decimal sumSubTotal = 0;//总价格
                    int sumQuantity = 0;//总数量
                    //订单分页详情(商品的信息)
                    var list = orderService.GetPagingOrderDetails(Convert.ToInt64(model.OrderID), langType).Data as List<OrderDetailsModel>;
                    for (int i = 0; i < list.Count; i++)
                    {
                        OrderDetailsModel ordermodel = list[i];

                        cell = new PdfPCell(new Paragraph((i + 1).ToString(), font3));

                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                        //var path1 = ImagePath + ordermodel.PicUrl;
                        //Image gif1 = Image.GetInstance(path1);
                        //gif1.ScalePercent(9f);
                        //cell = new PdfPCell(gif1);

                        //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        //table.AddCell(cell);


                        cell = new PdfPCell(new Paragraph(ordermodel.ProductName+" \n", font3));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        cell = new PdfPCell(new Paragraph(ordermodel.SalesPrice.ToString(), font3));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        cell = new PdfPCell(new Paragraph(ordermodel.Quantity.ToString(), font3));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                        //table.AddCell(new Paragraph("测试", font));

                        cell = new PdfPCell(new Paragraph(ordermodel.SubTotal.ToString(), font3));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);


                        sumSubTotal += ordermodel.SubTotal;
                        sumQuantity += ordermodel.Quantity;
                    }

                    document.Add(table);

                    //第六行
                    table = new PdfPTable(1);
                    var str4 = "";

                    str4 += "Total Qty. : " + sumQuantity + "                      Freight : 0                      Total amount : " + sumSubTotal;
                    str4 += "\n\n";
                    cell = new PdfPCell(new Phrase(str4, font1));

                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);

                    var str5 = "";
                    str5 += "  1. ทางเรารู้สึกขอบคุณเป็นอย่างยิ่งในการเข้ามาช็อบปิ้งกับ Huika mall (www.0066mall.com) ";
                    str5 += "\n";
                    str5 += "       และหวังว่าคุณจะกลับมาใช้บริการทางเราอีก ";
                    str5 += "\n";
                    str5 += "  2. หากมีปัญหาเกี่ยวกับการตรวจสอบข้อมูลสินค้า นโยบายการคืนสินค้าและขั้นตอนการสมัครต่าง ";
                    str5 += "\n";
                    str5 += "  หากมีปัญหาเกี่ยวกับการตรวจสอบข้อมูลสินค้า นโยบายการคืนสินค้าและขั้นตอนการสมัครต่าง ";
                    str5 += "\n  คุณสามารถติดต่อกับศูนย์ช่วยเหลือได้ที่ www.0066mall.com.";
                    //str5 += "  ▪ Contact us at 02-695-0278, from 08:00 to 21:00 on monday to Friday ";
                    str5 += "\n\n";
                    PdfPCell cell1 = new PdfPCell(new Phrase(str5, font));

                    cell1.BackgroundColor = new BaseColor(241, 241, 241);

                    table.AddCell(cell1);

                    document.Add(table);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    //关闭目标文件
                    document.Close();
                    //关闭写入流
                    writer.Close();
                }
                // 弹出提示框，提示用户是否下载保存到本地
                try
                {
                    //这里是你文件在项目中的位置,根目录下就这么写 
                    String FullFileName = path; //HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; //System.Web.HttpContext.Current.Server.MapPath(strFileName);
                    //String FullFileName = "http://localhost/Pdf/" + strFileName;//本地IIS测试用
                    FileInfo DownloadFile = new FileInfo(FullFileName);
                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.Buffer = false;
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
                        + System.Web.HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));//DownloadFile.FullName
                    System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.End();
                }

            }
        }

        /// <summary>
        /// 打印PDF(订单打印 泰)
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="PDFFileName">PDF名称</param>
        /// <param name="FontPath">字体路径（系统安装默认c:\\WINDOWS\\Fonts\\msyh.ttf）</param>
        /// <param name="FontSize">字体大小（6）</param>
        public static void CovertPdfObjectThai(OrderModel model, YH_MerchantInfoView YH_MerchantInfoModel, string orderStatusStr, string payTypeStr, float FontSize, int langType)
        {
            //字体
            var FontPath = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("FontPathThai"));
            //logo
            var imagepath = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("ImgPath"));
            //条形码
            //var imagetxm = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("ImgPathtx"));


            if (model != null)
            {
                //在服务器端保存PDF时的文件名
                string strFileName = model.OrderID + ".pdf";

                //E:\版本\trunk\HKTHMall\src\HKTHMall.Admin\Pdf\5728473636.pdf
                var path = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath")) + strFileName; //HttpContext.Current.Server.MapPath(strFileName);

                //E:\版本\trunk\HKTHMall\src\HKTHMall.Admin\Pdf\5728473636.pdf
                //var path = HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; 
                // GridView的所有数据全输出
                //pobjGrdv.AllowPaging = false;
                //**************************
                var properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);


                //初始化一个目标文档类        
                //Document document = new Document();
                //竖排模式,大小为A4，四周边距均为25
                Document document = new Document(PageSize.A4, 0, 0, 10, 0);
                //横排模式,大小为A4，四周边距均为50
                //Document doc = new Document(PageSize.A4.rotate(),50,50,50,50);
                //调用PDF的写入方法流
                //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
                PdfWriter writer = PdfWriter.GetInstance(document,
                    new FileStream(path, FileMode.Create));
                try
                {
                    //创建PDF文档中的字体
                    BaseFont baseFont = BaseFont.CreateFont(
                      FontPath,
                      BaseFont.IDENTITY_H,
                      BaseFont.NOT_EMBEDDED);
                    //根据字体路径和字体大小属性创建字体
                    Font font = new Font(baseFont, FontSize);
                    // 添加页脚

                    //打开目标文档对象
                    document.Open();



                    //**************************
                    //根据数据表内容创建一个PDF格式的表
                    PdfPTable table = new PdfPTable(3);
                    PdfPCell cell;
                    //第一行
                    //第一列
                    cell = new PdfPCell(new Phrase("\n\n" + model.OrderDate.ToString("dd/MM/yyyy"), font));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;//控制线是否显示
                    table.AddCell(cell);


                    // 第二列
                    Image gif = Image.GetInstance(imagepath);

                    //商业打印机需要的图片最小分辨率为300dpi.为了达到这个效果，你可以将72dpi的图片缩小至原图片的24%.实际上你是将原来300像素的图片缩小为72像素：72/300 * 100 = 24%
                    gif.ScalePercent(33f);

                    cell = new PdfPCell(gif);

                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;//控制线是否显示
                    table.AddCell(cell);
                    ///Content/css/images/avatar5.png
                    //Image sourceImage = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("/Content/css/images/avatar5.png"));
                    //table.AddCell();

                    //第三列
                    cell = new PdfPCell(new Phrase("\n\nรหัสสินค้า/หมายเหตุ ", font));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table.AddCell(cell);



                    //在目标文档中添加转化后的表数据
                    document.Add(table);


                    table = new PdfPTable(2);
                    //第二行
                    //第一列


                    //table.AddCell(new Phrase("Order Number >> \n------------------------", font));
                    PdfPTable table1 = new PdfPTable(1);

                    cell = new PdfPCell(new Phrase("เลขที่ใบสั่งซื้อ >> \n------------------------\n\n", font));
                    cell.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table1.AddCell(cell);

                    //订单条形码 图片
                    //Image gif1 = Image.GetInstance(imagetxm);

                    Image gif1 = Image.GetInstance(Code.BarCode.GetBarCode.GetTxm(model.OrderID));
                    //商业打印机需要的图片最小分辨率为300dpi.为了达到这个效果，你可以将72dpi的图片缩小至原图片的24%.实际上你是将原来300像素的图片缩小为72像素：72/300 * 100 = 24%
                    gif1.ScalePercent(70f);
                    cell = new PdfPCell(gif1);
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table1.AddCell(cell);

                    //订单条形码下面的订单编码
                    Font font2 = new Font(baseFont, 10);
                    cell = new PdfPCell(new Phrase(model.OrderID.ToString() + "\n\n", font2));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table1.AddCell(cell);

                    cell = new PdfPCell(table1);
                    table.AddCell(cell);
                    //第二列
                    table.AddCell(new Phrase("ช่องทางการชำระเงิน >> \n------------------------\n\n\n\n                   " + payTypeStr + "\n\n\n", font));

                    //第三行
                    //第一列
                    var str = "\n  วันที่สั่งซื้อ : " + model.OrderDate.ToString("dd/MM/yyyy") + "\n\n  ลูกค้า : " + model.Receiver;
                    str += "\n\n";
                    str += "  เลขที่ใบสั่งซื้อ. : " + model.OrderID + "\n\n  Mobile phone : " + model.Mobile;
                    str += "\n\n";
                    str += "  ที่อยู่ : " + model.DetailsAddress;
                    str += "\n\n";
                    cell = new PdfPCell(new Phrase(str, font));
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table.AddCell(cell);
                    //第二列
                    var str1 = "\n  ผู้ขาย : " + YH_MerchantInfoModel.ShopName;
                    str1 += "\n\n";
                    str1 += "  โทร : " + YH_MerchantInfoModel.Tel;
                    str1 += "\n\n";
                    str1 += "  ที่อยู่  : " + YH_MerchantInfoModel.ShopAddress;
                    cell = new PdfPCell(new Phrase(str1, font));
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    table.AddCell(cell);

                    Font font1 = new Font(baseFont, 12);
                    cell = new PdfPCell(new Phrase("We're looking forward to your visit again. \n\n", font1));
                    cell.Colspan = 2;
                    cell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;//控制线是否显示
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);

                    //document.Add(table);

                    //第四行
                    //第一列
                    var str2 = "\n\n  Recipient :                  ";
                    str2 += "\n\n\n";
                    str2 += "  Date of Receipt :                   ";
                    str2 += "\n\n";
                    str2 += "  ▪ Recipient has received all items in the order.";
                    str2 += "\n\n";
                    str2 += "  ▪ Items are good and subject to all Terms of Payment.";
                    str2 += "\n\n";
                    str2 += "  ▪ Ownership of the items in this document belongs to";
                    str2 += "\n\n";
                    str2 += "  Seller even if they have been in the hands of Recipient";
                    str2 += "\n\n";
                    str2 += "  who hasn't paid the bills.";
                    table.AddCell(new Phrase(str2, font));
                    //第二列
                    var str3 = "";
                    str3 += "\n\n";
                    str3 += "  ▪ Items with (*) symbol are tax-free.";
                    str3 += "\n\n";
                    str3 += "  ▪ (Y) Return items must be 100% intact.";
                    str3 += "\n\n";
                    str3 += "  ▪ (N) Reserve the right of return: Customer ";
                    str3 += "\n\n";
                    str3 += "  shall check whether the packaging of goods ";
                    str3 += "\n\n";
                    str3 += "  is intact upon receipt and has the right to ";
                    str3 += "\n\n";
                    str3 += "  reject the goods if it's broken.";
                    str3 += "\n\n";
                    str3 += "  ▪ Return of goods is available within 7 days ";
                    str3 += "\n\n";
                    str3 += "upon receipt.";
                    str3 += "\n\n";
                    str3 += "  ▪ If there is no extra charge, customer is ";
                    str3 += "\n\n";
                    str3 += "  required to pay the bills as stated in the ";
                    str3 += "\n\n";
                    str3 += "  order.";
                    str3 += "\n\n";
                    table.AddCell(new Phrase(str3, font));
                    document.Add(table);

                    //第五行 商品列表
                    table = new PdfPTable(5);
                    //表头

                    cell = new PdfPCell(new Phrase("เลขที่ \n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);

                    table.AddCell(cell);

                    //table.AddCell(new Phrase("Product picture", font));

                    cell = new PdfPCell(new Phrase("ชื่อสินค้า \n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("ราคา \n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(" จำนวน \n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    //table.AddCell(new Phrase("Discount", font));

                    cell = new PdfPCell(new Phrase("ยอดรวม \n\n", font1));
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BackgroundColor = new BaseColor(241, 241, 241);
                    table.AddCell(cell);

                    table.HeaderRows = 1;
                    //数据第一行

                    decimal sumSubTotal = 0;//总价格
                    int sumQuantity = 0;//总数量
                    //订单分页详情(商品的信息)
                    var list = orderService.GetPagingOrderDetails(Convert.ToInt64(model.OrderID), langType).Data as List<OrderDetailsModel>;
                    for (int i = 0; i < list.Count; i++)
                    {
                        OrderDetailsModel ordermodel = list[i];

                        cell = new PdfPCell(new Paragraph((i + 1).ToString(), font));

                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                        //var path1 = ImagePath + ordermodel.PicUrl;
                        //Image gif1 = Image.GetInstance(path1);
                        //gif1.ScalePercent(9f);
                        //cell = new PdfPCell(gif1);

                        //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        //table.AddCell(cell);


                        cell = new PdfPCell(new Paragraph(ordermodel.ProductName, font));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        cell = new PdfPCell(new Paragraph(ordermodel.SalesPrice.ToString(), font));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                        cell = new PdfPCell(new Paragraph(ordermodel.Quantity.ToString(), font));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                        //table.AddCell(new Paragraph("测试", font));

                        cell = new PdfPCell(new Paragraph(ordermodel.SubTotal.ToString(), font));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);


                        sumSubTotal += ordermodel.SubTotal;
                        sumQuantity += ordermodel.Quantity;
                    }

                    document.Add(table);

                    //第六行
                    table = new PdfPTable(1);
                    var str4 = "";

                    str4 += "จำนวนทั้งหมด : " + sumQuantity + "                      ค่าขนส่ง : 0                      รวมเงิน : " + sumSubTotal;
                    str4 += "\n\n";
                    cell = new PdfPCell(new Phrase(str4, font1));

                    cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);

                    var str5 = "";
                    //str5 += "  ▪ We deeply appreciate you shopping on Huika Mall (www.0066mall.com) and look forward to your ";
                    //str5 += "\n\n";
                    //str5 += "  ▪ visit again.";
                    str5 += "  ▪ ทางเรารู้สึกขอบคุณเป็นอย่างยิ่งในการเข้ามาช็อบปิ้งกับ Huika mall (www.0066mall.com) และหวังว่าคุณจะกลับมาใช้บริการทางเราอีก";
                    str5 += "\n\n";
                    //str5 += "  ▪ For details about product inspection, return policy and process, please go to Help Center at";
                    str5 += "  ▪ หากมีปัญหาเกี่ยวกับการตรวจสอบข้อมูลสินค้า นโยบายการคืนสินค้าและขั้นตอนการสมัครต่าง คุณสามารถติดต่อกับศูนย์ช่วยเหลือได้ที่ www.0066mall.com.";
                    str5 += "\n\n";
                    str5 += "  ▪ www.0066mall.com.";
                    str5 += "\n\n";
                    //str5 += "  ▪ Contact us at 02-695-0278, from 08:00 to 21:00 on monday to Friday ";
                    str5 += "  ▪ ติดต่อเรา：02-695-0278  ตั้งแต่วันจันทร์-วันอาทิตย์ ";
                    str5 += "\n\n";
                    PdfPCell cell1 = new PdfPCell(new Phrase(str5, font));

                    cell1.BackgroundColor = new BaseColor(241, 241, 241);

                    table.AddCell(cell1);

                    document.Add(table);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    //关闭目标文件
                    document.Close();
                    //关闭写入流
                    writer.Close();
                }
                // 弹出提示框，提示用户是否下载保存到本地
                try
                {
                    //这里是你文件在项目中的位置,根目录下就这么写 
                    String FullFileName = path; //HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; //System.Web.HttpContext.Current.Server.MapPath(strFileName);
                    //String FullFileName = "http://localhost/Pdf/" + strFileName;//本地IIS测试用
                    FileInfo DownloadFile = new FileInfo(FullFileName);
                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.Buffer = false;
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
                        + System.Web.HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));//DownloadFile.FullName
                    System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                    System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.End();
                }

            }
        }

        /// <summary>
        /// 执行此Url，下载PDF档案
        /// </summary>
        /// <returns></returns>
        public static void DownloadPdf()
        {
            url = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("htmlPath"));
            System.Net.WebClient wc = new System.Net.WebClient();
            //从网址下载Html字串
            string htmlText = wc.DownloadString(url);
            //byte[] pdfFile = this.ConvertHtmlTextToPDF(url,8);
            
            // 弹出提示框，提示用户是否下载保存到本地
            try
            {
                //这里是你文件在项目中的位置,根目录下就这么写 
                String FullFileName = ConvertHtmlTextToPDF1(htmlText, 8); //HKSJ.Common.ConfigHelper.GetConfigString("PdfPath") + strFileName; //System.Web.HttpContext.Current.Server.MapPath(strFileName);
                //String FullFileName = "http://localhost/Pdf/" + strFileName;//本地IIS测试用
                FileInfo DownloadFile = new FileInfo(FullFileName);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
                    + System.Web.HttpUtility.UrlEncode("html.pdf", System.Text.Encoding.UTF8));//DownloadFile.FullName
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }
            //return File(pdfFile, "application/pdf", "范例PDF档.pdf");
        }


        /// <summary>
        /// 将Html文字 输出到PDF档里
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public byte[] ConvertHtmlTextToPDF(string htmlText,int FontSize)
        {
            //避免当htmlText无任何html tag标签的纯文字时，转PDF时会挂掉，所以一律加上<p>标签
            htmlText = "<p>" + htmlText + "</p>";
            byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串转成byte[]
            MemoryStream msInput = new MemoryStream(data);

            MemoryStream outputStream = new MemoryStream();//要把PDF写到哪个串流
            Document doc = new Document(PageSize.A4, 10, 10, 10, 10);//要写PDF的文件，建构子没填的话预设直式A4
            try
            {
                
                
                PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
                //指定文件预设开档时的缩放为100%
                PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
                //开启Document文件 
                doc.Open();


                //使用XMLWorkerHelper把Html parse到PDF档里
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());//new UnicodeFontFactory()
                //将pdfDest设定的资料写到PDF档
                PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
                writer.SetOpenAction(action);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                doc.Close();
                msInput.Close();
                outputStream.Close();
            }
            if (string.IsNullOrEmpty(htmlText))
            {
                return null;
            }
            

            
            //回传PDF档案 
            return outputStream.ToArray();

        }

        public static string ConvertHtmlTextToPDF1(string htmlText, int FontSize)
        {
            //避免当htmlText无任何html tag标签的纯文字时，转PDF时会挂掉，所以一律加上<p>标签
            //htmlText = "<p>" + htmlText + "</p>";
            byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串转成byte[]
            MemoryStream msInput = new MemoryStream(data);

            //MemoryStream outputStream = new MemoryStream();//要把PDF写到哪个串流
            Document doc = new Document(PageSize.A4, 10, 10, 10, 10);//要写PDF的文件，建构子没填的话预设直式A4

            //在服务器端保存PDF时的文件名
            string strFileName = DateTime.Now.ToString("yyyyMMddhhmmss")+".pdf";
            //E:\版本\trunk\HKTHMall\src\HKTHMall.Admin\Pdf\5728473636.pdf
            var path = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath")) + strFileName;
            try
            {
               

                

                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                //指定文件预设开档时的缩放为100%
                PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
                //开启Document文件 
                doc.Open();


                //使用XMLWorkerHelper把Html parse到PDF档里
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());//new UnicodeFontFactory()
                //将pdfDest设定的资料写到PDF档
                PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
                writer.SetOpenAction(action);
                
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                doc.Close();
                msInput.Close();
                //outputStream.Close();
            }
            if (string.IsNullOrEmpty(htmlText))
            {
                return null;
            }



            //回传PDF档案 
            return path;

        }
    }

    
}