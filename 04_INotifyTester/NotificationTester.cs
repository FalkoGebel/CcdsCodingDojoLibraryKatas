using System.ComponentModel;
using System.Reflection;

namespace _04_INotifyTester
{
    public static class NotificationTester
    {
        public static void Verify<T>() where T : new()
        {
            Type type = typeof(T);
            bool isNotifyPropertyChanged = typeof(INotifyPropertyChanged).IsAssignableFrom(type);

            if (!isNotifyPropertyChanged)
                throw new ArgumentException($"\"{type.FullName}\" does not implement \"INotifyPropertyChanged\"");

            EventInfo eventInfo = type.GetEvent("PropertyChanged") ?? throw new ArgumentException($"\"{type.FullName}\" not valid - event \"PropertyChanged\" is missing");

            foreach (PropertyInfo pi in type.GetProperties())
            {
                MethodInfo? setMethod = pi.GetSetMethod() ?? throw new ArgumentException($"\"{type.FullName}\" not valid - \"{pi.Name}\" does not implement a setter");

                T initiatedObject = (T)Activator.CreateInstance(type);

                int count = 0;

                void IncrementCount() { count++; }
                MethodInfo? incMethod = typeof(NotificationTester).GetMethod("Verify");

                Delegate handler = Delegate.CreateDelegate(
                      eventInfo?.EventHandlerType,
                      initiatedObject,
                      incMethod);

                eventInfo.AddEventHandler(initiatedObject, handler);

                setMethod.Invoke(initiatedObject, [1]);

                if (count == 0)
                    throw new ArgumentException(($"\"{type.FullName}\" not valid - \"{pi.Name}\" setter does not implement \"OnPropertyChanged\""));
            }
        }
    }
}