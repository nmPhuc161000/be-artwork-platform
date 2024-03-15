using be_project_swp.Core.Base;
using be_project_swp.Core.Dtos.Zalopays;

namespace be_project_swp.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<BaseResultWithData<PaymentLinkDtos>> Handle(/*CreatePayment request, */CancellationToken cancellationToken, string userId);
    }
}
