using System;

namespace Jenx.Bluetooth.GattServer.Common
{
    public class CharacteristicEventArgs : EventArgs
    {
        public CharacteristicEventArgs(Guid characteristicId, object value)
        {
            Characteristic = characteristicId;
            Value = value;
        }

        public Guid Characteristic { get; set; }

        public object Value { get; set; }
    }
}