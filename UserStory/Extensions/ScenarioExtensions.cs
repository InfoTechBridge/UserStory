using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserStory.Extensions
{
    public static class ScenarioExtensions
    {
        public static Scenario<T> Scenario<T>(this T testObject, string title = null, string id = null, bool continueOnFailed = false) where T : class
        {
            return new Scenario<T>(testObject, title, id, continueOnFailed);
        }

        public static Scenario<T> Given<T>(this T testObject, string title, Action<T> action = null) where T : class
        {
            return new Scenario<T>(testObject).Given(title, action);
        }

        public static Scenario<T> Given<T>(this T testObject, Expression<Action<T>> action = null) where T : class
        {
            return new Scenario<T>(testObject).Given(action);
        }
    }
}
