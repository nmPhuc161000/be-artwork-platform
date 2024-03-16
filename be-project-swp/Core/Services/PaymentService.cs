using be_artwork_sharing_platform.Core.DbContext;
using be_project_swp.Core.Base;
using be_project_swp.Core.Dtos.Zalopays;
using be_project_swp.Core.Entities;
using be_project_swp.Core.Interfaces;

namespace be_project_swp.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResultWithData<PaymentLinkDtos>> Handle(/*CreatePayment request, */CancellationToken cancellationToken, string userId)
        {
            var result = new BaseResultWithData<PaymentLinkDtos>();
            var payment = new Payment
            {
/*                Id = DateTime.Now.Ticks.ToString(),
                PaymentContent = request.PaymentContent,
                PaymentCurrency = request.PaymentCurrency,
                PaymentRefId = request.PaymentRefId,
                RequiredAmount = request.RequiredAmount,
                PaymentDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddMinutes(15),
                PaymentLanguage = request.PaymentLanguage,
                Signature = request.Signature,
                MerchantId = request.MerchantId,
                PaymentDestinationId = request.PaymentDestinationId,
                InsertUser = userId*/
            };
/*            _context.Payments.Add(payment);*/
            await _context.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
