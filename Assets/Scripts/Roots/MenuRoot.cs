using GameBase;
using UnityEngine;

public class MenuRoot : RootBase
{
  public readonly Signal UpdateUnlocksRequest = new Signal();
  #region Internal data

  #endregion

  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  private void Start()
  {
    Game.Swipe.SetLimits(-194, -155);
    Game.Swipe.SetInitiaAngle(-176);
    if (!Game.AudioManager.HasActiveMusic())
    {
      Game.AudioManager.PlayMusic(AudioId.Music, loop: true);
    }

    Game.MenuRoot = this;
    if (Game.Settings.IsFirstLaunch)
    {
      Game.StateManager.SetState(GameState.Play, withAnim: false);
      return;
    }


  }
  #endregion
  #region Internal logic
  //---------------------------------------------------------------------------------------------------------------
  private void StartAction()
  {
    Game.StateManager.SetState(GameState.Play, withAnim: false);
  }

  #endregion

  //---------------------------------------------------------------------------------------------------------------
  public void OnStartActionPressed()
  {
    Game.AudioManager.PlaySound(AudioId.ArcadeCLick);
    if (Game.Settings.GameProgress > DefaultContent.LevelsWithoutAds)
    {
      Game.UiManager.Open(PopupId.Ok, "You just enjoyed some cool ads.", CallBack: () => 
      { Game.AdsManager.Show(Game.MenuRoot.StartAction, Game.MenuRoot.StartAction, AdsType.VIDEO); });
      return;
    }

    this.StartAction();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OpenMenu()
  {
    Game.AudioManager.PlaySound(AudioId.ArcadeCLick);
    Game.UiManager.Open(PopupId.GameMenu);
  }


  //---------------------------------------------------------------------------------------------------------------
  public override void Unload()
  {
    Game.PlayRoot = null;
    base.Unload();
  }

}
