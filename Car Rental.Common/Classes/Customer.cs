using Car_Rental.Common.Interfaces;

namespace Car_Rental.Common.Classes;

public class Customer : IPerson
{
    public int Id { get; private set; }
    public int SSN { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public Customer(int id, int ssn, string firstName, string lastName)
    {
        Id = id;
        SSN = ssn;
        FirstName = firstName;
        LastName = lastName;
    }
}
