using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfuserExTestCase
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new TestClass();
            t.Changed += HandleChanged;
            t.RaiseChanged(1);
        }

        private static void HandleChanged(object sender, TestClassEventArgs e)
        {
            Console.WriteLine("Change raised: " + e.Value);
        }
    }

    class TestClass
    {
        public event EventHandler<TestClassEventArgs> Changed;

        private readonly TestClassEventArgs _sharedEventArgs = new TestClassEventArgs();

        public void RaiseChanged(int value)
        {
            _sharedEventArgs.Set(value);
            Changed?.Invoke(this, _sharedEventArgs);
        }
    }

    class TestClassEventArgs : EventArgs
    {
        public int Value { get; private set; }

        public void Set(int value)
        {
            Value = value;
        }
    }
}
