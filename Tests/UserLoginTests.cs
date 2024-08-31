using _01_UserLogin;
using FluentAssertions;

namespace Tests
{
    [TestClass]
    public class UserLoginTests
    {
        [DataTestMethod]
        [DataRow("")]
        [DataRow("email")]
        [DataRow("email@")]
        [DataRow(".com")]
        [DataRow("@house.de")]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_invalid_emails(string email)
        {
            UserLoginAdministration.ValidateEmail(email);
        }

        [DataTestMethod]
        [DataRow("mail@mail.com")]
        [DataRow("peter_burk@web.ru")]
        [DataRow("white-snake@zoo.org")]
        public void Validate_valid_emails(string email)
        {
            UserLoginAdministration.ValidateEmail(email);
            Assert.IsTrue(true);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("pd")]
        [DataRow("password")]
        [DataRow("PA?)wordpass")]
        [DataRow("PA?)wordpa00")]
        [DataRow("pa?)wordpa01")]
        [DataRow("PP?)wordpa01")]
        [DataRow("PAsswordpa01")]
        [DataRow("PA))wordpa01")]
        [DataRow("PA/)WORDPA01")]
        [DataRow("PA/)wwRDPA01")]
        [DataRow("PA/)wo\nDPA01")]
        [DataRow("PA/)wo DPA01")]
        [DataRow("PA/)worDPA01\t")]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_invalid_passwords(string password)
        {
            UserLoginAdministration.ValidatePassword(password);
        }

        [DataTestMethod]
        [DataRow("0a23Z5_78901\\345G78v0")]
        [DataRow("2024IsASp€cia|Year")]
        [DataRow("UneverKnowWhere2Go4yourBro!What?")]
        public void Validate_valid_passwords(string password)
        {
            UserLoginAdministration.ValidatePassword(password);
            Assert.IsTrue(true);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("name")]
        [DataRow("nickname\\")]
        [DataRow("This-is-a-very-long-nickname_too_long-guy")]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_invalid_nicknames(string nickname)
        {
            UserLoginAdministration.ValidateNickname(nickname);
        }

        [DataTestMethod]
        [DataRow("Peter123")]
        [DataRow("Paul_99")]
        [DataRow("012345_and-this-is-cool")]
        public void Validate_valid_nicknames(string nickname)
        {
            UserLoginAdministration.ValidateNickname(nickname);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Get_special_characters()
        {
            string expected = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~€";
            UserLoginAdministration.GetSpecialCharacters().Should().Be(expected);
        }

        [TestMethod]
        public void Create_valid_ten_passwords()
        {
            for (int i = 0; i < 10; i++)
            {
                UserLoginAdministration.ValidatePassword(UserLoginAdministration.CreatePassword());
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void Register_new_user_with_valid_email_and_one_user_to_confirm_with_password_exists()
        {
            UserLoginAdministration sut = new();
            string email = "mail@mail.com";
            DateTime today = DateTime.Now;

            sut.Register(email, "", "");
            List<User> users = sut.GetUsers();
            users.Should().NotBeNull();
            users.Count().Should().Be(1);

            User user = users[0];

            user.Id.Should().NotBeNullOrEmpty();
            Guid.TryParse(user.Id, out _).Should().BeTrue();
            user.Email.Should().Be(email);
            user.Nickname.Should().BeEmpty();
            user.Password.Should().NotBeNullOrEmpty();
            UserLoginAdministration.ValidatePassword(user.Password);
            user.Confirmed.Should().BeFalse();
            user.RegistrationDate.Date.Should().Be(today.Date);
            user.LastLoginDate.Date.Should().Be(today.Date);
            user.LastUpdatedDate.Date.Should().Be(today.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Register_new_user_without_nickname_twice()
        {
            UserLoginAdministration sut = new();
            string email = "mail@mail.com";

            sut.Register(email, "", "");
            sut.Register(email, "", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Register_new_users_with_same_nickname()
        {
            UserLoginAdministration sut = new();
            string email1 = "mail1@mail.com";
            string email2 = "mail2@mail.com";
            string nickname = "Peter";

            sut.Register(email1, "", nickname);
            sut.Register(email2, "", nickname);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("email")]
        [DataRow("-1200")]
        [DataRow("0")]
        [DataRow("invalid_registration_number")]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_invalid_registration_number(string registrationNumber)
        {
            UserLoginAdministration.ValidateRegistrationNumber(registrationNumber);
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("55")]
        [DataRow("123478934")]
        public void Validate_valid_registration_number(string registrationNumber)
        {
            UserLoginAdministration.ValidateRegistrationNumber(registrationNumber).Should().Be(uint.Parse(registrationNumber));
        }

        [TestMethod]
        public void Confirm_unconfirmed_user()
        {
            UserLoginAdministration sut = new();
            string email = "mail1@mail.com";
            string nickname = "Peter";

            sut.Register(email, "", nickname);

            User user = sut.GetUsers()[0];

            sut.Confirm(user.RegistrationNumber.ToString());

            user.Confirmed.Should().BeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Confirm_confirmed_user()
        {
            UserLoginAdministration sut = new();
            string email = "mail1@mail.com";
            string nickname = "Peter";

            sut.Register(email, "", nickname);

            User user = sut.GetUsers()[0];
            string registrationNumber = user.RegistrationNumber.ToString();

            sut.Confirm(registrationNumber);
            sut.Confirm(registrationNumber);
        }

        [DataTestMethod]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_user_for_empty_loginname(string loginname)
        {
            UserLoginAdministration sut = new();
            sut.GetUserForLoginname(loginname);
        }

        [TestMethod]
        public void Get_user_for_loginname_email_or_nickname()
        {
            UserLoginAdministration sut = new();
            sut.Register("mail1@mail.com", "", "Peter");
            User userPeter = sut.GetUsers()[^1];
            string registrationNumber = userPeter.RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.Register("mail2@mail.com", "", "");
            User userNoName = sut.GetUsers()[^1];
            registrationNumber = userNoName.RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.Register("mail3@mail.com", "", "Thomas_P");
            User userThomasP = sut.GetUsers()[^1];
            registrationNumber = userThomasP.RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.Register("mail4@mail.com", "", "Clara");
            User userClara = sut.GetUsers()[^1];
            sut.GetUserForLoginname("mail1@mail.com").Should().BeEquivalentTo(userPeter);
            sut.GetUserForLoginname("Peter").Should().BeEquivalentTo(userPeter);
            sut.GetUserForLoginname("mail2@mail.com").Should().BeEquivalentTo(userNoName);
            sut.GetUserForLoginname("mail3@mail.com").Should().BeEquivalentTo(userThomasP);
            sut.GetUserForLoginname("Thomas_P").Should().BeEquivalentTo(userThomasP);
            sut.GetUserForLoginname("mail4@mail.com").Should().BeEquivalentTo(userClara);
            sut.GetUserForLoginname("mail10@mail.com").Should().BeNull();
            sut.GetUserForLoginname("Clara").Should().BeEquivalentTo(userClara);
            sut.GetUserForLoginname("Star").Should().BeNull();
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("mail1@mail.com", "")]
        [DataRow("Peter", "adlkjalsdf")]
        [DataRow("mail2@mail.com", "adlkjalsdf")]
        [DataRow("Paul", "2p348dfadf")]
        [DataRow("mail3@mail.com", "2p348dfadf")]
        [DataRow("mail4@mail.com", "ABcd01_-1223")]
        [DataRow("James", "ABcd01_-1223")]
        [DataRow("mail5@mail.com", "ABcd01_-1223")]
        [DataRow("mail6@mail.com", "ABcd01_-1223")]
        [DataRow("Marius", "ABcd01_-1223")]
        [ExpectedException(typeof(ArgumentException))]
        public void Login_with_invalid_loginname_and_or_password(string loginname, string password)
        {
            UserLoginAdministration sut = new();
            sut.Register("mail1@mail.com", "ABcd01_-1223", "Peter");
            string registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.Register("mail2@mail.com", "ABcd01_-1223", "Paul123");
            registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.Register("mail3@mail.com", "ABcd01_-1223", "");
            registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.Register("mail4@mail.com", "ABcd01_-1223", "James");
            sut.Register("mail5@mail.com", "ABcd01_-1223", "");
            sut.Login(loginname, password);
        }

        [DataTestMethod]
        [DataRow("mail1@mail.com", "ABcd01_-1223")]
        [DataRow("Peter", "ABcd01_-1223")]
        [DataRow("mail2@mail.com", "ABcd01_-1223")]
        [DataRow("Paul123", "ABcd01_-1223")]
        [DataRow("mail3@mail.com", "ABcd01_-1223")]
        public void Login_with_valid_loginname_and_password(string loginname, string password)
        {
            UserLoginAdministration sut = new();
            sut.Register("mail1@mail.com", "ABcd01_-1223", "Peter");
            string registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.Register("mail2@mail.com", "ABcd01_-1223", "Paul123");
            registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.Register("mail3@mail.com", "ABcd01_-1223", "");
            registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            string token = sut.Login(loginname, password);
            token.Should().NotBeNullOrEmpty();
            token.Length.Should().BeGreaterThanOrEqualTo(10);
        }

        [TestMethod]
        public void Give_invalid_token()
        {
            UserLoginAdministration sut = new();
            sut.TokenLifetimeInDays = -10;
            sut.IsLoginValid("").Should().BeFalse();
            sut.IsLoginValid("jfll23094jasdlfj20354jlf").Should().BeFalse();
            sut.Register("mail1@mail.com", "ABcd01_-1223", "Peter");
            string registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            string token = sut.Login("Peter", "ABcd01_-1223");
            sut.IsLoginValid(token).Should().BeFalse();
        }

        [TestMethod]
        public void Give_valid_token()
        {
            UserLoginAdministration sut = new();
            sut.TokenLifetimeInDays = 0;
            sut.Register("mail1@mail.com", "ABcd01_-1223", "Peter");
            string registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            string token = sut.Login("Peter", "ABcd01_-1223");
            sut.IsLoginValid(token).Should().BeTrue();
            sut.TokenLifetimeInDays = 10;
            sut.Register("mail2@mail.com", "ABcd01_-1223", "Pauly_0815");
            registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            token = sut.Login("Peter", "ABcd01_-1223");
            sut.IsLoginValid(token).Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("mail1@mail.com")]
        [ExpectedException(typeof(ArgumentException))]
        public void Request_password_reset_with_invalid_email(string email)
        {
            UserLoginAdministration sut = new();
            sut.RequestPasswordReset(email);
        }

        [TestMethod]
        public void Request_password_reset_with_valid_email()
        {
            UserLoginAdministration sut = new();
            sut.Register("mail1@mail.com", "ABcd01_-1223", "Peter");
            string registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.RequestPasswordReset(sut.GetUsers()[^1].Email);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("email")]
        [DataRow("-1200")]
        [DataRow("0")]
        [DataRow("invalid_registration_number")]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_invalid_reset_request_number(string resetRequestNumber)
        {
            UserLoginAdministration.ValidateResetRequestNumber(resetRequestNumber);
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("55")]
        [DataRow("123478934")]
        public void Validate_valid_reset_request_number(string resetRequestNumber)
        {
            UserLoginAdministration.ValidateResetRequestNumber(resetRequestNumber).Should().Be(uint.Parse(resetRequestNumber));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Reset_password_with_invalid_request_number()
        {
            UserLoginAdministration sut = new();
            sut.Register("mail1@mail.com", "ABcd01_-1223", "Peter");
            string registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.RequestPasswordReset(sut.GetUsers()[^1].Email);
            sut.ResetPassword((-12).ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reset_password_with_unknown_request_number()
        {
            UserLoginAdministration sut = new();
            sut.Register("mail1@mail.com", "ABcd01_-1223", "Peter");
            string registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.RequestPasswordReset(sut.GetUsers()[^1].Email);

            Random rnd = new();
            uint number = 0;

            do
            {
                number = (uint)rnd.Next();
            } while (number == 0 || number == sut.GetUsers()[^1].ResetRequestNumber);

            sut.ResetPassword(number.ToString());
        }

        [TestMethod]
        public void Reset_password_with_valid_and_known_request_number()
        {
            UserLoginAdministration sut = new();
            string oldPassword = "ABcd01_-1223";
            sut.Register("mail1@mail.com", oldPassword, "Peter");
            string registrationNumber = sut.GetUsers()[^1].RegistrationNumber.ToString();
            sut.Confirm(registrationNumber);
            sut.RequestPasswordReset(sut.GetUsers()[^1].Email);
            string resetRequestNumber = sut.GetUsers()[^1].ResetRequestNumber.ToString();
            sut.ResetPassword(resetRequestNumber);
            sut.GetUsers()[^1].Password.Should().NotBeNullOrEmpty();
            sut.GetUsers()[^1].Password.Should().NotBe(oldPassword);
        }
    }
}