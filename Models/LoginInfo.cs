namespace restaurant.Models
{
    public class LoginInfo
    {
        public int LoginId { get; set; }
        public string TableName { get; set; }

        public LoginInfo(int loginId, string tableName)
        {
            LoginId = loginId;
            TableName = tableName;
        }
    }
}
