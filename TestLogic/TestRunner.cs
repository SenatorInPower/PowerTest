using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace PowerTest
{
    [Flags]
    public enum TestLogInfo
    {
        None = 0,
        ClassName = 1 << 0,
        ExecutionTime = 1 << 1,
        ExceptionDetails = 1 << 2,

    }

    public static class TestRunner
    {
        private static List<MethodInfo> isolatedTestMethods = new List<MethodInfo>();

        private static Dictionary<Type, object> classInstances = new Dictionary<Type, object>();

        private static string LastClassName = string.Empty; // Добавить это поле в класс TestRunner

        private static TestLogInfo logInfo = TestLogInfo.ClassName | TestLogInfo.ExecutionTime | TestLogInfo.ExceptionDetails;

        private static List<MethodInfo> setupMethods = new List<MethodInfo>();
        private static List<MethodInfo> testMethods = new List<MethodInfo>();
        private static List<MethodInfo> tearDownMethods = new List<MethodInfo>();
        public static List<string> TestResults { get; private set; } = new List<string>();

        public static void ClearTestResults()
        {
            TestResults.Clear();
        }
        public static async Task RegisterAndRunTests()
        {
            RegisterMethods();
            await RunSetupMethods();
            await RunTestMethods();
            await RunIsolatedTestMethods(); // Выполняем изолированные тесты
            await RunTearDownMethods();
        }
        private static void RegisterMethods()
        {
            setupMethods.Clear();
            testMethods.Clear();
            tearDownMethods.Clear();
            isolatedTestMethods.Clear();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (Attribute.IsDefined(method, typeof(PowerTestSetupAttribute)))
                    {
                        setupMethods.Add(method);
                    }
                    else if (Attribute.IsDefined(method, typeof(PowerTestAttribute)))
                    {
                        testMethods.Add(method);
                    }
                    else if (Attribute.IsDefined(method, typeof(PowerTestTearDownAttribute)))
                    {
                        tearDownMethods.Add(method);
                    }
                    else if (Attribute.IsDefined(method, typeof(PowerIsolatedTestAttribute)))
                    {
                        isolatedTestMethods.Add(method);
                    }
                }
            }
        }

        private static async Task RunIsolatedTestMethods()
        {
            foreach (var method in isolatedTestMethods)
            {
                var stopwatch = Stopwatch.StartNew();
                object classInstance = Activator.CreateInstance(method.DeclaringType);

                try
                {
                    if (method.ReturnType == typeof(Task) || method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        await (Task)method.Invoke(classInstance, null);
                    }
                    else
                    {
                        method.Invoke(classInstance, null);
                    }
                    stopwatch.Stop();
                    AddResult(method, stopwatch.ElapsedMilliseconds, null);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    AddResult(method, stopwatch.ElapsedMilliseconds, ex);
                }
            }
        }


        private static object GetOrCreateClassInstance(Type classType)
        {
            if (!classInstances.TryGetValue(classType, out var instance))
            {
                instance = Activator.CreateInstance(classType);
                classInstances[classType] = instance;
            }
            return instance;
        }


        private static async Task RunSetupMethods()
        {

            foreach (var method in setupMethods)
            {
                var stopwatch = Stopwatch.StartNew();
                object classInstance = GetOrCreateClassInstance(method.DeclaringType);


                try
                {
                    if (method.ReturnType == typeof(Task))
                    {
                        await (Task)method.Invoke(classInstance, null);
                    }
                    else
                    {
                        method.Invoke(classInstance, null);
                    }

                    AddResult(method, stopwatch.ElapsedMilliseconds, null);
                }
                catch (Exception ex)
                {
                    AddResult(method, stopwatch.ElapsedMilliseconds, ex);
                }
            }
        }

        private static void AddResult(MethodInfo method, long elapsedMilliseconds, Exception ex)
        {
            var result = new System.Text.StringBuilder();
            if (logInfo.HasFlag(TestLogInfo.ClassName))
            {
                result.Append($"{method.DeclaringType?.Name}.{method?.Name}");
            }

            if (logInfo.HasFlag(TestLogInfo.ExecutionTime))
            {
                result.Append($" in {elapsedMilliseconds} ms");
            }

            if (ex != null)
            {
                result.Append($": Failed - {ex.Message}");
                if (logInfo.HasFlag(TestLogInfo.ExceptionDetails) && ex.InnerException != null)
                {
                    result.Append($"\nException Details: {ex.InnerException}");
                }
            }
            else
            {
                result.Append(": Passed");
            }

            string className = method.DeclaringType?.Name ?? "UnknownClass";
            if (!LastClassName.Equals(className))
            {
                TestResults.Add($"----- {className} -----");
                LastClassName = className;
            }

            TestResults.Add(result.ToString());
        }
#if UniTask
        private static async Task RunTestMethods()
        {
            foreach (var method in testMethods)
            {
                var stopwatch = Stopwatch.StartNew();
                object classInstance = GetOrCreateClassInstance(method.DeclaringType);

                try
                {
                    if (method.ReturnType == typeof(Task) ||
                        (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)))
                    {
                        await (Task)method.Invoke(classInstance, null);
                    }
                    else if (method.ReturnType == typeof(Cysharp.Threading.Tasks.UniTask))
                    {
                        var uniTask = (Cysharp.Threading.Tasks.UniTask)method.Invoke(classInstance, null);
                        await uniTask.AsTask();
                    }
                    else if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Cysharp.Threading.Tasks.UniTask<>))
                    {
                        var result = method.Invoke(classInstance, null);
                        var asTaskMethod = result.GetType().GetMethod("AsTask");
                        var task = (Task)asTaskMethod.Invoke(result, null);
                        await task;
                    }
                    else
                    {
                        method.Invoke(classInstance, null);
                    }
                    stopwatch.Stop();
                    AddResult(method, stopwatch.ElapsedMilliseconds, null);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    AddResult(method, stopwatch.ElapsedMilliseconds, ex);
                }
            }
        }
#else
private static async Task RunTestMethods()
{
    foreach (var method in testMethods)
    {
        var stopwatch = Stopwatch.StartNew();
        object classInstance = GetOrCreateClassInstance(method.DeclaringType);

        try
        {
            if (method.ReturnType == typeof(Task) || 
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)))
            {
                await (Task)method.Invoke(classInstance, null);
            }
            else
            {
                method.Invoke(classInstance, null);
            }
            stopwatch.Stop();
            AddResult(method, stopwatch.ElapsedMilliseconds, null);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            AddResult(method, stopwatch.ElapsedMilliseconds, ex);
        }
    }
}
#endif


        private static async Task RunTearDownMethods()
        {
            foreach (var method in tearDownMethods)
            {
                var stopwatch = Stopwatch.StartNew();
                object classInstance = GetOrCreateClassInstance(method.DeclaringType);

                try
                {
                    if (method.ReturnType == typeof(Task) || method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        await (Task)method.Invoke(classInstance, null);
                    }
                    else
                    {
                        method.Invoke(classInstance, null);
                    }
                    stopwatch.Stop();
                    AddResult(method, stopwatch.ElapsedMilliseconds, null);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    AddResult(method, stopwatch.ElapsedMilliseconds, ex);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PowerTestTearDownAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PowerTestSetupAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PowerTestAttribute : Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Method)]
    public class PowerIsolatedTestAttribute : Attribute
    {

    }

}