using System.Net.Sockets;
using System.Net;
using System.Text;

namespace PiReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("UDP Server");
            StartListener();
        }

      
        public static void StartListener()
        {
            using (UdpClient socket = new UdpClient())
            {
                socket.Client.Bind(new IPEndPoint(IPAddress.Any, 7000));

                while (true)
                {
                    IPEndPoint from = null;
                    byte[] data = socket.Receive(ref from);
                    string recieved = Encoding.UTF8.GetString(data);
                    Console.WriteLine(recieved);
                    PostToAPI(recieved);
                }
            }
        }

      
        public static async void PostToAPI(string recieved)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpContent content = new StringContent(recieved, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("https://plantwateringapi20221214103134.azurewebsites.net/api/Humidity", content);
                    Console.WriteLine(result.StatusCode);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}