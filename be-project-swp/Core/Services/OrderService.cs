using be_artwork_sharing_platform.Core.DbContext;
using be_project_swp.Core.Dtos.Order;
using be_project_swp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace be_project_swp.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetResultAfterPayment> GetBill(long id)
        {
            var orderBill = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            var bill = new GetResultAfterPayment()
            {
                Url_Image = orderBill.Url_Image,
                Name_Artwork = orderBill.Name_Artwork,
                NickName_Buyer = orderBill.NickName_Buyer,
                NickName_Seller = orderBill.NickName_Seller,
                Date_Payment = orderBill.CreatedAt,
                Category_Artwork = orderBill.Category_Artwork
            };
            return bill;
        }
    }
}
