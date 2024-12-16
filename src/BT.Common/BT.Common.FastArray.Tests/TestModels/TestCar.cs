namespace BT.Common.FastArray.Tests.TestModels
{
    internal enum DriveSystems
    {
        Fourwd,
        Awd,
        Fwd,
    }
    internal class TestCar : TestVehicle
    {
        public int NumberOfDoors { get; set; }
        public DriveSystems DriveSystem { get; set; }
    }
}