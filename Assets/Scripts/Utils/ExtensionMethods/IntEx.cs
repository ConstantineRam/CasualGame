﻿using System;
namespace Extensions
{
  public static class IntEx
  {
    public static int RandomTo(this int i)
    {
      return UnityEngine.Random.Range(0, i);
    }

  }
}
