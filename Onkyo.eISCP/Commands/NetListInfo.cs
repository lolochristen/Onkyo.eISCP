using System;
using System.Collections.Generic;
using System.Text;

namespace Onkyo.eISCP.Commands
{
    public enum NetListInformationType
    {
        ASCIILetter = 'A',
        CursorPosition = 'C',
        UnicodeLetter = 'U'

    }
    public class NetListInfo : ISCPMessage
    {
        public NetListInfo() : base("NLS")
        { }

        public NetListInformationType InformationType { get; set; }
        public short Position { get; private set; }
        public char Property { get; private set; }
        public string Text { get; private set; }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            InformationType = (NetListInformationType)RawData.Substring(0, 1)[0];
            if (RawData.Substring(1, 1) == "-")
                Position = -1;
            else
                Position = short.Parse(RawData.Substring(1, 1));
            Property = RawData.Substring(2, 1)[0];
            Text = RawData.Substring(3);
        }
    }
}
