using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotifyProgressLib
{
    public interface INotifyProgressCore<T> where T : struct
    {
        IObservable<T> Observable { get; }

        Task StartAsyncCore();

        T CompleteState { get; }
    }
}
