using System.Reflection;

namespace UR.Events;

public static class EventsAssemblyReference
{
    public static Assembly Instance =>  typeof(EventsAssemblyReference).Assembly;
    public static string Namespace => typeof(EventsAssemblyReference).Namespace ?? string.Empty;
}