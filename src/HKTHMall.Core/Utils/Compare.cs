using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region << 版 本 注 释 >>

/*

 * ========================================================================

 * Copyright(c) 

 * ========================================================================

 *  

 * 【去掉重复List项】

 *  

 *  

 * 作者:Eric   时间:2014-05-19 17:34:35

 * 文件名:Compare

 * 版本:V1.0.0

 * 

 * 修改者:           时间:               

 * 修改说明:

 * ========================================================================

*/

#endregion
namespace HKTHMall.Core
{
    public delegate bool EqualsComparer<T>(T x, T y);
    /// <summary>
    /// 去掉重复 List<T>.Distinct(） 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Compare<T> : IEqualityComparer<T>
    {
        private EqualsComparer<T> _equalsComparer;
        public Compare(EqualsComparer<T> equalsComparer)
        {
            this._equalsComparer = equalsComparer;
        }
        public bool Equals(T x, T y)
        {
            if (null != this._equalsComparer)
            {

                return this._equalsComparer(x, y);
            }
            else
            {
                return false;
            }
        }
        public int GetHashCode(T obj)
        {
            //if (obj == null)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return obj.ToString().GetHashCode();
            //}
            return obj.ToString().GetHashCode();
        }
    }
}
