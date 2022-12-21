namespace api.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            return age;
        }
    }





    // public static class DateTimeExtends
    // {
    //     public static DateOnly ToDateOnly(this DateTime date)
    //     {
    //         return new DateOnly(date.Year, date.Month, date.Day);
    //     }

    //     public static DateOnly? ToDateOnly(this DateTime? date)
    //     {
    //         return date != null ? (DateOnly?)date.Value.ToDateOnly() : null;
    //     }
    // }
}