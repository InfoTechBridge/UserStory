using System;
using System.Collections.Generic;
using System.Text;

namespace UserStory
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class StoryAttribute : Attribute
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public string AsA { get; set; }
        public string IWantTo { get; set; }
        public string SoThatICan { get; set; }
    }
}
