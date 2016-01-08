using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenHelperServer.Models
{
    public class Fridge 
    {

        [Key, ForeignKey("UserGroup")]
        public int UserGroupId { get; set; }

        public virtual UserGroup UserGroup { get; set; }
        
        public virtual ICollection<Recipe> Dishes { get; set; }
    }
}