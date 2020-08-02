using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
  public abstract class ASwitchBtn : BasicMono
  {
    [SerializeField]
    private SwitchingImage Icon;

    //---------------------------------------------------------------------------------------------------------------
    public void OnClick()
    {
      this.ProcessClick();
      this.PlaySound();
      this.UpdateIcon();
    }

    #region Protected
    //---------------------------------------------------------------------------------------------------------------
    protected virtual void PlaySound()
    {
      Game.AudioManager.PlaySound(AudioId.PlasticBtn26);
    }

    //---------------------------------------------------------------------------------------------------------------
    protected abstract void ProcessClick();
    //---------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns true if something is ON and intial icon sprite is used and vsa versa.
    /// </summary>
    protected abstract bool GetStatus { get; }
    //---------------------------------------------------------------------------------------------------------------
    protected override void OnStart()
    {
      base.OnStart();
      this.UpdateIcon();
    }
    #endregion
    #region Private
    //---------------------------------------------------------------------------------------------------------------
    protected void UpdateIcon()
    {
      if (this.GetStatus)
      {
        this.Icon.Switch(SwitchingImage.UsedSprite.initial);
        return;
      }

      this.Icon.Switch(SwitchingImage.UsedSprite.secondary);
    }
    #endregion
  }

}

