using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _db;

    //Variables for UI-inputs
    public string RegNoInput { get; set; } = string.Empty;
    public string MakeInput { get; set; } = string.Empty;
    public int OdometerInput { get; set; } = 0;
    public double CostKmInput { get; set; } = 0;
    public string VehicleTypeInput { get; set; } = string.Empty;
    public int? SsnInput { get; set; } = null;
    public string FirstNameInput { get; set; } = string.Empty;
    public string LastNameInput { get; set; } = string.Empty;
    public int? DistanceInput { get; set; } = null;
    public int CustomerId { get; set; }
    public bool Processing { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public BookingProcessor(IData db) => _db = db;

    public void ClearError() => ErrorMessage = string.Empty;
    
    public IEnumerable<IPerson> GetCustomers()
    {
        try
        {
            return _db.Get<IPerson>(null);
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
            return Enumerable.Empty<IPerson>();
        }
    }
   
    public IEnumerable<IBooking> GetBookings()
    {
        try
        {
            return _db.Get<IBooking>(null);
        }
        catch (Exception ex)
        {
            ErrorMessage= ex.Message;
            return Enumerable.Empty<IBooking>();
        }
    }

    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses? status = null)
    {
        try
        {
            if (status == null)
                return _db.Get<IVehicle>(null);
            else
                return _db.Get<IVehicle>(v => v.Status == status);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Enumerable.Empty<IVehicle>();
        }
    }

    public Customer? GetCustomer(int ssn)
    {
        try
        {
            return _db.Single<Customer>(p => p.SSN == ssn);
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
            return null;
        }
    }
    public IVehicle? GetVehicle(int vehicleId) => _db.Single<IVehicle>(v => v.Id == vehicleId);
    public IVehicle? GetVehicle(string regNo) => _db.Single<IVehicle>(v => v.RegNo == regNo);

    public async Task RentVehicle(int vehicleId, int customerId)
    {
        try
        {
            Processing = true;
            var booking = _db.RentVehicle(vehicleId, customerId);
            _db.Add(booking);
            await Task.Delay(3000);
            Processing = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public async Task ReturnVehicle(int vehicleId, int? distanceInput)
    {
        Processing = true;
        int distance;
        IBooking? booking;
        try
        {
            if (distanceInput == null)
            {
                Processing = false;
                throw new ArgumentException("Invalid input for distance");
            }

            distance = (int)distanceInput;
            booking = _db.ReturnVehicle(vehicleId);

            if (booking == null)
            {
                Processing = false;
                throw new ArgumentException("Booking not found");
            }

            booking.ReturnVehicle(DateTime.Today, distance);
            await Task.Delay(2000);
            Processing =false;
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
    
    public void AddVehicle(string regNo, string make, int odometer, double costKm, string type, VehicleStatuses status = default)
    {
        try
        {
            if(regNo == string.Empty || make == string.Empty || type == string.Empty || costKm <= 0)
                throw new ArgumentException("Invalid input");
            
            var vehicleType = GetVehicleTypes(type);

            IVehicle? item = null;

            if (vehicleType == VehicleTypes.Motorcycle)
                item = new Motorcycle(_db.NextVehicleId, regNo, make, odometer, costKm, vehicleType, status);
            else
                item = new Car(_db.NextVehicleId, regNo, make, odometer, costKm, vehicleType, status);

            _db.Add(item);
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void AddCustomer(int? ssnInput, string lastName, string firstName)
    {
        try
        {
            if (ssnInput == null || lastName == string.Empty || firstName == string.Empty)
                throw new ArgumentException("Invalid input");
            var ssn = (int)ssnInput;
            IPerson customer = new Customer(_db.NextCustomerId, ssn, firstName, lastName);
            _db.Add(customer);
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    //default interface methods
    public VehicleTypes GetVehicleTypes(string name) => _db.GetVehicleTypes(name);
    public string[] VehicleTypeNames => _db.VehicleTypeNames;
    public string[] VehicleStatusNames => _db.VehicleStatusNames;
}