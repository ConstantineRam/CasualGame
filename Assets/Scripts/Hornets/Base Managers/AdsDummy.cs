using System;
using System.Collections.Generic;
using UnityEngine;
namespace Ads
{
  /// <summary>
  /// This class simulates ADs SDK.
  /// </summary>
  public class AdsDummy
  {
    public static bool IsReady(string placementId) => true;
    public static bool isShowing = false;
    //---------------------------------------------------------------------------------------------------------------
    public static void Show(string placementId, ShowOptions showOptions)
    {
      Debug.Log ("Player just enjoyed some cool ads.");
      showOptions.resultCallback(ShowResult.Finished);
    }

    public class ShowOptions
    {
      public Action<ShowResult> resultCallback { get; set; }
    }

    public enum ShowResult
    {
      Failed = 0,
      Skipped = 1,
      Finished = 2
    }
  }
}
