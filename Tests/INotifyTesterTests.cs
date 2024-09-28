using _04_INotifyTester;

namespace Tests
{
    [TestClass]
    public class INotifyTesterTests
    {
        [TestMethod]
        public void Test_Valid_Class_only_Ints()
        {
            NotificationTester.Verify<ValidTestClassInts>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_First_Invalid_Class_only_Ints()
        {
            NotificationTester.Verify<InvalidTestClassInts1>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Second_Invalid_Class_only_Ints()
        {
            NotificationTester.Verify<InvalidTestClassInts2>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_Invalid_Class_only_Strings()
        {
            NotificationTester.Verify<InvalidTestClassStrings>();
        }

        [TestMethod]
        public void Test_Valid_Class_only_Strings()
        {
            NotificationTester.Verify<ValidTestClassStrings>();
        }

        [TestMethod]
        public void Test_Complex_Valid_Class()
        {
            NotificationTester.Verify<ComplexValidClass>();
        }
    }
}
