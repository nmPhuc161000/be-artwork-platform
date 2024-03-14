namespace be_project_swp.Core.Dtos.RequestOrder;
public class CancelRequest
{
    public bool IsDelete { get; set; }
}

public class UpdateRequest
{
    public bool IsActive { get; set; }
}

public class UpdateStatusRequest
{
    public bool StatusRequest { get; set; }
}
