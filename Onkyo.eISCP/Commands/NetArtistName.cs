using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetArtistName : ISCPMessage
    {
        public NetArtistName() : base("NAT")
        { }

        public string ArtistName { get; private set; }

        protected override string BuildMessage()
        {
            return "QSTN";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            if (RawData != null)
                ArtistName = Encoding.Default.GetString(Encoding.UTF8.GetBytes(RawData));
            else
                ArtistName = string.Empty;
            OnPropertyChanged(nameof(ArtistName));
        }
    }

    public static class NetArtistNameExtensions
    {
        public static async Task<NetArtistName> GetNetArtistNameAsync(this ISCPConnection connection)
        {
            return await connection.SendCommandAsync<NetArtistName>(new NetArtistName());
        }
    }
}
