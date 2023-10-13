using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Interfaces;

public interface IPerson
{
    public int Id { get; }
    public int SSN { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}
