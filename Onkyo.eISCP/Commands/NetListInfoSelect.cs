using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetListInfoSelect : ISCPMessage
    {
        public NetListInfoSelect() : base("NLS")
        {
        }

        public int IndexNbr { get; set; }

        protected override string BuildMessage()
        {
            return $"I{IndexNbr:00000}";
        }

        public static NetListInfoSelect Index(int index)
        {
            return new NetListInfoSelect() { IndexNbr = index };
        }
    }

    public static class NNetListInfoSelectExtensions
    {
        public static async Task SelectNetListInfoByIndex(this ISCPConnection connection, short index)
        {
            await connection.SendCommandAsync(new NetListInfoSelect() { IndexNbr = index });
            connection.MessageProcessingWaitHandle.WaitOne();
        }
    }
}
