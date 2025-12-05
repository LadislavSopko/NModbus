using NModbus.Data;
using Xunit;

namespace NModbus.UnitTests.Data
{

    public class DefaultPointSourceFixture
    {
        [Theory]
        [InlineData(0, 42)]
        [InlineData(ushort.MaxValue - 1, 45)]
        [InlineData(77, 456)]
        [InlineData(ushort.MaxValue, 45123)]
        [InlineData(ushort.MaxValue - 2, 123)]
        public void AddValues(ushort startAddress, int value)
        {
            IPointSource<int> points = new DefaultPointSource<int>();

            points.WritePoints(startAddress, new []{ value });

            Assert.Equal(value, points.ReadPoints(startAddress, 1)[0]);
        }

        [Fact]
        public void Write_Read_Boundary_Block()
        {
            IPointSource<ushort> points = new DefaultPointSource<ushort>();

            var values = new ushort[] { 1, 2, 3 };

            points.WritePoints(ushort.MaxValue - 2, values);

            Assert.Equal(values, points.ReadPoints(ushort.MaxValue - 2, (ushort)values.Length));
        }

        [Fact]
        public void DefaultSlaveDataStore_Should_Handle_LastRegisters_Block()
        {
            var store = new DefaultSlaveDataStore();
            var values = new ushort[] { 1, 2, 3 };

            store.HoldingRegisters.WritePoints(ushort.MaxValue - 2, values);

            Assert.Equal(values, store.HoldingRegisters.ReadPoints(ushort.MaxValue - 2, (ushort)values.Length));
        }

    }
}
