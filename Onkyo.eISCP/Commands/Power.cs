using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public enum Zone
    {
        Main,
        Zone2
    }

    public class Power : ISCPMessage
    {
        private bool _systemOn;

        public Power() : this(Zone.Main)
        {
        }

        public Power(Zone zone) : base(zone == Zone.Zone2 ? "ZPW" : "PWR")
        {
        }

        public bool SystemOn
        {
            get => _systemOn; set
            {
                if (_systemOn != value)
                {
                    _systemOn = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override string BuildMessage()
        {
            return SystemOn ? "01" : "00";
        }

        public override void ParseFrom(ISCPMessage source)
        {
            base.ParseFrom(source);
            SystemOn = RawData == "01" ? true : false;
        }
    }

    public class PowerStatus : Power
    {
        public PowerStatus(Zone zone = Zone.Main) : base(zone)
        {
        }

        protected override string BuildMessage()
        {
            return "QSTN";
        }
    }

    public static class PowerExtensions
    {
        public static async Task PowerOnAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            await connection.SendCommandAsync(new Power(zone) { SystemOn = true });
        }

        public static async Task PowerStandbyAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            await connection.SendCommandAsync(new Power(zone) { SystemOn = false });
        }

        public static async Task<Power> GetPowerStatusAsync(this ISCPConnection connection, Zone zone = Zone.Main)
        {
            return await connection.SendCommandAsync<Power>(new PowerStatus(zone));
        }
    }
}
