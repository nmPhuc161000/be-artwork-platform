using be_project_swp.Core.Dtos.RequestOrder;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace be_artwork_sharing_platform.Core.Entities
{
    [Table("requestorders")]
    public class RequestOrder
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string NickName_Sender { get; set; }
        public string NickName_Receivier { get; set; }
        public string UserId_Sender { get; set; }
        public string Text {  get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPayment { get; set; } = false;
        public StatusRequest StatusRequest { get; set; } = StatusRequest.Waiting;

        //Relationship
        public ApplicationUser User { get; set; }
    }
}
