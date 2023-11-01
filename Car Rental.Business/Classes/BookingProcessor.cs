using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Linq.Expressions;

namespace Car_Rental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _db;

    public string RegNoInput { get; set; } = string.Empty;
    public string MakeInput { get; set; } = string.Empty;
    public int OdometerInput { get; set; } = 0;
    public double CostKmInput { get; set; } = 0;
    public string VehicleTypeInput { get; set; } = string.Empty;
    public int? SsnInput { get; set; } = null;
    public string FirstNameInput { get; set; } = string.Empty;
    public string LastNameInput { get; set; } = string.Empty;
    public int DistanceInput { get; set; } = 0;
    public int CustomerId { get; set; }
    public bool Processing { get; set; }

    public BookingProcessor(IData db) => _db = db;
    
    public IEnumerable<IPerson> GetCustomers() => _db.Get<IPerson>(null);
   
    public IEnumerable<IBooking> GetBookings() => _db.Get<IBooking>(null);

    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses? status = null)
    {
        if(status == null)
            return _db.Get<IVehicle>(null);
        else
            return _db.Get<IVehicle>(v => v.Status == status);
    }

    public IPerson? GetCustomer(int ssn) => _db.Single<IPerson>(p => p.SSN == ssn);
    //change back to this code if input is only possible as string
/*    {
        try
        {
            var ssnToInt = Int32.Parse(ssn);
            return _db.Single<IPerson>(p => p.SSN == ssnToInt);
        }
        catch(FormatException e)
        {
            throw;
        }
    }
*/
    public IVehicle? GetVehicle(int vehicleId) => _db.Single<IVehicle>(v => v.Id == vehicleId);
    public IVehicle? GetVehicle(string regNo) => _db.Single<IVehicle>(v => v.RegNo == regNo);

    //TODO: possibly needs code to deactivate inputs while waiting for task to finish
    public async Task RentVehicle(int vehicleId, int customerId)
    {
        Processing = true;
        await Task.Delay(3000);
        var booking = _db.RentVehicle(vehicleId, customerId);
        _db.Add(booking);
        Processing = false;
    }

    public void ReturnVehicle(int vehicleId, int distance)
    {
        IBooking? booking = null;
        try
        {
            booking = _db.Single<IBooking>(b => b.Vehicle.Id == vehicleId);

            if (booking == null)
                throw new ArgumentException("Booking not found");
        }
        catch(Exception ex)
        {
            throw;
        }
        booking.ReturnVehicle(DateTime.Today, distance);
    }
    
    public void AddVehicle(string regNo, string make, int odometer, double costKm, VehicleTypes type, VehicleStatuses status = default)
    {
        IVehicle? item = null;

        if (type == VehicleTypes.Motorcycle)
            item = new Motorcycle(_db.NextVehicleId, regNo, make, odometer, costKm, type, status);
        else
            item = new Car(_db.NextVehicleId, regNo, make, odometer, costKm, type, status);

        _db.Add(item);
    }

    //TODO: add null-exception handling
    public void AddCustomer(int? ssnInput, string lastName, string firstName)
    {
        if (ssnInput == null || lastName == string.Empty || firstName == string.Empty)
            return;
        var ssn = (int)ssnInput;
        IPerson customer = new Customer(_db.NextCustomerId, ssn, firstName, lastName);
        _db.Add(customer);
    }

    //default interface methods
    public VehicleTypes GetVehicleTypes(string name) => _db.GetVehicleTypes(name);
    public string[] VehicleTypeNames => _db.VehicleTypeNames;
    public string[] VehicleStatusNames => _db.VehicleStatusNames;
}