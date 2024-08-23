namespace _01_UserLogin
{
    public interface IRegistration
    {
        public void Register(string email, string password, string nickname);
        public void Confirm(string registrationNumber);
    }
}