using Humanizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserStory.Extensions;

namespace UserStory
{
    public class Scenario<T> where T : class
    {
        public string Id { get; set; }
        public string Title { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Result Result { get; private set; }
        public T TestObject { get; internal set; }

        private readonly List<Step<T>> _steps;
        private bool _continueOnFailed = false;

        private static readonly SequentialIdGenerator idGenerator = new SequentialIdGenerator();

        public Scenario(T testObject, string title = null, string id = null, bool continueOnFailed = false)
        {
            Title = title ?? GetCallerMethodName();

            Id = id ?? $"#{idGenerator.NewId()}";

            _continueOnFailed = continueOnFailed;

            TestObject = testObject;
            _steps = new List<Step<T>>();
        }

        public Scenario<T> Given(string title, Action<T> action = null)
        {
            AddNewStep("Given", title, action);

            return this;
        }

        public Scenario<T> Given(Expression<Action<T>> action)
        {
            return Given(GetActionName(action), action.Compile());
        }

        public Scenario<T> When(string title, Action<T> action = null)
        {
            AddNewStep("When", title, action);

            return this;
        }

        public Scenario<T> When(Expression<Action<T>> action)
        {
            return When(GetActionName(action), action.Compile());
        }

        public Scenario<T> Then(string title, Action<T> action = null)
        {
            AddNewStep("Then", title, action);

            return this;
        }

        public Scenario<T> Then(Expression<Action<T>> action)
        {
            return Then(GetActionName(action), action.Compile());
        }

        public Scenario<T> And(string title, Action<T> action = null)
        {
            AddNewStep("And", title, action);

            return this;
        }

        public Scenario<T> And(Expression<Action<T>> action)
        {
            return And(GetActionName(action), action.Compile());
        }

        public Result Run()
        {
            Result = Result.Running;
            Duration = TimeSpan.Zero;

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                Console.WriteLine($"Scenario {Id}: {Title}");                

                foreach (var step in _steps)
                {
                    try
                    {
                        step.Execute(TestObject);
                        Console.WriteLine($"\t{step.Title}");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"\t{step.Title} {step.Result.ToString().ToLower()}");

                        if (!_continueOnFailed)
                            throw;
                    }
                    finally
                    {
                        //Console.WriteLine($"{step.Title} {step.Result.ToString().ToLower()}");
                    }
                }

                if (_steps.All(x => x.Result == Result.Success))
                    Result = Result.Success;

                else
                    Result = Result.Failed;
            }
            catch (Exception)
            {
                Result = Result.Failed;
                throw;
            }
            finally
            {
                sw.Stop();
                Duration = sw.Elapsed;
                Console.WriteLine($"\tResult: {Result.ToString().ToUpper()}");
            }

            return Result;
        }

        private string GetCallerMethodName()
        {
            var stackTrace = new StackTrace();

            var frames = stackTrace.GetFrames();
            if (frames != null)
            {
                // find original caller
                var originalFrame = frames.LastOrDefault(s => s.GetMethod().DeclaringType == typeof(T));
                if (originalFrame != null)
                    return originalFrame.GetMethod().Name.Humanize();
            }

            return null;
        }

        ////https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/caller-information
        //private static string GetCallerMethodName([System.Runtime.CompilerServices.CallerMemberName] string caller = default,
        //    [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = default,
        //    [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = default)
        //{
        //    return $"{caller}";
        //}

        private void AddNewStep(string prefix, string title, Action<T> action)
        {
            var step = new Step<T>(action, $"{prefix} {title ?? GetActionName(action)}");
            _steps.Add(step);
        }

        private static MethodInfo GetMethodInfo(Action<T> action)
        {
            return action.Method;
        }

        private static MethodInfo GetMethodInfo(Expression<Action<T>> action)
        {
            var methodCall = (MethodCallExpression)action.Body;
            return methodCall.Method;
        }

        private static MethodInfo GetMethodInfo(Expression<Func<T, Task>> action)
        {
            var methodCall = (MethodCallExpression)action.Body;
            return methodCall.Method;
        }
                
        private string GetActionName(Action<T> action)
        {
            var methodInfo = GetMethodInfo(action);

            return methodInfo.Name.Humanize(LetterCasing.LowerCase);
        }

        private string GetActionName(Expression<Action<T>> action)
        {
            var methodInfo = GetMethodInfo(action);

            return methodInfo.Name.Humanize(LetterCasing.LowerCase);
        }        
    }
}
