using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class NetListInfoSelectExtended : ISCPMessage
    {
        public NetListInfoSelectExtended() : base("NLA")
        {
            Layer = 1;
            Sequence = 0;
        }

        public short Sequence { get; set; }
        public short Layer { get; set; }
        public short Index { get; set; }

        protected override string BuildMessage()
        {
            return $"I{Sequence:X4}{Layer:X2}{Index:X4}----";
        }
    }


    public static class NetListInfoSelectExtendedExtensions
    {
        public static async Task SelectNetListInfoExtended(this ISCPConnection connection, short layer, short index, short sequence = 0)
        {
            await connection.SendCommandAsync(new NetListInfoSelectExtended() { Sequence = sequence, Layer = layer, Index = index });
            connection.MessageProcessingWaitHandle.WaitOne();
        }
    }
}
