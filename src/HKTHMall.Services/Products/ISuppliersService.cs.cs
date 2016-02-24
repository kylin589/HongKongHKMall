using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Products
{
    public interface ISuppliersService : IDependency
    {
        /// <summary>
        /// 添加供应商表（wuyf）
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否成功</returns>
        
        ResultModel AddSuppliers(SuppliersModel model);

        /// <summary>
        /// 根据供应商表id获取供应商表（wuyf）
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>供应商表</returns>
        ResultModel GetSuppliersById(long SupplierId);

        /// <summary>
        /// 获取供应商列表（wuyf）
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>供应商列表</returns>
        ResultModel GetSuppliers(SalesSuppliersModel model);

        /// <summary>
        /// 更新供应商表（wuyf）
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateSuppliers(SuppliersModel model);

        /// <summary>
        /// 重置密码（wuyf）
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateSuppliersPassWord(SuppliersModel model);

        /// <summary>
        /// 删除供应商表（wuyf）
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteSuppliers(SuppliersModel model);

        /// <summary>
        /// 根据手机号获取供应商信息
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-25</remarks>
        ResultModel GetSuppliersByPhone(string phone);

        /// <summary>
        /// 更新供应商密码
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否修改成功</returns>
        /// <remarks>added by jimmy,2015-9-25</remarks>
        ResultModel UpdatePwd(SuppliersModel model);
    }
}
