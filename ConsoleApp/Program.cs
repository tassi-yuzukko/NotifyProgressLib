using NotifyProgressLib;
using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                await UseSample2();
            }
        }

        static async Task UseSample1()
        {
            var x = new Sample1();

            await TestCore(x);
        }

        static async Task UseSample2()
        {
            var x = new NotifyProgressImpl<double>(new Sample2());

            await TestCore(x);
        }

        static async Task TestCore(INotifyProgress<double> notifyProgress)
        {
            try
            {
                var observable = notifyProgress.Start();

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
    }
}
