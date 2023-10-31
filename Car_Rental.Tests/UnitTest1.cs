namespace Car_Rental.Tests;

public class UnitTest1
{
    [Fact]
    public void VehicleIdTest()
    {
        //Create CollectionData instance
        CollectionData db = new();

        var idTest = db.NextVehicleId;

        Assert.NotNull(db);
        Assert.Equal(9,idTest);
    }
}