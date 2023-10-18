using Car_Rental.Common.Enums;

namespace Car_Rental.Common.Interfaces;

public interface IVehicle
{
    public int Id { get; }
    public string RegNo { get; init; }
    public string Make { get; init; }
    public int Odometer { get; set; }
    public double CostKm { get; set; }
    public int CostDay { get; set; }
    public VehicleTypes Type { get; init; }
    public VehicleStatuses Status { get; set; }
}
