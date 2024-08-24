using _01_UserLogin;
using FluentAssertions;

namespace Tests
{
    [TestClass]
    public class UserLoginTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Register_new_user_without_email_and_argument_exception()
        {
            UserLoginAdministration sut = new();
            sut.Register("", "", "");
        }

        [DataTestMethod]
        [DataRow("email")]
        [DataRow("email@")]
        [DataRow(".com")]
        [DataRow("@house.de")]
        [ExpectedException(typeof(ArgumentException))]
        public void Register_new_user_without_invalid_email_and_argument_exception(string email)
        {
            UserLoginAdministration sut = new();
            sut.Register(email, "", "");
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