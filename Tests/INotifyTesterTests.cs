using _04_INotifyTester;

namespace Tests
{
    [TestClass]
    public class INotifyTesterTests
    {
        [TestMethod]
        public void Test_Valid_Class()
        {
            NotificationTester.Verify<ValidTestClass>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Invalid_Class()
        {
            NotificationTester.Verify<InvalidTestClass>();
        }
    }
}
