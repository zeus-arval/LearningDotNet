using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace LearningTasks
{
    class Program
    {
        static Task _task;
        static object _locker = new object();
        static DateTime now;

        static void Main(string[] args)
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        Task.Delay(2000).ContinueWith(Task =>
                        {
                            throw new Exception("Fuck");
                        });
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message, "1");
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //RunTask();
            //Test();

            //CancellationTokenSource cts = new CancellationTokenSource();
            //now = DateTime.Now;
            //RunTask4(cts.Token);
            //Task.WaitAll();
            //Console.Read();

            //string msg = "{\"sender\": \"Server\",\"content\": \"No errors\"}";
            //var response = JsonConvert.DeserializeObject<Response>(msg);

            //if (response is Response)
            //{
            //    Console.WriteLine(response.ToString());

            //}
            //else
            //{
            //    var request = JsonConvert.DeserializeObject<Request>(msg);
            //    Console.WriteLine(request.Timestamp);
            //}
            Console.Read();
        }

        static void RunTask4(CancellationToken token)
        {
            lock (_locker)
            {
                Console.WriteLine("Running method [{0}]", Passed);
                Task.Delay(TimeSpan.FromSeconds(10), token).ContinueWith(_ =>
                {
                    Console.WriteLine("Running first Task [{0}]", Passed);
                    CancellationTokenSource disconnectCTS = new CancellationTokenSource();
                    Task.Delay(TimeSpan.FromSeconds(5), disconnectCTS.Token).ContinueWith(__ =>
                    {
                        Console.WriteLine("Running second Task [{0}]", Passed);

                        if (disconnectCTS.IsCancellationRequested)
                        {
                            Console.WriteLine($"{nameof(disconnectCTS)} cancellation requested. {Passed}");
                            return;
                        }
                    });

                    Console.WriteLine("Sending ping [{0}]", DateTime.UtcNow.ToLongTimeString());
                    RunTask4(token);
                });
            }
        }
        static string Passed => (DateTime.Now - now).TotalSeconds.ToString();

        static void RunTask()
        {
            Console.WriteLine("Running the task");

            _task = Task.Run(async () =>
            {
                await Run();
            });

            Console.WriteLine(_task.Status.ToString());
            _task.Wait();
            Console.WriteLine("End of RunTask method");
            Console.ReadLine();
        }
        static void RunTask2()
        {
            Console.WriteLine("Running the task");

            _task = new Task(async () =>
            {
                await Run();
            });

            _task.Start();
            Console.WriteLine(_task.Status.ToString());
            _task.Wait();
            Console.WriteLine("End of RunTask method");
            Console.ReadLine();
        }

        static async Task Run()
        {
            Console.WriteLine($"START - Task status:\t[{_task.Status.ToString()}]");
            await Task.Delay(1000);
            Console.WriteLine($"END - Task status:\t[{_task.Status.ToString()}]");
        }
    }

    public class BaseMessage
    {
        [JsonProperty(PropertyName = "sender")]
        public string Sender { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }

    public class Request : BaseMessage
    {
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }
    }

    public class Response : BaseMessage { }
}
