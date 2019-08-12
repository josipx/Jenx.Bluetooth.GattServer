using System;
using System.Text;
using Plugin.BLE.Abstractions.Contracts;
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
                        output.Text = str;
                    }
                }
            }
            catch
            {
            }            
        }

        private async void WriteDataButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var service = await _connectedDevice.GetServiceAsync(GattCharacteristicIdentifiers.ServiceId);
                if (service != null)
                {
                    var characteristic = await service.GetCharacteristicAsync(GattCharacteristicIdentifiers.DataExchange);
                    if (characteristic != null)
                    {
                        byte[] senddata = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(writedata.Text) ? "jenx.si was here" : writedata.Text);
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