using System.Globalization;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public enum VocalDialogValues
    {
        Level0 = 0x00,
        Level1 = 0x01,
        Level2 = 0x02,
        Level3 = 0x03,
        Level4 = 0x04,
        Level5 = 0x05,
        UP = 0x06,
        DOWN = 0x07
    }

    public class VocalDialogCommand : ISCPMessage
    {
        private VocalDialogValues _selectedValue;

        public VocalDialogCommand() : base("VOC")
        {
        }

        public VocalDialogValues SelectedValue
        {
            get => _selectedValue;
            set
            {
                if (_selectedValue != value)
                {
                    _selectedValue = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override string BuildMessage()
        {
            if (string.IsNullOrEmpty(RawData))
                return ((int)SelectedValue).ToString("X2");
            return RawData;
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            SelectedValue = (VocalDialogValues)int.Parse(RawData, NumberStyles.HexNumber);
        }
    }

    public static class VocalDialogCommandExtensions
    {
        public static async Task<VocalDialogCommand> SetVocalDialogAsync(this ISCPConnection connection, VocalDialogValues value)
        {
            return await connection.SendCommandAsync<VocalDialogCommand>(new VocalDialogCommand { SelectedValue = value });
        }

        public static async Task<VocalDialogCommand> GetVocalDialogAsync(this ISCPConnection connection)
        {
            return await connection.SendCommandAsync<VocalDialogCommand>(new VocalDialogCommand { RawData = "QSTN" });
        }
    }
}