using Car_Rental.Common.Enums;

public static class VehicleExtensions
{
    public static int CostPerDay(VehicleTypes type)
    {
        switch (type)
        {
            case VehicleTypes.Motorcycle:
                return 60;
            case VehicleTypes.Sedan:
                return 140;
            case VehicleTypes.Combi:
                return 180;
            case VehicleTypes.Van:
                return 120;
            default:
                throw new ArgumentException("Vehicle Type does not exist", nameof(type));
        }
    }

    public static int Duration(DateTime rented, DateTime returned) => (int)(returned - rented).TotalDays + 1;

}
