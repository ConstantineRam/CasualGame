﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
  public static class ListEx
  {
    public static bool HasIndex<T>(this IEnumerable<T> list, int i)
    {
      return i > -1 && i < list.Count();
    }

    public static bool IsEmpty<T>(this IEnumerable<T> list)
    {
      return !list.Any();
    }

    public static ICollection AddAndGet<T>(this List<T> list, T item)
    {
      list.Add(item);
      return list;
    }

    public static T GetRandom<T>(this IEnumerable<T> list)
    {
      int index = list.Count().RandomTo();
      return list.ElementAt(index);
    }
  }
}
