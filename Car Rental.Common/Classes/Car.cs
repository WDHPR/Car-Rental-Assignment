﻿using Car_Rental.Common.Enums;

namespace Car_Rental.Common.Classes;

public class Car : Vehicle
{
/*    public int Id { get; private set; }
    public string RegNo { get; init; }
    public string Make { get; init; }
    public int Odometer { get; set; }
    public double CostKm { get; set; }
    public int CostDay { get; set; }
    public VehicleTypes Type { get; init; }
    public VehicleStatuses Status { get; set; }
*/
    public Car(int id, string regNo, string make, int odometer, double costKm, int costDay, VehicleTypes type, VehicleStatuses status = VehicleStatuses.available)
        : base(id,regNo,make,odometer,costKm,costDay,type,status)
    {
        //Id = id;
        //RegNo = regNo;
        //Make = make;
        //Odometer = odometer;
        //CostKm = costKm;
        //CostDay = costDay;
        //Type = type;
        //Status = status;
    }

}
