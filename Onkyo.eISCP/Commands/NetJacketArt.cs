using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetJacketArt : ISCPMessage
    {
        public NetJacketArt() : base("NJA")
        { }

        public char ImageType { get; private set; }
        public char PacketFlag { get; private set; }
        public string ArtData { get; private set; }

        protected override string BuildMessage()
        {
            return "REQ";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            //"tpxxxxxxxxxxxx"
            ImageType = RawData[0];
            PacketFlag = RawData[1];
            ArtData = RawData.Substring(2);
            OnPropertyChanged(nameof(ArtData));
        }
    }

    public static class NetJacketArtExtensions
    {
        public static async Task<NetJacketArt> GetNetJacketArtAsync(this ISCPConnection connection)
        {
            return await connection.SendCommandAsync<NetJacketArt>(new NetJacketArt());
        }
    }
}
