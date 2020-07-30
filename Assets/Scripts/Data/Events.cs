using GameBase;

// This manager is a shell for event system.
public class Events
{
  public readonly Signal GameStarted = new Signal();
  public readonly Signal GameClosed = new Signal();



  // Ingame state changes
  public readonly Signal SessionEnded = new Signal();
  public readonly Signal<Direction> Swipe = new Signal<Direction>();
  public readonly Signal StartMainMenu = new Signal();
  public readonly Signal<Direction> DirectionChanged = new Signal<Direction>();

  public readonly Signal GameWon = new Signal();
  public readonly Signal GameLost = new Signal();
  public readonly Signal<int> ReportScore = new Signal<int>();

  // Settings
  public readonly Signal<bool> MusicEnabled = new Signal<bool>();
  public readonly Signal<bool> SoundEnabled = new Signal<bool>();
  public readonly Signal<int> BestScoreChanged = new Signal<int>();
  public readonly Signal<LanguageKind> LanguageChanged = new Signal<LanguageKind>();

  // SHARING
  public readonly Signal ScreenshotTaken = new Signal();


}
