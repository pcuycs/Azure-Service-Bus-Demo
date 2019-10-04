using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PushMessageToQueue
{
    class Program
    {
        static QueueClient queueClient;

        static void Main(string[] args)
        {
            try
            {
                Send();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Console.ReadKey();
            }
        }

        async static void Send()
        {
            try
            {
                var result = await SendAsync();
            }
            catch (Exception ex)
            {
                await queueClient.CloseAsync();
                throw;
            }
            finally
            {
                await queueClient.CloseAsync();
            }
        }

        static async Task<string> SendAsync()
        {
            string connectString = "your connect string";
            string queueName = "queuename01";
            queueClient = new QueueClient(connectString, queueName);
            var message = new Message(Encoding.UTF8.GetBytes("message01"));
            await queueClient.SendAsync(message);
            var aa = string.Empty;
            
            return "ok";
        }
    }
}
