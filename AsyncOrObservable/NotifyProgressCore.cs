using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AsyncOrObservable
{
    class NotifyProgressCore : IObservable<double>
    {
        readonly Subject<double> subject = new();

        public async Task StartAsync()
        {
            //throw new InvalidOperationException("hogeeeeeeeeee");
            subject.OnNext(0);

            await Task.Delay(1000);

            subject.OnNext(0.5);

            await Task.Delay(1000);


            subject.OnNext(1);
        }

        public IDisposable Subscribe(IObserver<double> observer)
        {
            return subject.Subscribe(observer);
        }
    }

    class NotifyProgress : NotifyProgressBase<double>
    {
        readonly Subject<double> subject = new();

        protected override IObservable<double> Observable => subject;

        protected override double CompleteState => 1.0;

        protected override async Task StartAsyncCore()
        {
            subject.OnNext(0);

            await Task.Delay(1000);

            subject.OnNext(0.5);

            await Task.Delay(1000);

            subject.OnNext(1);
        }
    }

    class NotifyProgressBeta : NotifyProgressBase2<double>
    {
        readonly Subject<double> subject = new();

        protected override IObservable<double> Observable => subject;

        protected override double CompleteState => 1.0;

        protected override async Task StartAsyncCore()
        {
            subject.OnNext(0);

            await Task.Delay(1000);

            subject.OnNext(0.5);

            await Task.Delay(1000);

            subject.OnNext(1);
        }
    }

    class NotifyProgressGamma : INotifyProgressCore<double>
    {
        readonly Subject<double> subject = new();

        public IObservable<double> Observable => subject;

        public double CompleteState => 1.0;

        public async Task StartAsyncCore()
        {
            subject.OnNext(0);

            await Task.Delay(1000);

            subject.OnNext(0.5);

            await Task.Delay(1000);

            subject.OnNext(1);
        }
    }
}
