using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace AsyncOrObservable
{
    //class NotifyProgress1 : INotifyProgress1
    //{
    //    readonly NotifyProgressCore core;
    //    readonly Subject<double> subject = new Subject<double>();

    //    public NotifyProgress1(NotifyProgressCore core)
    //    {
    //        this.core = core;
    //    }

    //    public void Start()
    //    {
    //        var disposable = core.Subscribe(x => subject.OnNext(x));

    //        core.StartAsync().ToObservable()
    //            .Subscribe(
    //                _ => { },
    //                ex =>
    //                {
    //                    subject.OnError(ex);
    //                    disposable?.Dispose();
    //                },
    //                () =>
    //                {
    //                    subject.OnCompleted();
    //                    disposable?.Dispose();
    //                });
    //    }

    //    public IDisposable Subscribe(IObserver<double> observer)
    //    {
    //        return subject.Subscribe(observer);
    //    }

    //    public void Dispose()
    //    {
    //        subject.Dispose();
    //    }
    //}

    class NotifyProgress2 : INotifyProgress2
    {
        readonly NotifyProgressCore core;
        readonly Subject<double> subject = new();

        public NotifyProgress2(NotifyProgressCore core)
        {
            this.core = core;
        }

        public async Task StartAsync()
        {
            using (core.Subscribe(x => subject.OnNext(x)))
            {
                await core.StartAsync();
            }
        }

        public IDisposable Subscribe(IObserver<double> observer)
        {
            return subject.Subscribe(observer);
        }

        public void Dispose()
        {
            subject.Dispose();
        }
    }

    class NotifyProgress3 : INotifyProgress3
    {
        readonly NotifyProgressCore core;

        public NotifyProgress3(NotifyProgressCore core)
        {
            this.core = core;
        }

        public IObservable<double> Start()
        {
            var subject = new Subject<double>();

            var disposable = core.Subscribe(x => subject.OnNext(x));

            return core.StartAsync().ToObservable()
                .Do(_ => { }, _ => Dispose(), () => { subject.OnCompleted(); Dispose(); })
                .Select(x => 1.0)
                .Merge(subject);

            void Dispose()
            {
                disposable?.Dispose();
                //subject?.Dispose();
            }
        }
    }

    abstract class NotifyProgressBase<T> : INotifyProgress<T> where T : struct
    {
        public IObservable<T> Start()
        {
            var subject = new Subject<T>();

            var disposable = Observable.Subscribe(x => subject.OnNext(x));

            return StartAsyncCore().ToObservable()
                .Select(_ => CompleteState)
                .Do(_ => { }, _ => Dispose(), () => { subject.OnCompleted(); Dispose(); })
                .Merge(subject);

            void Dispose()
            {
                disposable?.Dispose();
                //subject?.Dispose();
            }
        }

        protected abstract IObservable<T> Observable { get; }

        protected abstract Task StartAsyncCore();

        protected abstract T CompleteState { get; }
    }

    abstract class NotifyProgressBase2<T> : INotifyProgress<T> where T : struct
    {
        public IObservable<T> Start()
        {
            var subject = new Subject<T>();

            var disposable = Observable.Subscribe(x => subject.OnNext(x));

            var observable = StartAsyncCore().ToObservable()
                .Select(_ => CompleteState)
                .Do(_ => { }, _ => { }, () => { subject.OnCompleted(); })
                .Merge(subject)
                .Distinct();

            return new InnerObservable(observable, () =>
            {
                disposable?.Dispose();
                subject?.Dispose();
            });
        }

        protected abstract IObservable<T> Observable { get; }

        protected abstract Task StartAsyncCore();

        protected abstract T CompleteState { get; }

        class InnerObservable : IObservable<T>
        {
            readonly IObservable<T> observable;
            readonly Action onDispose;

            public InnerObservable(IObservable<T> observable, Action onDispose)
            {
                this.observable = observable;
                this.onDispose = onDispose;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var disposable = observable.Subscribe(observer);

                var unsubsriber = new Unsubscriber(() =>
                {
                    disposable?.Dispose();
                    onDispose();
                });

                return unsubsriber;
            }
        }

        class Unsubscriber : IDisposable
        {
            readonly Action dispose;

            public Unsubscriber(Action dispose)
            {
                this.dispose = dispose;
            }

            public void Dispose()
            {
                dispose();
            }
        }
    }

    class NotifyProgressImpl<T> : NotifyProgressBase2<T> where T : struct
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

    interface INotifyProgressCore<T> where T : struct
    {
        IObservable<T> Observable { get; }

        Task StartAsyncCore();

        T CompleteState { get; }
    }

    class NotifyProgress4
    {
        readonly NotifyProgressCore core;

        public NotifyProgress4(NotifyProgressCore core)
        {
            this.core = core;
        }

        public IObservable<System.Reactive.Unit> Start()
        {
            return core.StartAsync().ToObservable();
        }
    }
}
