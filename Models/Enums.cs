namespace SmartEMS.API.Models;

public enum UserRole
{
    Admin,
    Employee
}

public enum AccountStatus
{
    Active,
    Inactive
}

public enum LeaveType
{
    Paid = 1,
    Sick = 2
}

public enum LeaveStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}

public enum CorrectionStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}