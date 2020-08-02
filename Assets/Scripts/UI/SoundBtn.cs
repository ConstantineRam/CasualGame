namespace UI
{
  public class SoundBtn : ASwitchBtn
  {
    #region Protected
    //---------------------------------------------------------------------------------------------------------------
    protected override sealed void ProcessClick()
    {
      Game.Settings.SoundEnabled = !Game.Settings.SoundEnabled;
    }
    //---------------------------------------------------------------------------------------------------------------
    protected override sealed bool GetStatus => Game.Settings.SoundEnabled;
    #endregion
  }
}
