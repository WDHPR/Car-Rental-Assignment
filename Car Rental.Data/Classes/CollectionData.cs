﻿using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using Car_Rental.Common.Classes;
using System.Reflection;

namespace Car_Rental.Data.Classes;

public class CollectionData : IData
{
    readonly List<IPerson> _customers = new();
    readonly List<IVehicle> _vehicles = new();
    readonly List<IBooking> _bookings = new();
    
    public int NextVehicleId => _vehicles.Count == 0 ? 1 : _vehicles.Max(b => b.Id) + 1;
    public int NextCustomerId => _customers.Count == 0 ? 1 : _customers.Max(b => b.Id) + 1;
    public int NextBookingId => _bookings.Count == 0 ? 1 : _bookings.Max(b => b.Id) + 1;

    public CollectionData() => SeedData();

    //Add data to lists to create database
    void SeedData()
    {
        _vehicles.Add(new Car(NextVehicleId, "ABC432", "Saab", 123000, 0.9, VehicleTypes.Sedan));
        _vehicles.Add(new Car(NextVehicleId, "AVD123", "Toyota", 25000, 1, VehicleTypes.Sedan));
        _vehicles.Add(new Car(NextVehicleId, "FED666", "Volvo", 1500, 1.2, VehicleTypes.Combi));
        _vehicles.Add(new Car(NextVehicleId, "KDM744", "VW", 86000, 1, VehicleTypes.Van));
        _vehicles.Add(new Car(NextVehicleId, "BCA543", "Volvo", 47000, 1.5, VehicleTypes.Combi));
        _vehicles.Add(new Car(NextVehicleId, "RED987", "Tesla", 16000, 0.5, VehicleTypes.Sedan));
        _vehicles.Add(new Motorcycle(NextVehicleId, "ACB455", "Yamaha", 16000, 0.5, VehicleTypes.Motorcycle));
        _vehicles.Add(new Motorcycle(NextVehicleId, "NMK846", "Kawasaki", 25000, 0.6, VehicleTypes.Motorcycle));

        _customers.Add(new Customer(NextCustomerId, 123456, "John", "Doe"));
        _customers.Add(new Customer(NextCustomerId, 124557, "Diana", "Barrigan"));
        _customers.Add(new Customer(NextCustomerId, 514544, "Clinton", "Jones"));
        _customers.Add(new Customer(NextCustomerId, 123846, "Neil", "Caffrey"));
        _customers.Add(new Customer(NextCustomerId, 428753, "Peter", "Burke"));

        _bookings.Add(new Booking(NextBookingId, new DateTime(2023, 10, 31), Single<IPerson>(p => p.Id == 1), Single<IVehicle>(v => v.Id == 7)));
        _bookings.Add(new Booking(NextBookingId, new DateTime(2023, 11, 6), Single<IPerson>(p => p.Id == 2), Single<IVehicle>(v => v.Id == 2)));
        _bookings.Add(new Booking(NextBookingId, new DateTime(2023, 10, 19), Single<IPerson>(p => p.Id == 4), Single<IVehicle>(v => v.Id == 5)));

        //Change booking 3 to returned
        Single<IBooking>(b => b.Id == 3).ReturnVehicle(new DateTime(2023, 10, 25), 650);
    }

    public List<T> Get<T>(Func<T, bool>? expression)
    {
        try
        {
            var collections = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.FieldType == typeof(List<T>))
                ?? throw new InvalidOperationException($"Unsupported Type {typeof(T).Name}");

            var value = collections.GetValue(this) ?? throw new InvalidDataException();

            var collection = ((List<T>)value).AsQueryable();

            return expression == null ? collection.ToList() : collection.Where(expression).ToList();
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    public void Add<T>(T item)
    {
        try
        {
            var collections = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.FieldType == typeof(List<T>) && f.IsInitOnly)
                ?? throw new InvalidOperationException($"Unsupported Type {typeof(T).Name}");

            var list = collections.GetValue(this) as List<T> ?? throw new InvalidDataException("Could not add item to list");

            list.Add(item);
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }
    public T? Single<T>(Func<T, bool> expression)
    {
        try
        {
            var collections = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.FieldType == typeof(List<T>))
                ?? throw new InvalidOperationException($"Unsupported Type {typeof(T).Name}");

            var value = collections.GetValue(this) ?? throw new InvalidDataException();

            var collection = ((List<T>)value).AsQueryable();

            var item = collection.SingleOrDefault(expression);

            return item;
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    public IBooking RentVehicle(int vehicleId, int customerId)
    {
        try
        {
            var vehicle = Single<IVehicle>(v => v.Id == vehicleId);
            var customer = Single<IPerson>(p => p.Id == customerId);

            return vehicle == null || customer == null
                ? throw new ArgumentException("Customer or vehicle not found")
                : (IBooking)new Booking(NextBookingId, DateTime.Today, customer, vehicle);
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    public IBooking ReturnVehicle(int vehicleId)
    {
        try
        {
            var bookings = Get<IBooking>(b => b.Status == BookingStatuses.Open);
            var booking = bookings.SingleOrDefault(b => b.Vehicle.Id == vehicleId);

            return booking ?? throw new ArgumentException("Booking not found");
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }
}