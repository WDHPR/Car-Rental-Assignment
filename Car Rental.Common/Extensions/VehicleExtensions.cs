using Car_Rental.Common.Enums;

namespace Car_Rental.Common.Extensions;

public static class VehicleExtensions
{
    public static int CostPerDay(VehicleTypes type)
    {
        return type switch
        {
            VehicleTypes.Motorcycle => 60,
            VehicleTypes.Sedan => 140,
            VehicleTypes.Combi => 180,
            VehicleTypes.Van => 120,
            _ => throw new ArgumentException("Vehicle Type does not exist", nameof(type)),
        };
    }

    public static int Duration(DateTime rented, DateTime returned) => (int)(returned - rented).TotalDays + 1;

}
