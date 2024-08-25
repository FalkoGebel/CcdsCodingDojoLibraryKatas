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
            user.Email.Should().Be(email);
            user.Nickname.Should().BeEmpty();
            user.Confirmed.Should().BeFalse();
            user.RegistrationDate.Date.Should().Be(today.Date);
            user.LastLoginDate.Date.Should().Be(today.Date);
            user.LastUpdatedDate.Date.Should().Be(today.Date);
        }
    }
}