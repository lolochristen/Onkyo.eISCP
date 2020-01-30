using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public class Mute : ISCPMessage
    {
        private bool _muted;

        public Mute() : this(Zone.Main)
        { }

        public Mute(Zone zone) : base(zone == Zone.Zone2 ? "ZMT" : "AMT")
        { }

        public bool Muted
        {
            get => _muted; set
            {
                if (_muted != value)
                {
                    _muted = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override string BuildMessage()
        {
            return Muted ? "01" : "00";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            Muted = RawData == "01" ? true : false;
        }
    }

    public class MuteStatus : Mute
    {
        public MuteStatus(Zone zone = Zone.Main) : base(zone)
        {
        }

        protected override string BuildMessage()
        {
            return "QSTN";
        }
    }

    public static class MuteExtenions
    {
        public static async Task MuteAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            await connection.SendCommandAsync(new Mute(zone) { Muted = true });
        }

        public static async Task UnMuteAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            await connection.SendCommandAsync(new Mute(zone) { Muted = false });
        }

        public static async Task<Mute> GetMuteStatusAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            return await connection.SendCommandAsync<Mute>(new MuteStatus(zone));
        }
    }
}
