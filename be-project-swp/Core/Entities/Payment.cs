using be_artwork_sharing_platform.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace be_project_swp.Core.Entities
{
    [Table("payments")]
    public class Payment 
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string PaymentContent { get; set; }
        public string PaymentCurrency { get; set; }
        public string PaymentRefId { get; set; }
        public double RequiredAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string PaymentLanguage { get; set; }
        public string Signature { get; set; }
        public string MerchantId { get; set; }
        public string PaymentDestinationId { get; set; }
        public string InsertUser { get; set; }
        public long Artwork_Id { get; set; }
        // RelationShip
        [ForeignKey("InsertUser")]
        public ApplicationUser User { get; set; }

        [ForeignKey("Artwork_Id")]
        public Artwork Artworks { get; set; }
    }
}
