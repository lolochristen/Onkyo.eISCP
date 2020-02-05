using System;
using System.Threading.Tasks;
using Xunit;
using Onkyo.eISCP.Commands;

namespace Onkyo.eISCP.Tests
{
    public class IntegrationTests
    {
        private const string IPAddress = "192.168.168.125";

        [Fact]
        public async Task ConnectTest()
        {
            using (var connection = new ISCPConnection())
            {
                await connection.ConnectAsync(IPAddress);
            }
        }

        [Fact]
        public async Task SetVolumeTest()
        {
            using (var connection = new ISCPConnection())
            {
                await connection.ConnectAsync(IPAddress);

                await connection.PowerOnAsync(Zone.Main);

                var vol = await connection.SendCommandAsync<Volume>(new Volume(Zone.Main) { VolumeLevel = 80 });

                Assert.Equal(80, vol.VolumeLevel);
            }
        }

        [Fact]
        public async Task SetVolumeExtTest()
        {
            using (var connection = new ISCPConnection())
            {
                await connection.ConnectAsync(IPAddress);

                await connection.PowerOnAsync();

                var vol = await connection.SetVolumeAsync(90);

                Assert.Equal(90, vol.VolumeLevel);
            }
        }
    }
}
