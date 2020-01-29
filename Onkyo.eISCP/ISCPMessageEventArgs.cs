using System;

namespace Onkyo.eISCP
{
    public class ISCPMessageEventArgs : EventArgs
    {
        public ISCPMessage Message { get; set; }
    }
}
