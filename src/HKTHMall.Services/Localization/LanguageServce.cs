using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.Localization;
using HKTHMall.Domain.Models;
using OfficeOpenXml;
using Simple.Data;
using HKTHMall.Core;

namespace HKTHMall.Services.Localization
{
    public class LanguageServce : BaseService, ILanguageService
    {
        public ResultModel Add(AddLanguageModel model)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();
                var lang = db.MultiLanguage.Find(db.MultiLanguage.LangKey == model.LangKey && db.MultiLanguage.DataType == model.DataType);
                if (lang == null)
                {
                    result.Data = db.MultiLanguage.Insert(
                        model
                        );
                    MemCacheFactory.GetCurrentMemCache().ClearCache("languageWeb");//刘宏文加
                }
                else
                {
                    result.IsValid = false;
                    result.Messages.Add("Added with the same key.");//添加了具有相同的键
                }
                return result;
            });
        }

        public ResultModel Update(UpdateLanguageModel model)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();
                var lang = db.MultiLanguage.Find(db.MultiLanguage.LangKey == model.LangKey && db.MultiLanguage.DataType == model.DataType);
                if (lang == null || lang.ID == model.ID)
                {
                    result.Data = db.MultiLanguage.Update(
                        model
                        );
                    MemCacheFactory.GetCurrentMemCache().ClearCache("languageWeb");//刘宏文加
                }
                else
                {
                    result.IsValid = false;
                    result.Messages.Add("The keys to change already exist.");//修改的键已存在
                }
                return result;
            });
        }

        public ResultModel Delete(List<long> ids)
        {
            MemCacheFactory.GetCurrentMemCache().ClearCache("languageWeb");//刘宏文加
            return new ResultModel
            {
                Data = this._database.RunQuery(db =>
                    db.MultiLanguage.Delete(
                        ID: ids.ToArray()
                        )
                    )
            };
        }

        public ResultModel Export(List<long> ids, bool isAll = false)
        {




            var result = new ResultModel
            {
                Data = this._database.RunQuery(db =>
                {
                    IList<LanguageModel> list;

                    if (!isAll)
                    {
                        list = db.MultiLanguage
                            .Query()
                            .Where(db.MultiLanguage.ID == ids)
                            .ToList<LanguageModel>();
                    }
                    else
                    {
                        list = db.MultiLanguage.All().ToList<LanguageModel>();
                    }

                    using (var stream = new MemoryStream())
                    {
                        using (var xlPackage = new ExcelPackage(stream))
                        {
                            var worksheet = xlPackage.Workbook.Worksheets.Add("sheet1");

                            var properties = new[]
                            {
                                "变量名称",
                                "zh-CN",
                                "zh_HK",//"th-TH",update by liujc
                                "en-US",
                                "remark",
                                "type"
                            };

                            //worksheet.Cells.Style.ShrinkToFit = true;
                            worksheet.Cells.Style.Font.Name = "宋体";
                            worksheet.Cells.Style.Font.Size = 11;

                            for (var i = 0; i < properties.Length; i++)
                            {
                                worksheet.Cells[1, i + 1].Value = properties[i];
                                //worksheet.View.FreezePanes(1, i + 1);
                                //worksheet.Cells[1, i + 1].Style.Locked = true;
                            }

                            var row = 2;

                            foreach (var languageModel in list)
                            {
                                var col = 1;

                                worksheet.Cells[row, col].Value = languageModel.LangKey;
                                col++;

                                worksheet.Cells[row, col].Value = languageModel.NameChs;

                                col++;
                                worksheet.Cells[row, col].Value = languageModel.NameHK;
                                //worksheet.Cells[row, col].Value = languageModel.NameTH;

                                col++;
                                worksheet.Cells[row, col].Value = languageModel.NameEng;

                                col++;
                                worksheet.Cells[row, col].Value = languageModel.NameOther;
                                col++;
                                worksheet.Cells[row, col].Value = languageModel.DataType;
                                row++;
                            }

                            xlPackage.Save();
                        }

                        return stream.ToArray();
                    }
                })
            };
            return result;
        }

        /// <summary>
        /// 根据条件导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Export(SearchLanguageModel model)
        {

            SimpleExpression whereExpr = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (!string.IsNullOrEmpty(model.KeyWord))
            {
                whereExpr = new SimpleExpression(whereExpr, this._database.Db.MultiLanguage.LangKey.Like("%" + model.KeyWord + "%"), SimpleExpressionType.And);
            }
            if (model.DataType.HasValue)
            {
                whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(this._database.Db.MultiLanguage.DataType, model.DataType.Value, SimpleExpressionType.Equal), SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data = this._database.RunQuery(db =>
                {
                    IList<LanguageModel> list = db.MultiLanguage
                            .Query()
                            .Where(whereExpr)
                            .ToList<LanguageModel>();


                    using (var stream = new MemoryStream())
                    {
                        using (var xlPackage = new ExcelPackage(stream))
                        {
                            var worksheet = xlPackage.Workbook.Worksheets.Add("sheet1");

                            var properties = new[]
                            {
                                "变量名称",
                                "zh-CN",
                                "zh_HK",//"th-TH",update by liujc
                                "en-US",
                                "remark",
                                "type"
                            };

                            //worksheet.Cells.Style.ShrinkToFit = true;
                            worksheet.Cells.Style.Font.Name = "宋体";
                            worksheet.Cells.Style.Font.Size = 11;

                            for (var i = 0; i < properties.Length; i++)
                            {
                                worksheet.Cells[1, i + 1].Value = properties[i];
                                //worksheet.View.FreezePanes(1, i + 1);
                                //worksheet.Cells[1, i + 1].Style.Locked = true;
                            }

                            var row = 2;

                            foreach (var languageModel in list)
                            {
                                var col = 1;

                                worksheet.Cells[row, col].Value = languageModel.LangKey;
                                col++;

                                worksheet.Cells[row, col].Value = languageModel.NameChs;

                                col++;
                                //worksheet.Cells[row, col].Value = languageModel.NameTH;
                                worksheet.Cells[row, col].Value = languageModel.NameHK;//update by liujc

                                col++;
                                worksheet.Cells[row, col].Value = languageModel.NameEng;

                                col++;
                                worksheet.Cells[row, col].Value = languageModel.NameOther;
                                col++;
                                worksheet.Cells[row, col].Value = languageModel.DataType;
                                row++;
                            }

                            xlPackage.Save();
                        }

                        return stream.ToArray();
                    }
                })
            };
            return result;
        }

        public ResultModel ImportExcel(Stream xlsxStream, string createBy)
        {
            using (var xlPackage = new ExcelPackage(xlsxStream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new Exception("No worksheet found");

                var properties = new[]
                {
                    "变量名称",
                    "zh-CN",
                    "th-TH",
                    "zh-HK",
                    "en-US",
                    "remark",
                    "type"
                };

                var list = new List<LanguageModel>();
                //var errorList = new List<LanguageModel>();

                var iRow = 2;

                while (true)
                {
                    var allColumnsAreEmpty = true;
                    for (var i = 1; i <= properties.Length; i++)
                    {
                        if (worksheet.Cells[iRow, i].Value != null &&
                            !string.IsNullOrEmpty(worksheet.Cells[iRow, i].Value.ToString()))
                        {
                            allColumnsAreEmpty = false;
                            break;
                        }
                    }

                    if (allColumnsAreEmpty)
                        break;

                    var langKey = worksheet.Cells[iRow, this.GetColumnIndex(properties, "变量名称")].GetValue<string>();

                    if (!list.Any(m => m.LangKey.Equals(langKey)))
                    {
                        list.Add(new LanguageModel
                        {
                            CreateBy = createBy,
                            CreateDT = DateTime.Now,
                            LangKey = langKey,
                            NameChs = worksheet.Cells[iRow, this.GetColumnIndex(properties, "zh-CN")].GetValue<string>(),
                            NameTH = "",//update by liujc
                            NameHK = worksheet.Cells[iRow, this.GetColumnIndex(properties, "th-HK")].GetValue<string>(),//update by liujc
                            NameEng = worksheet.Cells[iRow, this.GetColumnIndex(properties, "en-US")].GetValue<string>(),
                            NameOther =
                                worksheet.Cells[iRow, this.GetColumnIndex(properties, "remark")].GetValue<string>(),
                            DataType = int.Parse(worksheet.Cells[iRow, this.GetColumnIndex(properties, "type")].GetValue<string>().Trim()),
                            UpdateBy = createBy,
                            UpdateDT = DateTime.Now
                        });
                    }
                    //else
                    //{
                    //    errorList.Add(new LanguageModel()
                    //    {
                    //        CreateBy = createBy,
                    //        CreateDT = DateTime.Now,
                    //        LangKey = langKey,
                    //        NameChs = worksheet.Cells[iRow, this.GetColumnIndex(properties, "zh-CN")].GetValue<string>(),
                    //        NameTH = worksheet.Cells[iRow, this.GetColumnIndex(properties, "th-TH")].GetValue<string>(),
                    //        NameEng = worksheet.Cells[iRow, this.GetColumnIndex(properties, "en-US")].GetValue<string>(),
                    //        NameOther =
                    //            worksheet.Cells[iRow, this.GetColumnIndex(properties, "remark")].GetValue<string>(),
                    //        UpdateBy = createBy,
                    //        UpdateDT = DateTime.Now
                    //    });
                    //}

                    iRow++;
                }
                MemCacheFactory.GetCurrentMemCache().ClearCache("languageWeb");//刘宏文加
                return this._database.RunQuery(db =>
                {
                    var result = new ResultModel();
                    List<UpdateLanguageModel> oldList = db.MultiLanguage
                        .Query()
                        .Where(db.MultiLanguage.LangKey == list.Select(m => m.LangKey).ToArray())
                        .ToList<UpdateLanguageModel>();

                    var addList = list.Where(m => !oldList.Any(o => m.LangKey.Equals(o.LangKey) && m.DataType == o.DataType))
                        .Select(m => m.To<AddLanguageModel>()).ToList();

                    if (addList.Count > 0)
                    {
                        db.MultiLanguage.Insert(addList);
                    }

                    var updateList = new List<UpdateLanguageModel>();

                    oldList.ForEach(m =>
                    {
                        var item = list.SingleOrDefault(n => n.LangKey.Equals(m.LangKey) && n.DataType == m.DataType);
                        if (item != null)
                        {
                            m.NameChs = item.NameChs;
                            m.NameEng = item.NameEng;
                            m.NameOther = item.NameOther;
                            m.NameTH = item.NameTH;
                            m.NameHK = item.NameHK;//update by liujc
                            m.UpdateBy = createBy;
                            m.UpdateDT = DateTime.Now;
                            updateList.Add(m);
                        }
                    });

                    if (updateList.Count > 0)
                    {
                        db.MultiLanguage.Update(updateList);

                    }

                    return result;
                });
            }
        }

        public ResultModel Search(SearchLanguageModel model)
        {
            return new ResultModel
            {
                Data = this._database.RunQuery(db =>
                {
                    var query = db.MultiLanguage
                        .Query()
                        .Where(
                            db.MultiLanguage.LangKey.like("%" + model.KeyWord + "%")
                            || db.MultiLanguage.NameTH.like("%" + model.KeyWord + "%")
                            || db.MultiLanguage.NameEng.like("%" + model.KeyWord + "%")
                            || db.MultiLanguage.NameChs.like("%" + model.KeyWord + "%")
                            || db.MultiLanguage.NameHK.like("%" + model.KeyWord + "%")//add by liujc
                        );
                    if (model.LanguageId.HasValue)
                    {
                        query = query.Where(db.MultiLanguage.ID == model.LanguageId.Value);
                    }
                    if (model.DataType.HasValue)
                    {
                        query = query.Where(db.MultiLanguage.DataType == model.DataType.Value);
                    }

                    query = query.OrderByDescending(db.MultiLanguage.CreateDT);
                    return new SimpleDataPagedList<LanguageModel>(query, model.PagedIndex, model.PagedSize);
                })
            };
        }

        private int GetColumnIndex(string[] properties, string columnName)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (columnName == null)
                throw new ArgumentNullException("columnName");

            for (var i = 0; i < properties.Length; i++)
                if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1; //excel indexes start from 1
            return 0;
        }
    }
}