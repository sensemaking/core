using System.Threading.Tasks;

namespace System;

public static class TaskExtentions
{
    public static T Await<T>(this Task<T> task) => task.GetAwaiter().GetResult();
    public static void Await(this Task task) => task.GetAwaiter().GetResult();
}
