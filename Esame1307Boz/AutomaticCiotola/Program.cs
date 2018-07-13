using Microsoft.Azure.Devices.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticCiotola
{
    class Program
    {
        static int dosi = 20;
        static DeviceClient deviceClient;
        const string connString = "HostName=CiotolaHub.azure-devices.net;DeviceId=ciotola2;SharedAccessKey=u36zWrjIlshZBCvB484Q7AuBzT6LmLsEq7odG0g5Nyg=";
        const string deviceId = "ciotola2";
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("digita per far mangiare il gatto");
                var input = Console.ReadKey();//alla pressione di un tasto
                simulatedCat();
                await sendMessage();
                Console.ReadLine();
            }
        }

        static void simulatedCat()
        {
            dosi--;
            Console.WriteLine("Il gatto sta mangiando");
        }

        private static async Task sendMessage()
        {
            deviceClient = DeviceClient.CreateFromConnectionString(connString, TransportType.Mqtt);
            try
            {
                var payload = "DoseMangiata " + DateTime.Now.ToLocalTime();
                var msg = new Message(Encoding.UTF8.GetBytes(payload));

                Console.WriteLine("\t{0}> Sending message: {1}", payload);

                await deviceClient.SendEventAsync(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("!!!! " + ex.Message);
            }
        }
    }
}
