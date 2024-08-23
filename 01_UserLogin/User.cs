namespace _01_UserLogin
{
    public class User
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public string Nickname { get; set; } = "";
        public bool Confirmed { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
