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
        public void Test_First_Invalid_Class()
        {
            NotificationTester.Verify<InvalidTestClass1>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Second_Invalid_Class()
        {
            NotificationTester.Verify<InvalidTestClass2>();
        }
    }
}
