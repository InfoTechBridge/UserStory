using System;
using System.Collections.Generic;
using System.Text;

namespace UserStory
{
    internal class SequentialIdGenerator
    {
        private int _currentNumber = 0;

        public int NewId()
        {
            return ++_currentNumber;
        }

        public void Reset()
        {
            _currentNumber = 0;
        }
    }
}
