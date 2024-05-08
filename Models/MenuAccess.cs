namespace restaurant.Models
{
    public class MenuAccess
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string? MenuName { get; set; }
        public int MenuID { get; set; }
        public int CanAdd { get; set; }
        public int CanEdit { get; set; }
        public int CanDelete { get; set; }
        public int CanView { get; set; }
    }
}
