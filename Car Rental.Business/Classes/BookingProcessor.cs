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

    public void ResetInput()
    {
        RegNoInput = string.Empty;
        MakeInput = string.Empty;
        OdometerInput = 0;
        CostKmInput = 0;
        VehicleTypeInput = string.Empty;
        SsnInput = null;
        FirstNameInput = string.Empty;
        LastNameInput = string.Empty;
        DistanceInput = null;
}
    
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

    public IPerson? GetCustomer(int ssn)
    {
        try
        {
            return _db.Single<IPerson>(p => p.SSN == ssn);
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
            return null;
        }
    }
    public IVehicle? GetVehicle(int vehicleId)
    {
        try
        {
            return _db.Single<IVehicle>(v => v.Id == vehicleId);
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
            return null;
        }
    }

    public IVehicle? GetVehicle(string regNo)
    {
        try
        {
            return _db.Single<IVehicle>(v => v.RegNo == regNo);
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
            return null;
        }
    }

    public async Task RentVehicle(int vehicleId, int customerId)
    {
        try
        {
            Processing = true;
            var booking = _db.RentVehicle(vehicleId, customerId);
            _db.Add(booking);
            await Task.Delay(3000);
            ResetInput();
            Processing = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Processing = false;
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
                throw new ArgumentException("Invalid input for distance");
            }

            distance = (int)distanceInput;
            booking = _db.ReturnVehicle(vehicleId);

            if (booking == null)
            {
                throw new ArgumentException("Booking not found");
            }

            booking.ReturnVehicle(DateTime.Today, distance);
            await Task.Delay(2000);
            ResetInput();
            Processing =false;
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
            Processing = false;
        }
    }
    
    public void AddVehicle(string regNo, string make, int odometer, double costKm, string type, VehicleStatuses status = default)
    {
        try
        {
            if(regNo == string.Empty || make == string.Empty || type == string.Empty || costKm <= 0)
                throw new ArgumentException("Invalid input");

            if (GetVehicle(regNo) != null)
                throw new ArgumentException($"Vehicle with registration {regNo} already exists");
            
            var vehicleType = GetVehicleTypes(type);

            IVehicle? item = null;

            if (vehicleType == VehicleTypes.Motorcycle)
                item = new Motorcycle(_db.NextVehicleId, regNo, make, odometer, costKm, vehicleType, status);
            else
                item = new Car(_db.NextVehicleId, regNo, make, odometer, costKm, vehicleType, status);

            _db.Add(item);
            ResetInput();
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

            if (GetCustomer(ssn) != null)
                throw new ArgumentException($"Customer with Social Security Number {ssn} already exists");

            IPerson customer = new Customer(_db.NextCustomerId, ssn, firstName, lastName);
            _db.Add(customer);
            ResetInput();
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