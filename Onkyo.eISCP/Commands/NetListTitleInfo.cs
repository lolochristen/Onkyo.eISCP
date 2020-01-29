using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetListTitleInfo : ISCPMessage
    {
        public NetServices Service { get; private set; }
        public UITypes UIType { get; private set; }
        public byte LayerInfo { get; private set; }
        public short Layer { get; private set; }
        public bool StartFirst { get; private set; }
        public short IconLeft { get; private set; }
        public short IconRight { get; private set; }
        public short Status { get; private set; }
        public string Title { get; private set; }
        public short Position { get; private set; }
        public short ItemNumber { get; private set; }

        public NetListTitleInfo() : base("NLT")
        { 
        }

        protected override string BuildMessage()
        {
            return "QSTN";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            // xxuycccciiiillsraabbssnnn...nnn
            Service = (NetServices)int.Parse(RawData.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            UIType = (UITypes)int.Parse(RawData.Substring(2, 1));
            LayerInfo = byte.Parse(RawData.Substring(3,1));
            Position = short.Parse(RawData.Substring(4, 4), System.Globalization.NumberStyles.HexNumber);
            ItemNumber = short.Parse(RawData.Substring(8, 4), System.Globalization.NumberStyles.HexNumber);
            Layer = short.Parse(RawData.Substring(12, 2), System.Globalization.NumberStyles.HexNumber);
            StartFirst = RawData.Substring(14, 1) == "1";
            IconLeft = short.Parse(RawData.Substring(16, 2), System.Globalization.NumberStyles.HexNumber);
            IconRight = short.Parse(RawData.Substring(18, 2), System.Globalization.NumberStyles.HexNumber);
            Status = short.Parse(RawData.Substring(20, 2), System.Globalization.NumberStyles.HexNumber); // todo enum
            Title = RawData.Substring(22);
        }
    }

    public static class NetListTitleInfoExtensions
    {
        public static async Task<NetListTitleInfo> GetNetListTitleInfoAsync(this ISCPConnection connection)
        {
            return await connection.SendCommandAsync<NetListTitleInfo>(new NetListTitleInfo());
        }
    }

}
