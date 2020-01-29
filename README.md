# Onkyo.eISCP
Yet another ISCP library to control Onkyo AVR receivers over Network using .NET Standard Library.

It's a library inspired by the other GitHub projects miracle2k/onkyo-eiscp-dotnet and hgjensen/onkyo-eiscp-remote-windows. I required something better regarding async programming and wait behavior for the calls for a Xamarin project. It's not by far not complete with all commands. So please download the specs (https://michael.elsdoerfer.name/onkyo/ISCP_AVR_134.xlsx) and implement remaining commands.

Example:
```C#
using (var receiver = new Receiver())
{
  // connect by IP
  //await receiver.ConnectAsync("192.168.168.125");

  // or discover
  var info = await receiver.DiscoverAndConnectAsync();
  Console.WriteLine($"Connected to {info.Model} on {info.IPAddress}");

  await receiver.PowerOnAsync(Zone.Zone2);
  ...
}
```
low level:
```C#
using (var c = new ISCPConnection())
{
    await c.ConnectAsync("192.168.168.125");
    await c.SendCommandAsync("PWR01");
}
```
