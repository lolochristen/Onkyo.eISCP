using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public enum UITypes
    {
        List = 0,
        Menu = 1,
        Playback = 2,
        Popup = 3,
        Keyboard = 4,
        MenuList = 5
    }


    public class NetListInfoExtended : ISCPMessage
    {
        public NetListInfoExtended() : base("NLA")
        {
            NumberOfItems = 30;
            Sequence = 1;
            Layer = 1;
        }

        public short Sequence { get; set; }
        public short Layer { get; set; }
        public short StartIndex { get; set; }
        public short NumberOfItems { get; set; }

        public UITypes UIType { get; private set; }
        public bool Success { get; private set; }

        public List<NetListInfoItem> Items { get; private set;}

        protected override string BuildMessage()
        {
            return $"L{Sequence:X4}{Layer:X2}{StartIndex:X4}{NumberOfItems:X4}";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            Sequence = short.Parse(RawData.Substring(1, 4));
            UIType = (UITypes)int.Parse(RawData.Substring(6, 1));
            Success = RawData.Substring(5, 1) == "S" ? true : false;

            if (Success)
            {
                Items = Xml.Element("items").Elements()
                    .Select(e => NetListInfoItem.Parse(e)).ToList();
            }
        }
    }

    public static class NetListInfoExtendedExtensions
    {
        public static async Task<NetListInfoExtended> GetNetListExtended(this ISCPConnection connection, short layer, short nbrOfItems = 30, short startIndex = 0, short sequence = 0)
        {
            return await connection.SendCommandAsync<NetListInfoExtended>(new NetListInfoExtended() { Sequence = sequence, Layer = layer, NumberOfItems = nbrOfItems, StartIndex = startIndex });
        }
    }
}
