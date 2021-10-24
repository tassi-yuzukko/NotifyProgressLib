using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace AsyncOrObservable
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var core = new NotifyProgressCore();

            while (true)
            {
                //await Use3(core);
                await UseBeta();
            }
        }

        static async Task UseBeta()
        {
            var x = new NotifyProgressBeta();

            try
            {
                var observable = x.Start();

                using (observable.Subscribe(x => Console.WriteLine($"進捗:{x * 100}%")))
                {
                    Console.WriteLine("開始");
                    await observable;
                    Console.WriteLine("完了");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外発生 [{ex}]");
            }
        }

        static async Task UseGamma()
        {
            var x = new NotifyProgressImpl<double>(new NotifyProgressGamma());

            try
            {
                var observable = x.Start();

                using (observable.Subscribe(x => Console.WriteLine($"進捗:{x * 100}%")))
                {
                    Console.WriteLine("開始");
                    await observable;
                    Console.WriteLine("完了");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外発生 [{ex}]");
            }
        }

        //static async Task Use1(NotifyProgressCore core)
        //{
        //    var x = new NotifyProgress1(core);

        //    try
        //    {
        //        using (x.Subscribe(x => Console.WriteLine($"進捗:{x * 100}%")))
        //        {
        //            Console.WriteLine("開始");

        //            x.Start();

        //            await x;

        //            Console.WriteLine("完了");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"例外発生 [{ex}]");
        //    }
        //}

        static async Task Use2(NotifyProgressCore core)
        {
            using (var x = new NotifyProgress2(core))
            using (x.Subscribe(x => Console.WriteLine($"進捗:{x * 100}%")))
            {
                try
                {
                    Console.WriteLine("開始");
                    await x.StartAsync();
                    Console.WriteLine("完了");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"例外発生 [{ex}]");
                }
            }
        }

        static async Task Use3(NotifyProgressCore core)
        {
            var x = new NotifyProgress3(core);

            try
            {
                var observable = x.Start();

                using (observable.Subscribe(x => Console.WriteLine($"進捗:{x * 100}%")))
                {
                    Console.WriteLine("開始");
                    await observable;
                    Console.WriteLine("完了");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外発生 [{ex}]");
            }
        }

        static async Task Use4(NotifyProgressCore core)
        {
            var x = new NotifyProgress4(core);

            try
            {
                var observable = x.Start();

                Console.WriteLine("開始");

                await observable;

                Console.WriteLine("完了");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外発生 [{ex}]");
            }
        }
    }
}
