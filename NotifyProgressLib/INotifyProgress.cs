using System;
using System.Collections.Generic;
using System.Text;

namespace NotifyProgressLib
{
    public interface INotifyProgress<T> where T : struct
    {
        IObservable<T> Start();
    }
}
