namespace Api;

public static class DataTimeExtensions
{
    public static int CalculateAge(this DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - dob.Year;
        if (dob > today.AddYears(-age)) age--;
        // So sánh ngày sinh (dob) với ngày sinh nhật trong năm hiện tại.
        // Nếu ngày sinh của người đó lớn hơn (tức là sinh nhật chưa đến), giảm tuổi đi 1.
        return age;
    }
}
