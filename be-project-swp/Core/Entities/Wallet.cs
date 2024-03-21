using be_artwork_sharing_platform.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace be_project_swp.Core.Entities
{
    public class Wallet : BaseEntity<long>
    {
        public string User_Id { get; set; }
        public double Balance { get; set; }

        public ApplicationUser User { get; set; }
    }
}
