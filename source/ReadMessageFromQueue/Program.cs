using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadMessageFromQueue
{
    class Program
    {
        static QueueClient _queueClient;
        static void Main(string[] args)
        {
            string connectString = "your connect string";
            string queueName = "queuename01";

            try
            {
                _queueClient = new QueueClient(connectString, queueName);

                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                _queueClient.RegisterMessageHandler(ReceiveMessageAsync, messageHandlerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
                _queueClient.CloseAsync();
            }

            Console.WriteLine("Hello World!");
        }

        static async Task<bool> ReceiveMessageAsync(Message message, CancellationToken token)
        {
            try
            {
                Console.WriteLine($"Receive message: {Encoding.UTF8.GetString(message.Body)}");
                //When you uncomment below code to generate exception
                //int i = 1;
                //var error = i / 0;
                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

                return true;
            }
            catch (Exception ex)
            {
                //message will be added dead letter queue
                await _queueClient.AbandonAsync(message.SystemProperties.LockToken);
                return false;
            }
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs.Exception);
            return Task.CompletedTask;
        }
    }
}
