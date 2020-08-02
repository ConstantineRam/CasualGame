using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoopSecurity
{
  public static bool IsOkay(ref int SecurityVariable, int MaxIterations = 1000)
  {
    if (SecurityVariable > MaxIterations)
    {
      Debug.LogError("Potentially endless loop was engaged.");
      return false;
    }

    return true;
  }
}
