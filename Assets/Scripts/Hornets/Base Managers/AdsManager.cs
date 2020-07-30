using System;
using Assets.Scripts.Utils.ExtensionMethods;
using Ads;
using UnityEngine;

public enum AdsType
{
  [Description("video")]
  VIDEO,
  [Description("rewardedVideo")]
  REWARDED_VIDEO,
}
/// <summary>
/// This is our shell for ADs SDK, in case of changing ADs platform all changes would be localised here.
/// </summary>
public class AdsManager : MonoBehaviour
{
  public const AdsType DefaultPlacementID = AdsType.REWARDED_VIDEO;
  private Action onAdsWatchedCallback;
  private Action onAdsNotWatchedCallback;
  //---------------------------------------------------------------------------------------------------------------
  public bool IsAvailable(AdsType type = DefaultPlacementID)
  {
    return AdsDummy.IsReady(type.GetDesc());
  }

  //---------------------------------------------------------------------------------------------------------------
  public void Show(Action callbackWatched, Action callbackNotWatched, AdsType type = DefaultPlacementID)
  {
    onAdsWatchedCallback = callbackWatched;
    onAdsNotWatchedCallback = callbackNotWatched;
    if (!IsAvailable(type))
    {
      return;
    }

    AdsDummy.Show(type.GetDesc(), new AdsDummy.ShowOptions()
    {
      resultCallback = OnAdsWatched
    });

  }

  //---------------------------------------------------------------------------------------------------------------
  private void OnAdsWatched(AdsDummy.ShowResult result)
  {
    if (result == AdsDummy.ShowResult.Finished)
    {
      if (onAdsWatchedCallback != null)
      {
        onAdsWatchedCallback.Invoke();
      }
      
      return;
    }

    if (result == AdsDummy.ShowResult.Failed)
    {
      if (onAdsNotWatchedCallback != null)
      {
        onAdsNotWatchedCallback.Invoke();
      }
      
    }

    if (result == AdsDummy.ShowResult.Skipped)
    {
      if (onAdsNotWatchedCallback != null)
      {
        onAdsNotWatchedCallback.Invoke();
      }
    }

  }

}
