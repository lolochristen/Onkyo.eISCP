using System.ComponentModel;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class Volume : ISCPMessage
    {
        private short _volumeLevel;

        public Volume() : this(Zone.Main)
        { }

        public Volume(Zone zone = Zone.Main) : base(zone == Zone.Zone2 ? "ZVL" : "MVL")
        {
        }

        public short VolumeLevel
        {
            get => _volumeLevel;
            set
            {
                if (_volumeLevel != value)
                {
                    _volumeLevel = value;
                    OnPropertyChanged();
                }
            }
        }
  
        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);

            if (RawData == "N/A")
                VolumeLevel = -1;
            else
                VolumeLevel = short.Parse(source.RawData, System.Globalization.NumberStyles.HexNumber);
        }

        protected override string BuildMessage()
        {
            return _volumeLevel.ToString("X2");
        }
    }

    public class VolumeStatus : Volume
    {
        public VolumeStatus(Zone zone = Zone.Main) : base(zone)
        { }

        protected override string BuildMessage()
        {
            return "QSTN";
        }
    }

    public static class VolumeExtensions
    {
        public static async Task<Volume> SetVolumeUpAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            return await connection.SendCommandAsync<Volume>(new Volume(zone) { RawData = "UP" });
        }

        public static async Task<Volume> SetVolumeDownAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            return await connection.SendCommandAsync<Volume>(new Volume(zone) { RawData = "DOWN" });
        }

        public static async Task<Volume> SetVolumeAsync(this ISCPConnection connection, short volume, Zone zone = Zone.Main)
        {
            return await connection.SendCommandAsync<Volume>(new Volume(zone) { VolumeLevel = volume });
        }

        public static async Task<Volume> GetVolumeAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            return await connection.SendCommandAsync<Volume>(new VolumeStatus(zone));
        }
    }
}
