namespace BT.Common.FastArray.Tests.TestModels
{
    internal class TestVehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is TestVehicle vehicle)
            {
                return Make == vehicle.Make &&
                   Model == vehicle.Model &&
                   Year == vehicle.Year &&
                   Color == vehicle.Color;
            }
            dynamic dynamic = obj;

            return Make == dynamic.Make &&
                   Model == dynamic.Model &&
                   Year == dynamic.Year &&
                   Color == dynamic.Color;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}