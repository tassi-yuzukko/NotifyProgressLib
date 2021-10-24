using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace NotifyProgressLib
{
    public abstract class NotifyProgressImplCore<T> : INotifyProgress<T> where T : struct
    {
        public IObservable<T> Start()
        {
            var subject = new Subject<T>();

            var disposable = Observable.Subscribe(x => subject.OnNext(x));

            /*
             ** 注意事項①：ToObservable を使わないといけない **
             * →なぜか ToObservable でないと、例外発生時にハングアップする（StartAsyncCore().ContinueWith(～）ではダメ）
             * 　ContinueWith で処理すると、たぶん異なるスレッドから例外が発生されるからっぽいけど、
             * 　なぜ ToObservable だと許されるのか謎。公式のソース見てもよくわからん。
             * 　だからとりあえず ToObservable を使っている。
             * 　そのアオリを受けて、DoしたりMergeしたりと小細工している
             */
            var observable = StartAsyncCore().ToObservable()
                .Select(_ => CompleteState)
                .Do(_ => { }, _ => { }, () => { subject.OnCompleted(); })
                .Merge(subject)
                .Distinct();

            /*
             ** 注意事項②：subject の Dispose は必ず↑の observable が Dispose された後ではないといけない **
             * →そうしないと、suject.Dispose のときに、「既にDispose地味の変数にアクセスできない例外」が発生する時がある
             * 　何故なのかは不明。だけどそれを回避するために、InnerObservable というクラスを作って小細工している
             */
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
}
