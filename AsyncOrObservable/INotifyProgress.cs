using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncOrObservable
{
    //interface INotifyProgress1 : IObservable<double>, IDisposable
    //{
    //    void Start();
    //}

    interface INotifyProgress2 : IObservable<double>, IDisposable
    {
        Task StartAsync();
    }

    interface INotifyProgress3
    {
        IObservable<double> Start();
    }

    interface INotifyProgress<T> where T : struct
    {
        IObservable<T> Start();
    }
}
