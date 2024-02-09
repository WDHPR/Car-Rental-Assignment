using Car_Rental.Common.Enums;

namespace Car_Rental.Common.Classes;

public class Motorcycle : Vehicle
{
    public Motorcycle(int id, string regNo, string make, int odometer, double costKm, VehicleTypes type, VehicleStatuses status = VehicleStatuses.Available)
        : base(id, regNo, make, odometer, costKm, type, status)
    {
    }

}
