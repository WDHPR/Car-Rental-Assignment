using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;

namespace Car_Rental.Common.Interfaces;

public interface IBooking
{
    public int Id { get; }
    public int KmRented { get; init; }
    public int? KmReturned { get; set; }
    public double? Cost { get; set; }
    public DateTime Rented { get; init; }
    public DateTime? Returned { get; set; }
    public IVehicle Vehicle { get; set; }
    public IPerson Customer { get; init; }
    public BookingStatuses Status { get; set; }

    public void ReturnVehicle(DateTime returned, int kmDriven);
}
