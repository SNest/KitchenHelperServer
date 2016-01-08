namespace KitchenHelperServer.Models
{
    public class UserGroup : Entity
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public string AppToken { get; set; }

        public int FridgeId { get; set; }

        public int ProductStorageId { get; set; }
    }
}