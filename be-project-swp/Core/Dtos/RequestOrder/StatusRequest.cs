namespace be_project_swp.Core.Dtos.RequestOrder;
public class CancelRequest
{
    public bool IsDelete { get; set; } = true;
}

public class UpdateRequest
{
    public bool IsActive { get; set; } = false;
}

public class UpdateStatusRequest
{
    public bool StatusRequest { get; set; } = true;
}
