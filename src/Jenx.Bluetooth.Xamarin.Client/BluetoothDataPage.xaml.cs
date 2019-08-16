using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jenx.Bluetooth.Xamarin.Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothDataPage : ContentPage
    {
        private readonly IDevice _connectedDevice;

        public BluetoothDataPage(IDevice connectedDevice)
        {
            InitializeComponent();
            _connectedDevice = connectedDevice;
        }

        private async void GetManufacturerDataButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var service = await _connectedDevice.GetServiceAsync(GattCharacteristicIdentifiers.ServiceId);
                if (service != null)
                {
                    var characteristic = await service.GetCharacteristicAsync(GattCharacteristicIdentifiers.ManufacturerName);
                    if (characteristic != null)
                    {
                        var bytes = await characteristic.ReadAsync();
                        var str = Encoding.UTF8.GetString(bytes);
                        ManufacturerLabel.Text = str;
                    }
                }
            }
            catch
            {
            }
        }

        private async void SendMessageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var service = await _connectedDevice.GetServiceAsync(GattCharacteristicIdentifiers.ServiceId);
                if (service != null)
                {
                    var characteristic = await service.GetCharacteristicAsync(GattCharacteristicIdentifiers.DataExchange);
                    if (characteristic != null)
                    {
                        byte[] senddata = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(SendMessageLabel.Text) ? "jenx.si was here" : SendMessageLabel.Text);
                        var bytes = await characteristic.WriteAsync(senddata);
                    }
                }
            }
            catch
            {
            }
        }
    }
}