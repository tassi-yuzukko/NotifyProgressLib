using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace NotifyProgressLib
{
    /// <summary>
    /// Bridge パターンで実装する場合のサンプル。
    /// この方法だと、NotifyProgressImpl と疎結合になるため、推奨の方法
    /// </summary>
    public class Sample2 : INotifyProgressCore<double>
    {
        readonly bool causeException;

        public Sample2(bool causeException = false)
        {
            this.causeException = causeException;
        }

        readonly Subject<double> subject = new Subject<double>();

        public IObservable<double> Observable => subject;

        public double CompleteState => 1.0;

        public async Task StartAsyncCore()
        {
            subject.OnNext(0);

            await Task.Delay(1000);

            subject.OnNext(0.5);

            await Task.Delay(1000);

            if (causeException) throw new InvalidOperationException("例外発生");

            subject.OnNext(1);
        }
    }
}
