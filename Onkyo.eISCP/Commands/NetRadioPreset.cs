using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetRadioPreset : ISCPMessage
    {
        public NetRadioPreset() : base ("NPR")
        {}

        public short Preset { get; set; }

        protected override string BuildMessage()
        {
            return $"{Preset:X2}";
        }
    }
    
    public static class NetRadioPresetExtensions
    {
        public static async Task NetRadioSetPresetAsync(this ISCPConnection connection, short preset)
        {
            await connection.SendCommandAsync(new NetRadioPreset() { Preset = preset});
        }
    }
}
