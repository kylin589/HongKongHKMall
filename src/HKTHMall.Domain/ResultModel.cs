using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HKTHMall.Domain.Models
{
    [DataContract(Name = "result")]
    public class ResultModel : IResult
    {
        public ResultModel()
        {
            this.IsValid = true;
            this.Messages = new List<string>();
        }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// flag状态
        /// </summary>
        [DataMember(Name = "flag")]
        public int Flag
        {
            get { return this.IsValid ? 1 : 0; }
        }

        /// <summary>
        /// 错误信息组
        /// </summary>
        public IList<string> Messages { get; set; }

        [DataMember(Name = "msg")]
        public string Message
        {
            get { return string.Join("", Messages); }
        }

        /// <summary>
        /// 返回的数据
        /// </summary>
        [DataMember(Name = "rs")]
        public dynamic Data { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember(Name = "status")]
        public int Status { get; set; }

        /// <summary>
        /// 转换到强类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T To<T>() where T : class
        {
            return this.Data;
        }
    }

    public class IResult { }

    [DataContract(Name = "result")]
    public class TempResultModel
    {

        /// <summary>
        /// flag状态
        /// </summary>
        [DataMember(Name = "flag")]
        public int Flag { get; set; }

        [DataMember(Name = "msg")]
        public string Message { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        [DataMember(Name = "rs")]
        public dynamic Data { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember(Name = "status")]
        public int Status { get; set; }

        /// <summary>
        /// 转换为结果模型
        /// </summary>
        /// <param name="tempResult"></param>
        /// <returns></returns>
        public ResultModel ConvertToResultModel(TempResultModel tempResult)
        {
            ResultModel resultModel = null;

            if (tempResult != null)
            {
                resultModel = new ResultModel()
                {
                    Messages = new List<string>() { tempResult.Message },
                    Status = tempResult.Status,
                    IsValid = tempResult.Flag == 1
                };
            }
            return resultModel;
        }

    }
}