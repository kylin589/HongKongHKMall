using System;
using System.Linq;
using Autofac;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using Castle.DynamicProxy;
using FluentValidation;
using FluentValidation.Attributes;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services
{
    /// <summary>
    ///     Service Aop 横切注入
    /// </summary>
    public class ServiceIInterceptor : IInterceptor
    {
        private readonly ILogger Logger = BrEngineContext.Current.Resolve<ILogger>();

        public void Intercept(IInvocation invocation)
        {
            var result = new ResultModel();
            try
            {
                if (invocation.Arguments != null)
                {
                    foreach (var argument in invocation.Arguments)
                    {
                        if (argument != null)
                        {
                            var validatorAttributes =
                                argument.GetType().GetCustomAttributes(typeof (ValidatorAttribute), true)
                                    .Select(a => (ValidatorAttribute) a).ToList();

                            foreach (var validatorAttribute in validatorAttributes)
                            {
                                var validator = (IValidator) Activator.CreateInstance(validatorAttribute.ValidatorType);
                                var validateResult = validator.Validate(argument);
                                if (validateResult.IsValid)
                                {
                                    continue;
                                }

                                foreach (var validationFailure in validateResult.Errors)
                                {
                                    result.Messages.Add(validationFailure.ErrorMessage);
                                }
                            }
                        }
                    }
                }

                if (result.Messages.Any())
                {
                    result.IsValid = false;
                    invocation.ReturnValue = result;
                }
                else
                {
                    invocation.Proceed();
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(invocation.TargetType,
                    string.Format("类名:{0}\t 方法:{1}\t 错误信息:{2}", invocation.TargetType.FullName, invocation.Method.Name,
                        ex.Message));
                result.Messages.Add(ex.Message);
                result.IsValid = false;
                invocation.ReturnValue = result;
                //throw;
            }
        }
    }
}