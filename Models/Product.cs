
using System.Collections.Generic;

namespace KitchenHelperServer.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}