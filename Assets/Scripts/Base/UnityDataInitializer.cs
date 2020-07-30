using System;

namespace GameBase
{

  public static class UnityDataInitializer
  {
    //---------------------------------------------------------------------------------------------------------------
    public static void CopyUnityData()
    {
      UnityDataInitializer.initializing = true;
      try
      {
        UnityData.CopyUnityData();
      }
      finally
      {
        UnityDataInitializer.initializing = false;
      }
    }

    public static bool initializing;
  }
}
