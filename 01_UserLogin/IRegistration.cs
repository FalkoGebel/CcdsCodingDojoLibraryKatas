namespace _01_UserLogin
{
    internal interface IRegistration
    {
        void Register(string email, string password, string nickname);
        void Confirm(string registrationNumber);
    }
}