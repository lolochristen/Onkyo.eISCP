using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetTrackInfo : ISCPMessage
    {
        public NetTrackInfo() : base("NTR")
        { }

        public int CurrentTrack { get; private set; }
        public int TotalTrack { get; private set; }

        protected override string BuildMessage()
        {
            return "QSTN";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            //"cccc/tttt"
            if (RawData[0] != '-')
                CurrentTrack = int.Parse(RawData.Substring(0, 4));
            else
                CurrentTrack = -1;
            if (RawData[5] != '-')
                TotalTrack = int.Parse(RawData.Substring(5, 4));
            else
                TotalTrack = -1;
            OnPropertyChanged(nameof(CurrentTrack));
            OnPropertyChanged(nameof(TotalTrack));
        }
    }

    public static class NetTrackInfoExtensions
    {
        public static async Task<NetTrackInfo> GetNetTrackInfoAsync(this ISCPConnection connection)
        {
            return await connection.SendCommandAsync<NetTrackInfo>(new NetTitleName());
        }
    }
}
