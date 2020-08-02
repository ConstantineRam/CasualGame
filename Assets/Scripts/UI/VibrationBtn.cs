using UnityEngine;
namespace UI
{
  public class VibrationBtn : ASwitchBtn
  {
    #region Protected
    //---------------------------------------------------------------------------------------------------------------
    protected override sealed void ProcessClick()
    {
      Game.Settings.VibrationEnabled = !Game.Settings.VibrationEnabled;

      if (Game.Settings.VibrationEnabled)
      {
        Handheld.Vibrate();
      }
    }
    //---------------------------------------------------------------------------------------------------------------
    protected override sealed bool GetStatus => Game.Settings.VibrationEnabled;
    #endregion
  }
}
