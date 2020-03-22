using Onkyo.eISCP;
using Onkyo.eISCP.Commands;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StartZone2TuneIn
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            var task = ExecCommands();
            task.Wait();

            Console.WriteLine("test done. [key] to conitnue");
            Console.ReadKey();
        }

        static async Task ExecCommands()
        {
            var receiver = new Receiver();
            //await receiver.ConnectAsync("192.168.168.125");

            var info = await receiver.DiscoverAndConnectAsync();
            Console.WriteLine($"Connected to {info.Model} on {info.IPAddress}");

            await receiver.PowerOnAsync(Zone.Zone2);

            await receiver.UpdateStatusAsync();

            await receiver.SelectInputAsync(Inputs.NET, Zone.Zone2);

            await receiver.SetNetServiceAsync(NetServices.TuneInRadio);

            await receiver.SelectNetListInfoByIndex(1); // first item > Presets

            await receiver.SelectNetListInfoByIndex(1); // first Preset

            Console.WriteLine("[ENTER]");
            Console.ReadKey();

            await receiver.SetNetServiceAsync(NetServices.TuneInRadio);

            await receiver.SelectNetListInfoByIndex(1); // first item > Presets

            await receiver.SelectNetListInfoByIndex(2); // second Preset

            Console.WriteLine("[ENTER]");
            Console.ReadKey();

            await receiver.SetNetServiceAsync(NetServices.TuneInRadio);

            await receiver.SelectNetListInfoByIndex(1); // first item > Presets

            await receiver.SelectNetListInfoByIndex(3); // third Preset

            Console.WriteLine("[ENTER]");
            Console.ReadKey();

            await receiver.PowerStandbyAsync(Zone.Zone2);

            receiver.Disconnect();
        }

    }
}
