namespace SmartEMS.API.DTOs.Correction;
public class CreateCorrectionRequestDto
{
    public DateTime Date { get; set; }
    public string Reason { get; set; } = string.Empty;
}