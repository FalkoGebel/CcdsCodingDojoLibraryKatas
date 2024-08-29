using System.Net.Mail;
using System.Text.RegularExpressions;

namespace _01_UserLogin
{
    public class UserLoginAdministration : IRegistration, ILogin
    {
        private List<User> _users;

        public UserLoginAdministration()
        {
            _users = [];
        }

        public void Confirm(string registrationNumber)
        {
            uint validatedRregistrationNumber = ValidateRegistrationNumber(registrationNumber);
            User? user = _users.FirstOrDefault(u => u.RegistrationNumber == validatedRregistrationNumber);

            if (user == null)
                throw new InvalidOperationException("No unconfired user find for this registration number");
            else
                user.RegistrationNumber = 0;

            SendConfirmationEmail(user);
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public void Register(string email, string password, string nickname)
        {
            ValidateEmail(email);

            if (password == string.Empty)
                password = CreatePassword();

            ValidatePassword(password);

            if (nickname != string.Empty)
            {
                ValidateNickname(nickname);

                // nickname still free (not used yet)
                if (_users.Any(u => u.Nickname == nickname))
                    throw new InvalidOperationException($"Nickname {nickname} already used.");
            }

            LoadUsers();

            // email still free (not used yet)
            if (_users.Any(u => u.Email == email))
                throw new InvalidOperationException($"Email {email} already registered.");

            // Create user with GUID as ID and registration number (not confirmed user)
            DateTime today = DateTime.Now;

            User newUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                Nickname = nickname,
                Password = password,
                RegistrationNumber = CreateRegistrationNumber(),
                RegistrationDate = today,
                LastLoginDate = today,
                LastUpdatedDate = today
            };

            _users.Add(newUser);

            SaveUsers();

            SendRegistrationEmail(newUser);
        }

        private void LoadUsers()
        {
            // TODO - get users from data
            //throw new NotImplementedException();
        }

        private void SaveUsers()
        {
            // TODO - save users to data
            //throw new NotImplementedException();
        }

        private static void SendRegistrationEmail(User user)
        {
            // TODO - Create registration email and send it
            //throw new NotImplementedException();
        }

        private static void SendConfirmationEmail(User user)
        {
            // TODO - Create confirmation email and send it
            //throw new NotImplementedException();
        }

        public static string CreatePassword()
        {
            Random rnd = new();
            List<char> chars = [];
            string specialChars = string.Concat(GetSpecialCharacters().OrderBy(c => rnd.Next()));
            string digits = string.Concat(Enumerable.Range(48, 10).Select(x => (char)x).OrderBy(c => rnd.Next()));
            string upperLetters = string.Concat(Enumerable.Range(65, 26).Select(x => (char)x).OrderBy(c => rnd.Next()));
            string lowerLetters = string.Concat(Enumerable.Range(97, 26).Select(x => (char)x).OrderBy(c => rnd.Next()));
            string allChars = specialChars + digits + upperLetters + lowerLetters;

            // two different digits
            chars.Add(digits[0]);
            chars.Add(digits[1]);

            // two different uppercase letters
            chars.Add(upperLetters[0]);
            chars.Add(upperLetters[1]);

            // two different lowercase letters
            chars.Add(lowerLetters[0]);
            chars.Add(lowerLetters[1]);

            // two different special characters
            chars.Add(specialChars[0]);
            chars.Add(specialChars[1]);

            // four more from all
            for (int i = 0; i < 4; i++)
                chars.Add(allChars[rnd.Next(allChars.Length)]);

            return string.Concat(chars.OrderBy(x => rnd.Next()));
        }

        public static void ValidatePassword(string password)
        {
            // at least 12 characters long
            if (password.Length < 12)
                throw new ArgumentException("Invalid password - has to be at least twelve characters long");

            // no white spaces
            if (password.Any(c => char.IsWhiteSpace(c)))
                throw new ArgumentException("Invalid password - must not have white spaces");

            // at least 2 different digits
            if (password.Where(c => char.IsDigit(c)).Distinct().Count() < 2)
                throw new ArgumentException("Invalid password - has to have at least two different digits");

            // at least 2 different upper letters
            if (password.Where(c => char.IsUpper(c)).Distinct().Count() < 2)
                throw new ArgumentException("Invalid password - has to have at least two different upper letters");

            // at least 2 different upper letters
            if (password.Where(c => char.IsLower(c)).Distinct().Count() < 2)
                throw new ArgumentException("Invalid password - has to have at least two different lower letters");

            string specialChars = GetSpecialCharacters();

            // at least 2 different special characters
            if (password.Where(c => specialChars.Contains(c)).Distinct().Count() < 2)
                throw new ArgumentException($"Invalid password - has to have at least two different special characters {specialChars}");
        }

        public static void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is missing");

            try
            {
                MailAddress m = new(email);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email");
            }
        }

        public static void ValidateNickname(string nickname)
        {
            // at least 5 characters and maximum 30 long and only letters, digits, '_' and '-'
            if (!Regex.IsMatch(nickname, @"^[\w\d_\-]{5,30}$"))
                throw new ArgumentException("Invalid nickname - 5 to 30 characters length and only digits, letters and special characters '_' and '-'");
        }

        public static string GetSpecialCharacters() => "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~€";

        private uint CreateRegistrationNumber()
        {
            Random rnd = new();
            uint output = 0;

            do
            {
                output = (uint)rnd.Next();
            } while (output == 0 || _users.Any(u => u.RegistrationNumber == output));

            return output;
        }

        public static uint ValidateRegistrationNumber(string registrationNumber)
        {
            if (uint.TryParse(registrationNumber, out uint result) && result > 0)
                return result;

            throw new ArgumentException("Invalid registration number");
        }

        public string Login(string loginname, string password)
        {
            // TODO - Get user for loginname (email or nickname)
            User? user = GetUserForLoginname(loginname);

            // TODO - Validate password


            // TODO - Validate login
            // TODO - User confirmed?
            // TODO - Create token and save to user

            throw new NotImplementedException();
        }

        public bool IsLoginValid(string token)
        {
            // TODO - Validate token

            throw new NotImplementedException();
        }

        public void RequestPasswordReset(string email)
        {
            throw new NotImplementedException();
        }

        public void ResetPassword(string resetRequestNumber)
        {
            throw new NotImplementedException();
        }

        public User? GetUserForLoginname(string loginname)
        {
            if (string.IsNullOrEmpty(loginname))
                throw new ArgumentException("Loginname is missing");

            User? user = null;

            user = _users.Where(u => u.Email == loginname).FirstOrDefault();

            if (user == null)
                user = _users.Where(u => u.Nickname == loginname).FirstOrDefault();

            return user;
        }
    }
}