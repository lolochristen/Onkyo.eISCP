using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Onkyo.eISCP
{
    public class ISCPMessage : INotifyPropertyChanged
    {
        private static readonly byte[] iscpPackage = new byte[]
           {
                0x49, 0x53, 0x43, 0x50,
                0x00, 0x00, 0x00, 0x10,
                0x00, 0x00, 0x00, 0xFF,
                0x01, 0x00, 0x00, 0x00
           };

        public string Command { get; protected set; }
        public string RawData { get; set; }
        public XElement Xml { get; protected set; }

        public static ISCPMessage CreateCommand(string command, string rawData = null)
        {
            return new ISCPMessage() { Command = command, RawData = rawData };
        }

        public ISCPMessage()
        {
        }

        public ISCPMessage(string command)
        {
            Command = command;
        }

        public virtual void ParseFrom(ISCPMessage source)
        {
            // just copy
            Command = source.Command;
            RawData = source.RawData;
            Xml = source.Xml;
        }

        internal static byte[] Magic = Encoding.ASCII.GetBytes("ISCP");

        public event PropertyChangedEventHandler PropertyChanged;

        public static ISCPMessage Parse(byte[] buf)
        {
            if (buf.Take(4).SequenceEqual(Magic))
            {
                // ISCP
                var messageSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buf, 8));
                var msg = Encoding.ASCII.GetString(buf, 16, messageSize);

                var end = msg.IndexOfAny(new[] { (char)0x0a, (char)0x0d, (char)0x1a });
                msg = msg.Substring(2, end - 2); // remove !1 and EOF

                return ISCPMessage.Parse(msg);
            }
            return null;
        }

        public static ISCPMessage Parse(string raw)
        {
            if (raw[0] == '!')
                raw = raw.Substring(2); // skip !1

            raw = raw.TrimEnd((char)0x0a, (char)0x0d, (char)0x1a);

            var m = new ISCPMessage()
            {
                Command = raw.Length >= 3 ? raw.Substring(0, 3) : ""
            };

            if (raw.Length > 3)
            {
                var i = raw.IndexOf("<?xml");
                if (i > 0)
                {
                    m.RawData = raw.Substring(3, i - 3);
                    m.Xml = XDocument.Parse(raw.Substring(i)).Root;
                }
                else
                    m.RawData = raw.Substring(3);
            }
            return m;
        }

        public T Parse<T>() where T : ISCPMessage, new()
        {
            var msg = new T();
            msg.ParseFrom(this);
            return msg;
        }

        protected virtual string BuildMessage()
        {
            return RawData != null ? RawData : "";
        }

        public byte[] GetBytes(string prefix = "!1")
        {
            var bytes = iscpPackage.ToList();
            var msg = BuildMessage();
            var mb = Encoding.ASCII.GetBytes($"{prefix}{Command}{msg}");
            bytes[11] = (byte)mb.Length;
            bytes.AddRange(mb);
            bytes.Add(0x0d);
            return bytes.ToArray();
        }

        public override string ToString()
        {
            var m = BuildMessage();
            return Command + m + Xml?.ToString();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
