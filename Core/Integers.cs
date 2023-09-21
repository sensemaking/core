namespace System;

public static class Integers
{
    public static int OneBasedIndex(this int offset)
    {
        return offset + 1;
    }

    public static void Times(this int source, Action action)
    {
        for(var i = 0; i < source; i++)
            action();
    }

    public static void Times(this int source, Action<int> action)
    {
        for(var i = 0; i < source; i++)
            action(i);
    }
}

