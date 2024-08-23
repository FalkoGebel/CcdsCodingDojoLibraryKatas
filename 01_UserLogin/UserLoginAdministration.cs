using System.Net.Mail;

namespace _01_UserLogin
{
    public class UserLoginAdministration : IRegistration
    {
        private List<User> _users;

        public UserLoginAdministration()
        {
            _users = [];
        }

        public void Confirm(string registrationNumber)
        {
            throw new NotImplementedException();
        }

        public void Register(string email, string password, string nickname)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is missing");

            try
            {
                MailAddress m = new MailAddress(email);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email");
            }

            throw new NotImplementedException();
        }
    }
}
