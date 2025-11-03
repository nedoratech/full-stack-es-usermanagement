using System.Reflection;

namespace UR.Events;

public static class EventsAssemblyReference
{
    public static Assembly Instance =>  typeof(EventsAssemblyReference).Assembly;
}