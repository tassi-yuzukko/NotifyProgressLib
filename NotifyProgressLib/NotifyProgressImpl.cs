using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotifyProgressLib
{
    public class NotifyProgressImpl<T> : NotifyProgressImplCore<T> where T : struct
    {
        readonly INotifyProgressCore<T> notifyProgressCore;

        public NotifyProgressImpl(INotifyProgressCore<T> notifyProgressCore)
        {
            this.notifyProgressCore = notifyProgressCore;
        }

        protected override IObservable<T> Observable => notifyProgressCore.Observable;

        protected override T CompleteState => notifyProgressCore.CompleteState;

        protected override Task StartAsyncCore() => notifyProgressCore.StartAsyncCore();
    }
}
