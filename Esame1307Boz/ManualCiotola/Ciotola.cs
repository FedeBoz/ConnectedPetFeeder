using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MaunualConnectedCiotola
{
    class Ciotola
    {
        private DeviceClient _client;

        public Ciotola(string deviceId, DeviceClient client)
        {
            Task.Run(async () =>
            {

                while (true)
                {
                    var message = await client.ReceiveAsync();
                    if (message == null)
                    {
                        continue;
                    }
                    var bytes = message.GetBytes();
                    if (bytes == null)
                    {
                        continue;
                    }
                    var text = Encoding.UTF8.GetString(bytes);

                    Console.WriteLine($"Messaggio ricevuto: {text}");
                    var textParts = text.Split();
                    if (textParts[0].ToLower() == "image")
                    {
                        
                        SendToBlobAsync();
                    }
                    await client.CompleteAsync(message);
                }
            });
            _client = client;
        }

        private async void SendToBlobAsync()
        {
            string fileName = "image.jpg";
            Console.WriteLine("Uploading file: {0}", fileName);
            var watch = System.Diagnostics.Stopwatch.StartNew();

            using (var sourceData = new FileStream(@"image.jpg", FileMode.Open))
            {
                await _client.UploadToBlobAsync(fileName, sourceData);
            }

        }
    }
}