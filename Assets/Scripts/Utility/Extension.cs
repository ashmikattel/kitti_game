using System;
using System.Linq;
using System.Collections.Generic;

using kitti;

public static class ListUtils
{
    //source: https://stackoverflow.com/questions/8853747/linq-getting-consecutive-numbers-in-an-array
    //Extension method to find a subset of sequential consecutive elements with at least the specified count of members.
    //Comparisions are based on the field value in the selector.
    //Quick implementation for purposes of the example...
    //Ignores error and bounds checking for purposes of example.  
    //Also assumes we are searching for descending consecutive sequential values.
    public static IEnumerable<T> FindConsecutiveSequence<T>(this IEnumerable<T> sequence, Func<T, int> selector, int count)
    {
        int start = 0;
        int end = 1;
        T prevElement = sequence.First();

        foreach (T element in sequence.Skip(1))
        {
            if (selector(element) + 1 == selector(prevElement))
            {
                end++;
                if (end - start == count)
                {
                    return sequence.Skip(start).Take(count);
                }
            }
            else
            {
                start = end;
                end++;
            }
            prevElement = element;  
        }
        return sequence.Take(0);
    }

    public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> seq, Func<T, T, bool> condition)
    {
        T prev = seq.First();
        List<T> list = new List<T>() { prev };

        foreach (T item in seq.Skip(1))
        {
            if (condition(prev, item) == false)
            {
                yield return list;
                list = new List<T>();
            }
            list.Add(item);
            prev = item;
        }

        yield return list;
    }
}

//Compares cards based on value alone, not suit.
//Again, ignores validation for purposes of quick example.
public class CardValueComparer : IEqualityComparer<CardModel>
{
    public bool Equals(CardModel x, CardModel y)
    {
        return x.value == y.value ? true : false;
    }
    public int GetHashCode(CardModel c)
    {
        return c.value.GetHashCode();
    }
}