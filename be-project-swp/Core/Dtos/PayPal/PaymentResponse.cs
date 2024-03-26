namespace be_project_swp.Core.Dtos.PayPal;
public class OrderResponse
{
    public string Id { get; set; }
    public List<Link> Links { get; set; }
}

public class Link
{
    public string Href { get; set; }
    public string Rel { get; set; }
    public string Method { get; set; }
}
