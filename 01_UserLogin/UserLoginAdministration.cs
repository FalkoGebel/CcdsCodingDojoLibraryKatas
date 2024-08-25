using System.Net.Mail;
using System.Text.RegularExpressions;

namespace _01_UserLogin
{
    public partial class UserLoginAdministration : IRegistration
    {
        private List<User> _users;

        public UserLoginAdministration()
        {
            _users = [];
        }

        public void Confirm(string registrationNumber)
        {
            // TODO - Validate registration number
            // TODO - Confirm user for this registration number and invalidate registration number
            // TODO - Create confirmation email includig password, if not given by the user

            throw new NotImplementedException();
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public void Register(string email, string password, string nickname)
        {
            ValidateEmail(email);

            // TODO - email still free (not used yet)

            if (password == string.Empty)
                password = CreatePassword();

            ValidatePassword(password);

            if (nickname != string.Empty)
                ValidateNickname(nickname);

            // TODO - nickname still free (not used yet)

            // TODO - Create user (create ID) and save it to data
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

            // TODO - Create registration number and bind it to user
            // TODO - Create registration email and send it

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
    }
}