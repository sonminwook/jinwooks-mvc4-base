using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AopAlliance.Intercept;

namespace MVC4Base.Interceptor
{
    public class TimeCheckInterceptor : IInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            var dt = DateTime.Now;
            Console.WriteLine("Start : invocation=[{0}]", invocation);
            object rval = invocation.Proceed();
            Console.WriteLine("End : invocation=[{0}], Time : {1}", invocation, (DateTime.Now - dt).TotalMilliseconds.ToString());
            return rval;
        }
    }
}