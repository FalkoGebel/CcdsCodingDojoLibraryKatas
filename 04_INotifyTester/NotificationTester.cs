namespace _04_INotifyTester
{
    public static class NotificationTester
    {
        public static void Verify<T>() where T : new()
        {
            T t = new();

            foreach (var prop in t.GetType().GetProperties())
            {

            }
        }
    }
}