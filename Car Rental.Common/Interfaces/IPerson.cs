namespace Car_Rental.Common.Interfaces;

public interface IPerson
{
    public int Id { get; }
    public int SSN { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}
