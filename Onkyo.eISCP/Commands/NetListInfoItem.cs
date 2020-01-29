using System.Xml.Linq;

namespace Onkyo.eISCP.Commands
{
    public class NetListInfoItem
    {
        public short Icon { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        //public string IconType { get; set; }
        public bool Selectable { get; set; }

        public static NetListInfoItem Parse(XElement item)
        {
            return new NetListInfoItem()
            {
                Icon = short.Parse(item.Attribute("iconid").Value, System.Globalization.NumberStyles.HexNumber),
                Title = item.Attribute("title").Value,
                Selectable = item.Attribute("selectable") != null ? (item.Attribute("selectable").Value == "1" ? true : false) : false
            };
        }
    }
}
