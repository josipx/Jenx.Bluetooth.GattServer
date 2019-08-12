using Jenx.Bluetooth.GattServer.Common;
using System.Threading.Tasks;

namespace Jenx.Bluetooth.GattServer.Console
{
    internal class Program
    {
        static private ILogger _logger;
        static private IGattServer _gattServer;

        private static async Task Main(string[] args)
        {
            InitializeLogger();
            InitializeGattServer();
            await StartGattServer();
            await StartLooping();
        }

        #region Private

        private static void InitializeLogger()
        {
            _logger = new ConsoleLogger();
        }

        private static void InitializeGattServer()
        {
            _gattServer = new Common.GattServer(GattCharacteristicIdentifiers.ServiceId, _logger);
            _gattServer.OnChararteristicWrite += GattServerOnChararteristicWrite;
        }

        private static async Task StartGattServer()
        {
            try
            {
                await _logger.LogMessageAsync("Starting Initializong Jenx.si Bluetooth Gatt service.");
                await _gattServer.Initialize();
                await _logger.LogMessageAsync("Jenx.si Bluetooth Gatt service initialized.");
            }
            catch
            {
                await _logger.LogMessageAsync("Error starting Jenx.si Bluetooth Gatt service.");
                throw;
            }

            await _gattServer.AddReadWriteCharacteristicAsync(GattCharacteristicIdentifiers.DataExchange, "Data exchange");
            await _gattServer.AddReadCharacteristicAsync(GattCharacteristicIdentifiers.FirmwareVersion, "1.0.0.1", "Firmware Version");
            await _gattServer.AddWriteCharacteristicAsync(GattCharacteristicIdentifiers.InitData, "Init info");
            await _gattServer.AddReadCharacteristicAsync(GattCharacteristicIdentifiers.ManufacturerName, "Jenx.si", "Manufacturer");

            _gattServer.Start();
            await _logger.LogMessageAsync("Jenx.si Bluetooth Gatt service started.");
        }

        private static async Task StartLooping()
        {
            System.ConsoleKeyInfo cki;
            System.Console.CancelKeyPress += new System.ConsoleCancelEventHandler(KeyPressHandler);

            while (true)
            {
                await _logger.LogMessageAsync("Press any key, or 'X' to quit, or ");
                await _logger.LogMessageAsync("CTRL+C to interrupt the read operation:");
                cki = System.Console.ReadKey(true);
                await _logger.LogMessageAsync($"  Key pressed: {cki.Key}\n");

                // Exit if the user pressed the 'X' key.
                if (cki.Key == System.ConsoleKey.X) break;
            }
        }

        private static async void KeyPressHandler(object sender, System.ConsoleCancelEventArgs args)
        {
            await _logger.LogMessageAsync("\nThe read operation has been interrupted.");
            await _logger.LogMessageAsync($"  Key pressed: {args.SpecialKey}");
            await _logger.LogMessageAsync($"  Cancel property: {args.Cancel}");
            await _logger.LogMessageAsync("Setting the Cancel property to true...");
            args.Cancel = true;

            await _logger.LogMessageAsync($"  Cancel property: {args.Cancel}");
            await _logger.LogMessageAsync("The read operation will resume...\n");
        }

        private static async void GattServerOnChararteristicWrite(object myObject, CharacteristicEventArgs myArgs)
        {
            await _logger.LogMessageAsync($"Characteristic with Guid: {myArgs.Characteristic.ToString()} changed: {myArgs.Value.ToString()}");
        }

        private static void StopGattServer()
        {
            _gattServer.Stop();
        }

        #endregion Private
    }
}