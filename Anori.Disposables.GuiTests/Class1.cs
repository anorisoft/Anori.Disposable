using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakEvent
{
    public class Class1
    {
        public event EventHandler Event1;

        protected virtual void OnEvent1()
        {
            this.Event1?.Invoke(this, EventArgs.Empty);
        }
    }
}