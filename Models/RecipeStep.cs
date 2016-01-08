namespace KitchenHelperServer.Models
{
    public class RecipeStep : Entity
    {
        public string Description { get; set; }

        public int? RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}