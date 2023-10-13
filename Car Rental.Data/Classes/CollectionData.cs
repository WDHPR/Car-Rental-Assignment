using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using Car_Rental.Common.Classes;

namespace Car_Rental.Data.Classes;

public class CollectionData : IData
{
    readonly List<IPerson> _customers = new();
    readonly List<IVehicle> _vehicles = new();
    readonly List<IBooking> _bookings = new();

    public CollectionData() => SeedData();

    //Add data to lists to create database
    void SeedData()
    {
        _vehicles.Add(new Car(1, "ABC432", "Saab", 123000, 0.9, 100, VehicleTypes.Sedan));
        _vehicles.Add(new Car(2, "AVD123", "Toyota", 25000, 1, 120, VehicleTypes.Sedan));
        _vehicles.Add(new Car(3, "FED666", "Volvo", 1500, 1.2, 180, VehicleTypes.Combi));
        _vehicles.Add(new Car(4, "KDM744", "VW", 86000, 1, 100, VehicleTypes.Van));
        _vehicles.Add(new Car(5, "BCA543", "Volvo", 47000, 1.5, 150, VehicleTypes.Combi));
        _vehicles.Add(new Car(6, "RED987", "Tesla", 16000, 0.5, 200, VehicleTypes.Sedan));
        _vehicles.Add(new Motorcycle(7, "ACB455", "Yamaha", 16000, 0.5, 60, VehicleTypes.Motorcycle));
        _vehicles.Add(new Motorcycle(8, "NMK846", "Kawasaki", 25000, 0.6, 120, VehicleTypes.Motorcycle));

        _customers.Add(new Customer(1, 123456, "John", "Doe"));
        _customers.Add(new Customer(2, 124557, "Diana", "Barrigan"));
        _customers.Add(new Customer(3, 514544, "Clinton", "Jones"));
        _customers.Add(new Customer(4, 123846, "Neil", "Caffrey"));
        _customers.Add(new Customer(5, 428753, "Peter", "Burke"));

        _bookings.Add(new Booking(1, new DateTime(2023, 10, 10), GetCustomer(1), GetVehicle(7)));
        _bookings.Add(new Booking(2, new DateTime(2023, 10, 7), GetCustomer(2), GetVehicle(2)));
        _bookings.Add(new Booking(3, new DateTime(2023, 9, 25), GetCustomer(4), GetVehicle(5)));

        //Change booking 3 to returned
        GetBooking(3).ReturnVehicle(new DateTime(2023, 9, 30), 650);
    }

    public IEnumerable<IPerson> GetCustomers() => _customers;
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default) => _vehicles;
    public IEnumerable<IBooking> GetBookings() => _bookings;

    IPerson GetCustomer(int id) => _customers.First(c => c.Id == id);
    IVehicle GetVehicle(int id) => _vehicles.First(c => c.Id == id);
    IBooking GetBooking(int id) => _bookings.First(c => c.Id == id);
}