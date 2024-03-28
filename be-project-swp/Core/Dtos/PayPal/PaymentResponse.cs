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
    public string AccessToken { get; set; }
    public string User_Id { get; set; }
    public long Artwork_Id { get; set; }
    public string NickName { get; set; }
    public OrderResponse Order { get; set; }
}