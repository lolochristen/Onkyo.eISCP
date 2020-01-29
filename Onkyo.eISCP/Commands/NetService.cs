using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP.Commands
{
    public enum NetServices
    {
        MusicServer = 0x00, 
        Favorite = 0x01, 
        vTuner = 0x02, 
        SiriusXM = 0x03, 
        Pandora = 0x04, 
        Rhapsody = 0x05, 
        Last_fm = 0x06,
        Napster = 0x07, 
        Slacker = 0x08, 
        Mediafly = 0x09, 
        Spotify = 0x0A, 
        AUPEO = 0x0B, 
        Radiko = 0x0C, 
        e_onkyo = 0x0D,
        TuneInRadio = 0x0E, 
        mp3tunes = 0x0F, 
        Simfy = 0x10, 
        HomeMedia = 0x11, 
        Deezer = 0x12, 
        iHeartRadio = 0x13, 
        Airplay = 0x18, 
        TIDAL = 0x19, 
        onkyo_music = 0x1A, 
        USB_Front = 0xF0, 
        USB_Rear = 0xF1
    }

    public class NetService : ISCPMessage
    {
        public NetService() : base ("NSV")
        { }

        public NetServices Serivce { get; set; }
        public bool HasAccountInfo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        protected override string BuildMessage()
        {
            if (!HasAccountInfo)
                return ((int)Serivce).ToString("X2")+"0";
            else
                return $"{((int)Serivce).ToString("X2")}1{Encoding.Default.GetString(Encoding.UTF8.GetBytes(Username)):128}{Encoding.Default.GetString(Encoding.UTF8.GetBytes(Password)):128}";
        }
    }

    public static class NetServiceExtensions
    {
        public static async Task SetNetServiceAsync(this ISCPConnection connection, NetServices service)
        {
            await connection.SendCommandAsync(new NetService() { Serivce = service }, 5000, (om, sm) => sm.Command == "NLS"); // after sending NSV, NLT + NLS (many) are expected
            connection.MessageProcessingWaitHandle.WaitOne();

        }
    }
}
