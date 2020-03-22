using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetAlbumName : ISCPMessage
    {
        public NetAlbumName() : base("NAL")
        { }

        public string AlbumName { get; private set; }

        protected override string BuildMessage()
        {
            return "QSTN";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            if (RawData != null)
                AlbumName = Encoding.Default.GetString(Encoding.UTF8.GetBytes(RawData));
            else
                AlbumName = string.Empty;
            OnPropertyChanged(nameof(AlbumName));
        }
    }

    public static class NetAlbumNameExtensions
    {
        public static async Task<NetAlbumName> GetNetAlbumNameAsync(this ISCPConnection connection)
        {
            return await connection.SendCommandAsync<NetAlbumName>(new NetAlbumName());
        }
    }
}
