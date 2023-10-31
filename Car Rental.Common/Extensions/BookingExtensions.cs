using System.Runtime.CompilerServices;

namespace Car_Rental.Common.Extensions;

public static class BookingExtensions
{
    public static int Duration(DateTime rented, DateTime returned) => (int)(returned - rented).TotalDays + 1;
}
