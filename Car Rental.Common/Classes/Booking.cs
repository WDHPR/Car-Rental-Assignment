using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Common.Classes;

public class Booking : IBooking
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

    public Booking
        (int id,
        DateTime rented,
        IPerson customer,
        IVehicle vehicle,
        DateTime? returned = null,
        int? kmReturned = null,
        double? cost = null,
        BookingStatuses status = default)
    {
        Id = id;
        KmRented = vehicle.Odometer;
        KmReturned = kmReturned;
        Rented = rented;
        Returned = returned;
        Cost = cost;
        Vehicle = vehicle;
        Customer = customer;
        Status = status;
        Vehicle.Status = VehicleStatuses.Booked;
    }

    public void ReturnVehicle(DateTime returned, int kmDriven)
    {
        //add one day only if days is 1 or more
        var days = (returned - Rented).TotalDays +1;

        Vehicle.Odometer += kmDriven;
        Vehicle.Status = VehicleStatuses.Available;
        KmReturned = KmRented + kmDriven;
        Returned = returned;
        Status = BookingStatuses.Closed;
        Cost = days * Vehicle.CostDay + kmDriven * Vehicle.CostKm;
    }
}