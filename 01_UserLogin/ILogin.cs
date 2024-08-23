namespace _01_UserLogin
{
    public interface ILogin
    {
        public string Login(string loginname, string password);
        public bool IsLoginValid(string token);
        public void RequestPasswordReset(string email);
        public void ResetPassword(string resetRequestNumber);
    }
}
