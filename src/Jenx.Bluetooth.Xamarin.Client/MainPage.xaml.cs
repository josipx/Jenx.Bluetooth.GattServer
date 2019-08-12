using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using System.Linq;
using Plugin.Permissions.Abstractions;

namespace Jenx.Bluetooth.Xamarin.Client
{

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly IAdapter _adapter;
        private List<IDevice> _gattDevices= new List<IDevice>();

        public MainPage()
        {
            InitializeComponent();
            
            _adapter = CrossBluetoothLE.Current.Adapter;
            _adapter.DeviceDiscovered += (s, a) =>
            {                
                _gattDevices.Add(a.Device);             
            };
        }

        private async void ScanButton_Clicked(object sender, EventArgs e)
        {
            ScanButton.IsEnabled = false;
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await DisplayAlert("Need location", "App needs location permission", "OK");
                }

                var status1 = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });

                var loca = status1.FirstOrDefault(x=>x.Key == Permission.Location);
                if (loca.Value != null)
                    if (loca.Value == PermissionStatus.Granted) status = PermissionStatus.Granted;
                                
            }

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Need location", "App need location permission", "OK");
                return;
            }

            _gattDevices.Clear();
            await _adapter.StartScanningForDevicesAsync();
            listView.ItemsSource = _gattDevices.ToArray();

            ScanButton.IsEnabled = true;
        }

        async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            IDevice selectedItem = e.SelectedItem as IDevice;

            if (selectedItem.State == Plugin.BLE.Abstractions.DeviceState.Connected)
                await Navigation.PushAsync(new BluetoothDataPage(selectedItem));
            else
            {
                try
                {
                    await _adapter.ConnectToDeviceAsync(selectedItem);
                    await Navigation.PushAsync(new BluetoothDataPage(selectedItem));
                }
                catch (DeviceConnectionException ex)
                {
                    // ... could not connect to device
                }
            }
            
        }
    }
}
