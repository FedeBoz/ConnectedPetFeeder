using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ManualCiotola.ViewModels;
using ManualCiotola.Views;

namespace ManualCiotola
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var deviceId = ConfigurationManager.AppSettings["deviceId"];
            var authenticationMethod =
                new DeviceAuthenticationWithRegistrySymmetricKey(
                    deviceId,
                    ConfigurationManager.AppSettings["deviceKey"]
                )
            ;

            var transportType = TransportType.Mqtt;
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["transportType"]))
            {
                transportType = (TransportType)
                    Enum.Parse(typeof(TransportType),
                    ConfigurationManager.AppSettings["transportType"], true);

            }

            var client = DeviceClient.Create(
                ConfigurationManager.AppSettings["hostName"],
                authenticationMethod,
                transportType
            );
            var view = new CiotolaView();
            var viewModel = new CiotolaViewModel(deviceId, client);
            view.DataContext = viewModel;

            var app = new Application();
            app.Run(view);
        }
    }
}
