namespace UI
{
  public class MusicBtn : ASwitchBtn
  {
    #region Protected
    //---------------------------------------------------------------------------------------------------------------
    protected override sealed void ProcessClick()
    {
       Game.Settings.MusicEnabled = !Game.Settings.MusicEnabled;
    }
    //---------------------------------------------------------------------------------------------------------------
    protected override sealed bool GetStatus => Game.Settings.MusicEnabled;
    #endregion
  }
}
