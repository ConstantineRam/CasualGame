using System;
using System.Threading;
using UnityEngine;

namespace GameBase
{

  public static class UnityData
  {
    public static bool IsInMainThread
    {
      get
      {
        return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
      }
    }

    public static bool Is32BitBuild
    {
      get
      {
        return IntPtr.Size == 4;
      }
    }

    public static bool Is64BitBuild
    {
      get
      {
        return IntPtr.Size == 8;
      }
    }

    //---------------------------------------------------------------------------------------------------------------
    static UnityData()
    {
      if (!UnityData.initialized && !UnityDataInitializer.initializing)
      {
        Debug.LogError("Used UnityData before it's initialized.");
      }
    }

    //---------------------------------------------------------------------------------------------------------------
    public static void CopyUnityData()
    {
      UnityData.mainThreadId = Thread.CurrentThread.ManagedThreadId;
      UnityData.isEditor = Application.isEditor;
      UnityData.dataPath = Application.dataPath;
      UnityData.platform = Application.platform;
      UnityData.persistentDataPath = Application.persistentDataPath;
      UnityData.initialized = true;
    }


    private static bool initialized;
    public static bool isEditor;
    public static string dataPath;
    public static RuntimePlatform platform;
    public static string persistentDataPath;
    private static int mainThreadId;
  }
}
