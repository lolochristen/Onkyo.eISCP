using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public enum Inputs
    {
        STB_DVR = 0x0,
        CBL_SAT = 0x1,
        GAME1 = 0x2,
        AUX1 = 0x3,
        GAME2 = 0x4,
        PC = 0x5,
        VIDEO7 = 0x6,
        EXTRA1 = 0x7,
        EXTRA2 = 0x8,
        EXTRA3 = 0x9,
        BD_DVD = 0x10,
        STRM_BOX = 0x11,
        TV = 0x12,
        TV_TAPE = 0x20,
        TAPE2 = 0x21,
        PHONO = 0x22,
        CD = 0x23,
        FM = 0x24,
        AM = 0x25,
        TUNER = 0x26,
        DLNA = 0x27,
        INTERNET_RADIO = 0x28,
        USB = 0x29,
        USB_Rear = 0x2A,
        NET = 0x2B,
        USB_toggle = 0x2C,
        Aiplay = 0x2D,
        Bluetooth = 0x2E,
        Universal_PORT = 0x40,
        MULTI_CH = 0x30,
        XM = 0x31,
        SIRIUS = 0x32,
        DAB = 0x33,
        HDMI_5 = 0x55,
        HDMI_6 = 0x56,
        HDMI_7 = 0x57
    }

    public class Input : ISCPMessage
    {
        private Inputs _selectedInput;

        public Input() : base("SLI")
        {
        }

        public Input(Zone zone) : base(zone == Zone.Zone2 ? "SLZ" : "SLI")
        {
        }

        public Inputs SelectedInput
        {
            get => _selectedInput; set
            {
                if (_selectedInput != value)
                {
                    _selectedInput = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override string BuildMessage()
        {
            if (string.IsNullOrEmpty(RawData))
                return ((int)SelectedInput).ToString("X2");
            return RawData;
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            SelectedInput = (Inputs)int.Parse(RawData, System.Globalization.NumberStyles.HexNumber);
        }
    }

    public static class InputExtensions
    {
        public static async Task<Input> SelectInputAsync(this ISCPConnection connection, Inputs input, Zone zone = Zone.Main)
        {
            return await connection.SendCommandAsync<Input>(new Input(zone) { SelectedInput = input });
        }

        public static async Task<Input> GetInputAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            return await connection.SendCommandAsync<Input>(new Input(zone) { RawData = "QSTN" });
        }
    }

}
