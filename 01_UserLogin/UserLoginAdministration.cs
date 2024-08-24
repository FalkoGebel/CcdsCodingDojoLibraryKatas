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

        public List<User> GetUsers()
        {
            return _users;
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

            DateTime today = DateTime.Now;

            User newUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                Nickname = nickname,
                Confirmed = false,
                RegistrationDate = today,
                LastLoginDate = today,
                LastUpdatedDate = today
            };

            _users.Add(newUser);
        }
    }
}
