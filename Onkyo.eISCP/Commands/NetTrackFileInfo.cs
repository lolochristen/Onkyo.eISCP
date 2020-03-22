using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetTrackFileInfo : ISCPMessage
    {
        public NetTrackFileInfo() : base("NFI")
        { }

        public string Format { get; private set; }
        public string Bitrate { get; private set; }

        protected override string BuildMessage()
        {
            return "QSTN";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            var parts = RawData.Split('/');
            if (parts.Length > 0)
                Format = parts[0];
            else
                Format = string.Empty;
            if (parts.Length > 1)
                Bitrate = parts[1];
            else
                Bitrate = string.Empty;
            OnPropertyChanged(nameof(Format));
            OnPropertyChanged(nameof(Bitrate));
        }
    }

    public static class NetTrackFileInfoExtensions
    {
        public static async Task<NetTrackFileInfo> GetNetTrackFileInfoAsync(this ISCPConnection connection)
        {
            return await connection.SendCommandAsync<NetTrackFileInfo>(new NetTrackFileInfo());
        }
    }
}
