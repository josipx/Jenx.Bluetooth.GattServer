using System;

namespace Jenx.Bluetooth.Xamarin.Client
{
    public class GattCharacteristicIdentifiers
    {
        public static Guid ServiceId = Guid.Parse("19536E67-3682-4588-9F3A-5340B6712150");

        // read only characteristic
        public static Guid FirmwareVersion = Guid.Parse("BC1926EA-6FFA-4D04-928B-76CCCD068CEA");

        // read-write characteristic -> exchange data
        public static Guid DataExchange = Guid.Parse("72563044-DB33-4692-A45D-C5212EEBABFA");

        // write characteristic -> exchange data
        public static Guid InitData = Guid.Parse("168952DE-C604-4401-881E-989996888363");

        // use predefined Bluetooth SIG definitions for well know characteristics.
        public static Guid ManufacturerName = Guid.Parse("00002a29-0000-1000-8000-00805f9b34fb");
    }
}