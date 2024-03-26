namespace be_project_swp.Core.Dtos.PayPal;
public class OrderResponse
{
    public string id { get; set; }
    public List<Link> links { get; set; }
}

public class Link
{
    public string href { get; set; }
    public string rel { get; set; }
    public string method { get; set; }
}

public class OrderAndTokenResponse
{
    public OrderResponse Order { get; set; }
    public string AccessToken { get; set; }
}