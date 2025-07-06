using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

internal class Program
{
    static void Main(string[] args)
    {
        AInfo aInfo;
        int count_1 = 10000;
        int count_2 = 10000;
        Stopwatch stopwatch = new Stopwatch();
        double[] normalElapsed = new double[count_1];
        double[] cacheElapsed = new double[count_1];
        double[] expressionElapsed = new double[count_1];
        double[] expressionElapsed_1 = new double[count_1];
        double[] emitElapsed = new double[count_1];
        double[] emitElapsed_1 = new double[count_1];

        int[] aparams = new int[count_1];
        double[] bparams = new double[count_1];
        string[] cparams = new string[count_1];

        Random rand = new Random();
        for (int i = 0; i < count_1; i++)
        {
            aparams[i] = rand.Next(0, i);
            bparams[i] = rand.NextDouble();
            cparams[i] = DateTime.Now.ToString();
        }

        Console.WriteLine("***反射设值耗时统计***");
        aInfo = new AInfo();
        for (int i = 0; i < count_1; i++)
        {
            var aparam = aparams[i];
            var bparam = bparams[i];
            var cparam = cparams[i];

            stopwatch.Restart();
            for (int ii = 0; ii < count_2; ii++)
            {
                aInfo.SetParamValue_Normal(nameof(AInfo.AParam), aparam);
                aInfo.SetParamValue_Normal(nameof(AInfo.BParam), bparam);
                aInfo.SetParamValue_Normal(nameof(AInfo.CParam), cparam);
            }
            stopwatch.Stop();
            if (aInfo.AParam != aparam || aInfo.BParam != bparam || aInfo.CParam != cparam)
                throw new Exception("设值错误");
            normalElapsed[i] = stopwatch.Elapsed.TotalMilliseconds;
        }
        Console.WriteLine($"总耗时:{normalElapsed.Sum()}ms");
        Console.WriteLine($"平均耗时:{normalElapsed.Average()}ms");
        Console.WriteLine($"最长耗时:{normalElapsed.Max()}ms,首次耗时:{normalElapsed.First()}ms");
        Console.WriteLine($"最短耗时:{normalElapsed.Min()}ms");

        Console.WriteLine("***缓存反射设值设置耗时统计***");
        aInfo = new AInfo();
        for (int i = 0; i < count_1; i++)
        {
            var aparam = aparams[i];
            var bparam = bparams[i];
            var cparam = cparams[i];

            stopwatch.Restart();
            for (int ii = 0; ii < count_2; ii++)
            {
                aInfo.SetParamValue_Cache(nameof(AInfo.AParam), aparam);
                aInfo.SetParamValue_Cache(nameof(AInfo.BParam), bparam);
                aInfo.SetParamValue_Cache(nameof(AInfo.CParam), cparam);
            }
            stopwatch.Stop();
            if (aInfo.AParam != aparam || aInfo.BParam != bparam || aInfo.CParam != cparam)
                throw new Exception("设值错误");
            cacheElapsed[i] = stopwatch.Elapsed.TotalMilliseconds;
        }
        Console.WriteLine($"总耗时:{cacheElapsed.Sum()}ms");
        Console.WriteLine($"平均耗时:{cacheElapsed.Average()}ms");
        Console.WriteLine($"最长耗时:{cacheElapsed.Max()}ms,首次耗时:{cacheElapsed.First()}ms");
        Console.WriteLine($"最短耗时:{cacheElapsed.Min()}ms");

        Console.WriteLine("***表达式树设值设置耗时统计***");
        aInfo = new AInfo();
        for (int i = 0; i < count_1; i++)
        {
            var aparam = aparams[i];
            var bparam = bparams[i];
            var cparam = cparams[i];

            stopwatch.Restart();
            for (int ii = 0; ii < count_2; ii++)
            {
                aInfo.SetParamValue_Expression(nameof(AInfo.AParam), aparam);
                aInfo.SetParamValue_Expression(nameof(AInfo.BParam), bparam);
                aInfo.SetParamValue_Expression(nameof(AInfo.CParam), cparam);
            }
            stopwatch.Stop();
            if (aInfo.AParam != aparam || aInfo.BParam != bparam || aInfo.CParam != cparam)
                throw new Exception("设值错误");
            expressionElapsed[i] = stopwatch.Elapsed.TotalMilliseconds;
        }
        Console.WriteLine($"总耗时:{expressionElapsed.Sum()}ms");
        Console.WriteLine($"平均耗时:{expressionElapsed.Average()}ms");
        Console.WriteLine($"最长耗时:{expressionElapsed.Max()}ms,首次耗时:{expressionElapsed.First()}ms");
        Console.WriteLine($"最短耗时:{expressionElapsed.Min()} ms");


        Console.WriteLine("***表达式树(去除装箱)设值设置耗时统计***");
        aInfo = new AInfo();
        for (int i = 0; i < count_1; i++)
        {
            var aparam = aparams[i];
            var bparam = bparams[i];
            var cparam = cparams[i];

            stopwatch.Restart();
            for (int ii = 0; ii < count_2; ii++)
            {
                aInfo.SetParamValue_Expression_1(nameof(AInfo.AParam), aparam);
                aInfo.SetParamValue_Expression_1(nameof(AInfo.BParam), bparam);
                aInfo.SetParamValue_Expression_1(nameof(AInfo.CParam), cparam);
            }
            stopwatch.Stop();
            if (aInfo.AParam != aparam || aInfo.BParam != bparam || aInfo.CParam != cparam)
                throw new Exception("设值错误");
            expressionElapsed_1[i] = stopwatch.Elapsed.TotalMilliseconds;
        }
        Console.WriteLine($"总耗时:{expressionElapsed_1.Sum()}ms");
        Console.WriteLine($"平均耗时:{expressionElapsed_1.Average()} ms");
        Console.WriteLine($"最长耗时:{expressionElapsed_1.Max()}ms,首次耗时:{expressionElapsed_1.First()}ms");
        Console.WriteLine($"最短耗时:{expressionElapsed_1.Min()} ms");

        Console.WriteLine("***EMIT设值设置耗时统计***");
        aInfo = new AInfo();
        for (int i = 0; i < count_1; i++)
        {
            var aparam = aparams[i];
            var bparam = bparams[i];
            var cparam = cparams[i];

            stopwatch.Restart();
            for (int ii = 0; ii < count_2; ii++)
            {
                aInfo.SetParamValue_EMIT(nameof(AInfo.AParam), aparam);
                aInfo.SetParamValue_EMIT(nameof(AInfo.BParam), bparam);
                aInfo.SetParamValue_EMIT(nameof(AInfo.CParam), cparam);
            }
            stopwatch.Stop();
            if (aInfo.AParam != aparam || aInfo.BParam != bparam || aInfo.CParam != cparam)
                throw new Exception("设值错误");
            emitElapsed[i] = stopwatch.Elapsed.TotalMilliseconds;
        }
        Console.WriteLine($"总耗时:{emitElapsed.Sum()}ms");
        Console.WriteLine($"平均耗时:{emitElapsed.Average()} ms");
        Console.WriteLine($"最长耗时:{emitElapsed.Max()}ms,首次耗时:{emitElapsed.First()}ms");
        Console.WriteLine($"最短耗时:{emitElapsed.Min()}ms");

        Console.WriteLine("***EMIT绑定设值设置耗时统计***");
        aInfo = new AInfo();
        for (int i = 0; i < count_1; i++)
        {
            var aparam = aparams[i];
            var bparam = bparams[i];
            var cparam = cparams[i];

            stopwatch.Restart();
            for (int ii = 0; ii < count_2; ii++)
            {
                aInfo.SetParamValue_EMIT_1(nameof(AInfo.AParam), aparam);
                aInfo.SetParamValue_EMIT_1(nameof(AInfo.BParam), bparam);
                aInfo.SetParamValue_EMIT_1(nameof(AInfo.CParam), cparam);
            }
            stopwatch.Stop();
            if (aInfo.AParam != aparam || aInfo.BParam != bparam || aInfo.CParam != cparam)
                throw new Exception("设值错误");
            emitElapsed_1[i] = stopwatch.Elapsed.TotalMilliseconds;
        }
        Console.WriteLine($"总耗时:{emitElapsed_1.Sum()}ms");
        Console.WriteLine($"平均耗时:{emitElapsed_1.Average()} ms");
        Console.WriteLine($"最长耗时:{emitElapsed_1.Max()}ms,首次耗时:{emitElapsed_1.First()}ms");
        Console.WriteLine($"最短耗时:{emitElapsed_1.Min()}ms");

        Console.ReadLine();
    }
    class AInfo
    {
        public int AParam { get; set; }
        public double BParam { get; set; }
        public string CParam { get; set; }

        //设置指定属性的值
        public void SetParamValue_Normal<T>(string a, T value)
        {
            var property = this.GetType().GetProperty(a);
            if (property == null)
                return;
            property.SetValue(this, value);
        }

        //使用字典对PropertyInfo进行缓存，防止每次调用时都需要重新获取PropertyInfo
        Dictionary<string, PropertyInfo> cache_properties = new Dictionary<string, PropertyInfo>();
        public void SetParamValue_Cache<T>(string a, T value)
        {
            if (!cache_properties.ContainsKey(a))
            {
                var property = this.GetType().GetProperty(a);
                if (property == null)
                    return;
                cache_properties[a] = (property);
            }
            cache_properties[a].SetValue(this, value);
        }

        //使用字典缓存表达式树生成的动态方法
        Dictionary<string, Action<AInfo, object>> cache_expression_delegates = new Dictionary<string, Action<AInfo, object>>();
        public void SetParamValue_Expression<T>(string a, T value)
        {
            if (!cache_expression_delegates.ContainsKey(a))
            {
                var type_this = this.GetType();
                if (type_this == null)
                    return;
                var type_obj = typeof(object);
                var type_value = typeof(T);
                var param_this = Expression.Parameter(type_this);//定义输入参数，类型AInfo
                var param_this_value = Expression.Property(param_this, a);//访问AInfo的指定属性
                var param_value = Expression.Parameter(type_obj);//定义输入参数，由于目标类型是Action<AInfo, object>，因此value值设定为object
                var param_value_convert = Expression.Convert(param_value, type_value);//Expression.Assign严格限定值类型，因此需要先将object转换为指定属性的同类型
                var assign = Expression.Assign(param_this_value, param_value_convert);//将value值赋予指定属性
                var lambda = Expression.Lambda<Action<AInfo, object>>(assign, param_this, param_value).Compile();//生成动态函数并作为委托返回
                cache_expression_delegates[a] = lambda;
            }
            cache_expression_delegates[a](this, value);
        }

        //使用字典缓存表达式树生成的动态方法，使用委托基类作为Value值缓存,设置时转换委托类型，避免拆装箱
        Dictionary<string, Delegate> cache_expressions_NoBox_delegates = new Dictionary<string, Delegate>();
        public void SetParamValue_Expression_1<T>(string a, T value)
        {
            if (!cache_expressions_NoBox_delegates.ContainsKey(a))
            {
                var type_this = this.GetType();
                if (type_this == null)
                    return;
                var type_value = typeof(T);
                var param_this = Expression.Parameter(type_this);
                var param_this_value = Expression.Property(param_this, a);
                var param_value = Expression.Parameter(type_value);
                var assign = Expression.Assign(param_this_value, param_value);
                var lambda = Expression.Lambda<Action<AInfo, T>>(assign, param_this, param_value).Compile();
                cache_expressions_NoBox_delegates[a] = lambda;
            }
            ((Action<AInfo, T>)cache_expressions_NoBox_delegates[a])(this, value);
        }

        //使用字典缓存EMIT生成的动态方法
        Dictionary<string, Delegate> cache_emit_delegates = new Dictionary<string, Delegate>();
        public void SetParamValue_EMIT<T>(string a, T value)
        {
            if (!cache_emit_delegates.ContainsKey(a))
            {
                var type_this = this.GetType();
                if (type_this == null)
                    return;
                var methodInfo = type_this.GetProperty(a)?.GetSetMethod();
                if (methodInfo == null)
                    return;
                var type_obj = typeof(object);
                var type_value = typeof(T);
                var dynamicMethod = new DynamicMethod($"Set{a}", typeof(void), new Type[] { type_this, type_value }, typeof(AInfo));//.NET Framework中使用DynamicMethod(String, Type, Type[], Module)指定模块会报错
                var il = dynamicMethod.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Call, methodInfo);
                il.Emit(OpCodes.Ret);
                var lambda = dynamicMethod.CreateDelegate<Action<AInfo, T>>();//此泛型方法仅在.NET 5及以上版本提供，更低版本需使用MethodInfo.CreateDelegate(Type)及其重载。
                cache_emit_delegates[a] = lambda;
            }
            ((Action<AInfo, T>)cache_emit_delegates[a])(this, value);
        }

        //使用字典缓存EMIT生成的动态方法
        Dictionary<string, Delegate> cache_emit_bing_delegates = new Dictionary<string, Delegate>();
        public void SetParamValue_EMIT_1<T>(string a, T value)
        {
            if (!cache_emit_bing_delegates.ContainsKey(a))
            {
                var type_this = this.GetType();
                if (type_this == null)
                    return;
                var methodInfo = type_this.GetProperty(a)?.GetSetMethod();
                if (methodInfo == null)
                    return;
                var type_obj = typeof(object);
                var type_value = typeof(T);
                var dynamicMethod = new DynamicMethod($"Set{a}", typeof(void), new Type[] { type_this, type_value }, typeof(AInfo));//.NET Framework中使用DynamicMethod(String, Type, Type[], Module)指定模块会报错
                var il = dynamicMethod.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Call, methodInfo);
                il.Emit(OpCodes.Ret);
                //将创建的动态方法绑定到对象
                var lambda = dynamicMethod.CreateDelegate<Action<T>>(this);//此泛型方法仅在.NET 5及以上版本提供，更低版本需使用MethodInfo.CreateDelegate(Type,Object)及其重载。
                cache_emit_bing_delegates[a] = lambda;
            }
            ((Action<T>)cache_emit_bing_delegates[a])(value);
        }
    }
}