namespace BT.Common.FastArray.Tests.TestModels
{
    internal enum DriveSystems
    {
        FOURWD,
        AWD,
        FWD,
    }
    internal class TestCar : TestVehicle
    {
        public int NumberOfDoors { get; set; }
        public DriveSystems DriveSystem { get; set; }
    }
}