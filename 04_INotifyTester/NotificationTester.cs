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
                void IncrementCount(object sender, EventArgs e) { count++; }
                eventInfo.AddEventHandler(initiatedObject, new PropertyChangedEventHandler(IncrementCount));

                if (pi.PropertyType == typeof(string))
                {
                    setMethod.Invoke(initiatedObject, ["1"]);
                }
                else if (pi.PropertyType.IsArray)
                {
                    setMethod.Invoke(initiatedObject, [Array.CreateInstance(pi.PropertyType.GetElementType(), 1)]);
                }
                else
                {
                    setMethod.Invoke(initiatedObject, [Activator.CreateInstance(pi.PropertyType)]);
                }

                if (count == 0)
                    throw new ArgumentException(($"\"{type.FullName}\" not valid - \"{pi.Name}\" setter does not fire the event \"PropertyChanged\""));
            }
        }
    }
}