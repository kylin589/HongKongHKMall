using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using NLog;

namespace HKTHMall.Core
{
    public class NLogHelper
    {
        // NLog.LogManager
        public static Logger GetCurrentClassLogger()
        {
            return LogManager.GetLogger(GetClassFullName());
        }
        /// <summary>
        /// Gets the fully qualified name of the class invoking the LogManager, including the 
        /// namespace but not the assembly.    
        /// </summary>
        private static string GetClassFullName()
        {
            string className;
            Type declaringType;
            int framesToSkip = 2;

            do
            {

                StackFrame frame = new StackFrame(framesToSkip, true);
                MethodBase method = frame.GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    className = method.Name;
                    break;
                }
                framesToSkip++;
                className = declaringType.FullName;
            } while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return className;
        }

    }
}