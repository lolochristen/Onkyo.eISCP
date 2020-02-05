using Onkyo.eISCP.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Onkyo.eISCP
{
    public class Receiver : ISCPConnection
    {
        public Power MainPower { get; protected set; }
        public Power Zone2Power { get; protected set; }
        public Volume MasterVolume { get; protected set; }
        public Volume Zone2Volume { get; protected set; }
        public NetListTitleInfo NetListTitle { get; protected set; }
        public Input MainInput { get; protected set; }
        public Input Zone2Input { get; protected set; }

        public NetListInfo NetListCursorInfo { get; protected set; }
        public ObservableCollection<NetListInfo> NetListItems { get; protected set; }

        public Receiver()
        {
            MainPower = new Power(Zone.Main);
            Zone2Power = new Power(Zone.Zone2);
            MasterVolume = new Volume();
            Zone2Volume = new Volume();
            NetListTitle = new NetListTitleInfo();
            MainInput = new Input(Zone.Main);
            Zone2Input = new Input(Zone.Zone2);
            NetListCursorInfo = new NetListInfo();
            NetListItems = new ObservableCollection<NetListInfo>();
            MessageReceived += OnMessageReceived;
        }

        protected override void OnConnected()
        {
            // notify....
        }

        private void OnMessageReceived(object sender, ISCPMessageEventArgs e)
        {
            switch(e.Message.Command)
            {
                case "MVL":
                    MasterVolume.ParseFrom(e.Message);
                    break;
                case "ZVL":
                    Zone2Volume.ParseFrom(e.Message);
                    break;
                case "NLT":
                    NetListTitle.ParseFrom(e.Message);
                    break;
                case "SLI":
                    MainInput.ParseFrom(e.Message);
                    break;
                case "SLZ":
                    Zone2Input.ParseFrom(e.Message);
                    break;
                case "PWR":
                    MainPower.ParseFrom(e.Message);
                    break;
                case "ZPW":
                    Zone2Power.ParseFrom(e.Message);
                    break;
                case "NLS":
                    var nls = new NetListInfo();
                    nls.ParseFrom(e.Message);

                    if (nls.InformationType == NetListInformationType.CursorPosition)
                    {
                        NetListCursorInfo.ParseFrom(e.Message);
                        lock(NetListItems)
                            NetListItems.Clear();
                    }
                    else
                    {
                        lock(NetListItems)
                            NetListItems.Add(nls);
                    }
                    break;
            }
        }

        public async Task UpdateStatusAsync()
        {
            // todo query commands.....
            // response will processed on event
            await this.GetPowerStatusAsync(Zone.Main);
            await this.GetVolumeAsync(Zone.Main);
            await this.GetInputAsync(Zone.Main);

            await this.GetPowerStatusAsync(Zone.Zone2);
            await this.GetVolumeAsync(Zone.Zone2);
            await this.GetInputAsync(Zone.Zone2);

            await this.GetNetListTitleInfoAsync();
        }
    }
}
