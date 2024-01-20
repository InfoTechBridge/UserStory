using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStory
{
    internal class Step<T>
    {
        public string Title { get; private set; }

        private Action<T> _action { get; set; }

        //public bool Asserts { get; private set; }
        //public bool ShouldReport { get; private set; }
        
        public Result Result { get; private set; }
        public Exception Exception { get; private set; }
        public TimeSpan Duration { get; private set; }

        public Step(Action<T> action, string title)
        {
            Title = title ?? action?.ToString();
                        
            _action = action;
        }

        public Step(string title)
            : this(null, title)
        {
            
        }

        public void Execute(T testObject)
        {
            Result = Result.Running;
            Duration = TimeSpan.Zero;

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                if (_action != null)
                {
                    _action(testObject);
                }

                Result = Result.Success;
            }
            catch(Exception ex)
            {
                Result = Result.Failed;
                Exception = ex;
                throw;
            }
            finally
            {
                sw.Stop();
                Duration = sw.Elapsed;
            }
        }

        
    }
}
