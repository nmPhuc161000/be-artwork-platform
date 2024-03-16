using Amazon.Runtime.Internal;
using be_project_swp.Core.Base;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace be_project_swp.Core.Dtos.Zalopays;
public interface IRequest<T> { };
public class CreatePayment : IRequest<BaseResultWithData<PaymentLinkDtos>>
{
    public string PaymentContent { get; set; } = string.Empty;
    public string PaymentCurrency { get; set; } = string.Empty;
    public string PaymentRefId { get; set; } = string.Empty;
    public double RequiredAmount { get; set; }
    public DateTime? PaymentDate { get; set; } = DateTime.Now;
    public DateTime? ExpireDate { get; set; } = DateTime.Now.AddMinutes(15);
    public string? PaymentLanguage { get; set; } = string.Empty;
    public string? MerchantId { get; set; } = string.Empty;
    public string? PaymentDestinationId { get; set; } = string.Empty;
    public string? Signature { get; set; } = string.Empty;
}

