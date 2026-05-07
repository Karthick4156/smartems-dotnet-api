namespace SmartEMS.API.DTOs.Correction;

public class CorrectionResponseDto
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;

    public DateTime Date { get; set; }
    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}