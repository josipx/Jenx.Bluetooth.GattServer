using Jenx.Bluetooth.GattServer.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Jenx.Bluetooth.GattServer.Desktop
{
    public sealed partial class MainPage : Page
    {
        private ILogger _logger;
        private IGattServer _gattServer;

        public MainPage()
        {
            InitializeComponent();
            InitializeLogger();
            InitializeGattServer();
        }

        private void InitializeLogger()
        {
            _logger = new ControlLogger(LogTextBox);
        }

        private void InitializeGattServer()
        {
            _gattServer = new Common.GattServer(GattCharacteristicIdentifiers.ServiceId, _logger);
            _gattServer.OnChararteristicWrite += _gattServer_OnChararteristicWrite; ;
        }

        private async void _gattServer_OnChararteristicWrite(object myObject, CharacteristicEventArgs myArgs)
        {
            await _logger.LogMessageAsync($"Characteristic with Guid: {myArgs.Characteristic.ToString()} changed: {myArgs.Value.ToString()}");
        }

        private async void StartGattServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _gattServer.Initialize();
            }
            catch
            {
                return;
            }

            await _gattServer.AddReadWriteCharacteristicAsync(GattCharacteristicIdentifiers.DataExchange, "Data exchange");
            await _gattServer.AddReadCharacteristicAsync(GattCharacteristicIdentifiers.FirmwareVersion, "1.0.0.1", "Firmware Version");
            await _gattServer.AddWriteCharacteristicAsync(GattCharacteristicIdentifiers.InitData, "Init info");
            await _gattServer.AddReadCharacteristicAsync(GattCharacteristicIdentifiers.ManufacturerName, "Jenx.si", "Manufacturer");

            _gattServer.Start();
        }

        private void StopGattServer_Click(object sender, RoutedEventArgs e)
        {
            _gattServer.Stop();
        }
    }
}