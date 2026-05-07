using SmartEMS.API.Models;

public interface IAttendanceRepository
{
    Task<Attendance?> GetTodayAsync(int employeeId, DateTime date);
    Task AddAsync(Attendance attendance);
}