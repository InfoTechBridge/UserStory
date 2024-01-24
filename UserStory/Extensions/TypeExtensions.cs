using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserStory.Extensions
{
    public static class TypeExtensions
    {
        public static Story<T> Story<T>(this T testObject, string title = null, string id = null, string description = null, string asA = null, string iWantTo = null, string soThatICan = null, bool continueOnFailed = false) where T : class
        {
            return new Story<T>(testObject, title, id, description, asA, iWantTo, soThatICan, continueOnFailed);
        }

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
