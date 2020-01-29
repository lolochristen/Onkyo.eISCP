namespace Onkyo.eISCP.Commands
{
    public class NetListInfoSelectExtended : ISCPMessage
    {
        public NetListInfoSelectExtended() : base("NLA")
        {
            Layer = 1;
            Sequence = 1;
        }

        public short Sequence { get; set; }
        public short Layer { get; set; }
        public short Index { get; set; }

        protected override string BuildMessage()
        {
            return $"I{Sequence:X4}{Layer:X2}{Index:X4}----";
        }
    }
}
