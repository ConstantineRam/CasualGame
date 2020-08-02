using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace UI
{
  public class SessionRewardScreen : FlexiblePopUp
  {
    [SerializeField]
    private UpdatableIntString RewardString;
    [SerializeField]
    SwitchingImage[] Stars;
    [SerializeField]
    [Tooltip ("Delay between each star is activated.")]
    private float StarDelay = 0.5f;
    private int RemainigStars = 0;
    private int CurrentStar = 0;
    private int StarReward = 0;


    //---------------------------------------------------------------------------------------------------------------
    public override sealed void Activate(object data = null)
    {
      if (!(data is SessionData sd))
      {
        Debug.LogError("SessionRewardScreen got wrong data type.");
        return;
      }
      this.RewardString.Init(sd.StartingAmount);
      this.RewardString.Push(sd.PushedAmount);
      this.StarReward = sd.RewardPerStar;
      this.RemainigStars = sd.Stars;

      if (sd.Stars > 0)
      {
        this.ActivateStar();
      }
    }
    #region Private
    //---------------------------------------------------------------------------------------------------------------
    private void ActivateStar ()
    {
      if (this.RemainigStars < 1)
      {
        return;
      }

      if (this.CurrentStar > this.Stars.Length - 1)
      {
        return;
      }

      this.RemainigStars--;
      this.Stars[this.CurrentStar].Switch();
      this.Stars[this.CurrentStar].transform.DOShakeScale (0.3f, 2);

      this.RewardString.Push(this.StarReward);


      this.CurrentStar++;
      Game.TimerManager.Start(this.StarDelay, () => { this.ActivateStar(); });
    }
    #endregion
    public struct SessionData
    {
      public int StartingAmount;
      public int PushedAmount;
      public int Stars;
      public int RewardPerStar;
    }
  }
}
