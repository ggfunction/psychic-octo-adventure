namespace Clipboard
{
    using System;
    using System.ComponentModel;

    public static class ListExtensions
    {
        public static void Swap<T>(this BindingList<T> list, int indexA, int indexB)
        {
            if (indexA >= 0 && indexA < list.Count && indexB >= 0 && indexB < list.Count)
            {
                T temp = list[indexA];
                list[indexA] = list[indexB];
                list[indexB] = temp;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Index out of range");
            }
        }
    }
}
