namespace _01_UserLogin
{
    public class User
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public string Nickname { get; set; } = "";
        public string Password { get; set; } = "";
        public bool Confirmed { get => RegistrationNumber == 0; }
        public uint RegistrationNumber { get; set; } = 0;
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
