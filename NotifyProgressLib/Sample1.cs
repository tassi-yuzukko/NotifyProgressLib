using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace NotifyProgressLib
{
    /// <summary>
    /// 継承パターンで実装する場合のサンプル。
    /// 継承のため、親クラスとの依存関係が密になるので非推奨の方法
    /// </summary>
    public class Sample1 : NotifyProgressImplCore<double>
    {
        readonly bool causeException;

        public Sample1(bool causeException = false)
        {
            this.causeException = causeException;
        }

        readonly Subject<double> subject = new Subject<double>();

        protected override IObservable<double> Observable => subject;

        protected override double CompleteState => 1.0;

        protected override async Task StartAsyncCore()
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
