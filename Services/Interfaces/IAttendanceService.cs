namespace SmartEMS.API.Services.Interfaces;

using SmartEMS.API.DTOs.Attendance;
using SmartEMS.API.DTOs.Correction;
public interface IAttendanceService
{
    Task PunchInAsync(string email);
    Task PunchOutAsync(string email);
    Task<List<AttendanceResponseDto>> GetMyAttendanceAsync(string email);
    Task ApproveCorrectionAsync(int id);
    Task<List<CorrectionResponseDto>> GetAllCorrectionsAsync();
    Task RejectCorrectionAsync(int id);
}