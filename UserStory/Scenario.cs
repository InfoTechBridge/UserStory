using Humanizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UserStory
{
    public class Scenario<T> where T : class
    {
        /// <summary>
        /// Given the user has entered valid login credentials
        /// When a user clicks on the login button
        /// Then display the successful validation message
        /// </summary>
        public string Id { get; private set; }
        public string Title { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Result Result { get; private set; }
        public T TestObject { get; internal set; }

        private readonly List<Step<T>> _steps;
        private bool _continueOnFailed = false;
        private bool includeInputsInStepTitle = true;

        public Scenario(T testObject, string title = null, string id = null, bool continueOnFailed = false)
        {
            Title = title ?? GetTitleFromMethodName();
            
            Id = id ?? Guid.NewGuid().ToString();

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

        public void Run()
        {
            Result = Result.Running;
            Duration = TimeSpan.Zero;

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                Console.WriteLine($"Scenario: {Title}");

                foreach (var step in _steps)
                {
                    try
                    {
                        step.Execute(TestObject);
                        Console.WriteLine($"{step.Title}");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"{step.Title} {step.Result.ToString().ToLower()}");
                        
                        if(!_continueOnFailed)
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
                Console.WriteLine($"{Result.ToString().ToUpper()}");
            }
        }

        private string GetTitleFromMethodName()
        {
            var trace = new StackTrace();
            var frames = trace.GetFrames();
            if (frames == null)
                return null;

            var initiatingFrame = frames.LastOrDefault(s => s.GetMethod().DeclaringType == typeof(T));
            if (initiatingFrame == null)
                return null;

            return initiatingFrame.GetMethod().Name.Humanize();
            //return NetToString.Convert(initiatingFrame.GetMethod().Name);
        }

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

        private void AddStep(Expression<Action<T>> action, string title)
        {
            var methodInfo = GetMethodInfo(action);
            //var inputArguments = new object[0];
            //if (includeInputsInStepTitle)
            //{
            //    inputArguments = action.ExtractConstants().ToArray();
            //}

            //var flatInputArray = inputArguments.FlattenArrays();
            var stepTitle = methodInfo.Name.Humanize();

            //if (!string.IsNullOrEmpty(title))
            //    stepTitle = string.Format(title, flatInputArray);

            //else if (includeInputsInStepTitle)
            //{
            //    var stringFlatInputs = flatInputArray.Select(i => i.ToString()).ToArray();
            //    stepTitle = stepTitle + " " + string.Join(", ", stringFlatInputs);
            //}

            stepTitle = stepTitle.Trim();
            var compiledAction = action.Compile();
            _steps.Add(new Step<T>(compiledAction, stepTitle));
        }

        private void AddStep(Expression<Func<T, Task>> stepAction, string stepTextTemplate)
        {
            var methodInfo = GetMethodInfo(stepAction);
            //var inputArguments = new object[0];
            //if (includeInputsInStepTitle)
            //{
            //    inputArguments = stepAction.ExtractConstants().ToArray();
            //}

            //var flatInputArray = inputArguments.FlattenArrays();
            var stepTitle = methodInfo.Name.Humanize();

            //if (!string.IsNullOrEmpty(stepTextTemplate))
            //    stepTitle = string.Format(stepTextTemplate, flatInputArray);

            //else if (includeInputsInStepTitle)
            //{
            //    var stringFlatInputs = flatInputArray.Select(i => i.ToString()).ToArray();
            //    stepTitle = stepTitle + " " + string.Join(", ", stringFlatInputs);
            //}

            stepTitle = stepTitle.Trim();
            var compiledAction = stepAction.Compile();
            //_steps.Add(new TestStep<T>(compiledAction, stepTitle));
        }

        private string GetActionName(Action<T> action)
        {
            var methodInfo = GetMethodInfo(action);
            //var inputArguments = new object[0];
            //if (includeInputsInStepTitle)
            //{
            //    inputArguments = stepAction.ExtractConstants().ToArray();
            //}

            //var flatInputArray = inputArguments.FlattenArrays();
            var stepTitle = methodInfo.Name.Humanize();

            //if (!string.IsNullOrEmpty(stepTextTemplate))
            //    stepTitle = string.Format(stepTextTemplate, flatInputArray);

            //else if (includeInputsInStepTitle)
            //{
            //    var stringFlatInputs = flatInputArray.Select(i => i.ToString()).ToArray();
            //    stepTitle = stepTitle + " " + string.Join(", ", stringFlatInputs);
            //}

            stepTitle = stepTitle.Trim();
            return stepTitle;
        }

        private string GetActionName(Expression<Action<T>> action)
        {
            var methodInfo = GetMethodInfo(action);
            //var inputArguments = new object[0];
            //if (includeInputsInStepTitle)
            //{
            //    inputArguments = stepAction.ExtractConstants().ToArray();
            //}

            //var flatInputArray = inputArguments.FlattenArrays();
            var stepTitle = methodInfo.Name.Humanize();

            //if (!string.IsNullOrEmpty(stepTextTemplate))
            //    stepTitle = string.Format(stepTextTemplate, flatInputArray);

            //else if (includeInputsInStepTitle)
            //{
            //    var stringFlatInputs = flatInputArray.Select(i => i.ToString()).ToArray();
            //    stepTitle = stepTitle + " " + string.Join(", ", stringFlatInputs);
            //}

            stepTitle = stepTitle.Trim();
            return stepTitle;
        }
    }
}
