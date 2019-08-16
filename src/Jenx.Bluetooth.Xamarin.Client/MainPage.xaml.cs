using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace Jenx.Bluetooth.Xamarin.Client
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly IAdapter _bluetoothAdapter;
        private List<IDevice> _gattDevices = new List<IDevice>();

        public MainPage()
        {
            InitializeComponent();

            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            _bluetoothAdapter.DeviceDiscovered += (s, a) =>
            {
                _gattDevices.Add(a.Device);
            };
        }

        private async void ScanButton_Clicked(object sender, EventArgs e)
        {
            ScanButton.IsEnabled = false;

            var locationPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (locationPermissionStatus != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await DisplayAlert("Permission required", "Application needs location permission", "OK");
                }

                var requestLocationPermissionStatus = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });

                var locationPermissionPersent = requestLocationPermissionStatus.FirstOrDefault(x => x.Key == Permission.Location);
                if (locationPermissionPersent.Value != null)
                    if (locationPermissionPersent.Value == PermissionStatus.Granted) locationPermissionStatus = PermissionStatus.Granted;
            }

            if (locationPermissionStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission required", "Application needs location permission", "OK");
                return;
            }

            _gattDevices.Clear();
            await _bluetoothAdapter.StartScanningForDevicesAsync();
            listView.ItemsSource = _gattDevices.ToArray();
            ScanButton.IsEnabled = true;
        }

        private async void FoundBluetoothDevicesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            IDevice selectedItem = e.SelectedItem as IDevice;

            if (selectedItem.State == Plugin.BLE.Abstractions.DeviceState.Connected)
                await Navigation.PushAsync(new BluetoothDataPage(selectedItem));
            else
            {
                try
                {
                    await _bluetoothAdapter.ConnectToDeviceAsync(selectedItem);
                    await Navigation.PushAsync(new BluetoothDataPage(selectedItem));
                }
                catch
                {
                    // ... could not connect to device
                }
            }
        }
    }
}