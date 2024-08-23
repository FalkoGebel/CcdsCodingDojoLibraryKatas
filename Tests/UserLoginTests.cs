using _01_UserLogin;

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
    }
}