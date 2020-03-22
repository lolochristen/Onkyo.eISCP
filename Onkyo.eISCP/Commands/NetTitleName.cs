using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetTitleName : ISCPMessage
    {
        public NetTitleName() : base ("NTI")
        { }

        public string Title { get; private set; }

        protected override string BuildMessage()
        {
            return "QSTN";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            if (!string.IsNullOrEmpty(RawData))
                Title = Encoding.Default.GetString(Encoding.UTF8.GetBytes(RawData));
            else
                Title = "";
            OnPropertyChanged(nameof(Title));
        }
    }

    public static class NetTitleNameExtensions
    {
        public static async Task<string> GetNetTitleNameAsync(this ISCPConnection connection)
        {
            var ntn = await connection.SendCommandAsync<NetTitleName>(new NetTitleName());
            return ntn.Title;
        }
    }
}
