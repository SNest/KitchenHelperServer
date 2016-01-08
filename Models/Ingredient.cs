namespace KitchenHelperServer.Models
{
    public class Ingredient : Entity
    {
        public int? RecipeId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual Recipe Recipe { get; set; }

        public string Quantity { get; set; }
    }
}