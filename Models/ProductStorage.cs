using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace KitchenHelperServer.Models
{
    public class ProductStorage 
    {
        [Key, ForeignKey("UserGroup")]
        public int UserGroupId { get; set; }

        public virtual UserGroup UserGroup { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public string State { get; set; }
    }
}