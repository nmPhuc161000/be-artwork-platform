using be_project_swp.Core.Dtos.Order;

namespace be_project_swp.Core.Interfaces
{
    public interface IOrderService
    {
        Task<GetResultAfterPayment> GetBill(long id);
    }
}
