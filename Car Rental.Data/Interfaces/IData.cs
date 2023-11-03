using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Data.Interfaces;

public interface IData
{
    List<T> Get<T>(Func<T, bool>? expression);
    T? Single<T>(Func<T, bool> expression);
    public void Add<T>(T item);

    int NextVehicleId { get; }
    int NextCustomerId { get; }
    int NextBookingId { get; }

    IBooking RentVehicle(int vehicleId, int customerId);
    IBooking ReturnVehicle(int vehicleId);

    //Default Interface Methods
    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatuses));
    public string[] VehicleTypeNames => Enum.GetNames(typeof(VehicleTypes));
    public VehicleTypes GetVehicleTypes(string name)
    {
        try
        {
            return (VehicleTypes)Enum.Parse(typeof(VehicleTypes), name);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
