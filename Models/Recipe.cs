using System.Collections.Generic;
using System;

namespace KitchenHelperServer.Models
{
    public class Recipe : Entity
    {
        public string Name { get; set; }

		public Int32 MinutesForCooking { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }

        public virtual ICollection<RecipeStep> RecipeSteps { get; set; }
    }
}