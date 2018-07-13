using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ManualCiotola.ViewModels
{

    public class CiotolaViewModel : INotifyPropertyChanged
    {
        private int _startDoses = 20;

        private int _alarmPoint = 0;

        private DeviceClient _client;

        public bool Alarm => _doses <= _alarmPoint;

        public CiotolaViewModel(string deviceId, DeviceClient client)
        {
            Task.Run(() =>
            {
                Doses = _startDoses;

                while (true)
                {

                    var sample = new
                    {
                        Timestamp = DateTime.Now.ToUniversalTime(),
                        DeviceId = deviceId,
                        SampleType = "doses",
                        Value = Doses
                    };
                    var json = JsonConvert.SerializeObject(sample);
                    var bytes = Encoding.UTF8.GetBytes(json);

                    var message = new Message(bytes);
                    message.Properties["SampleType"] = "doses";
                    client.SendEventAsync(message).Wait();
                    

                    Task.Delay(1000).Wait();
                }
            });

            _client = client;
        }

        private decimal _doses;

        public decimal Doses
        {
            get => _doses;
            set
            {
                _doses = value;
                Notify();
                Notify("Alarm");
            }
        }

        protected void Notify([CallerMemberName]string propertyName = null)
        {
            if (_propertyChanged == null) return;
            _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private PropertyChangedEventHandler _propertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                _propertyChanged += value;
            }

            remove
            {
                _propertyChanged -= value;
            }
        }
    }
}
