using UnityEngine;
namespace GameBase
{
  [StaticConstructorOnStartup]
  public static class DefaultContent
  {
    public static readonly float DefaultMusicVolume = 0.45f;
    public static readonly float DefaultActionCooldown = 0.2f;
    public static readonly float PlayerActionCooldown = DefaultActionCooldown;
    public static readonly float DefaultMoveSpeed = 0.5f;
    public static readonly int DefaultTargetReward = 50;
    public static readonly int RewardAvoidSpider = 5;
    public static readonly int LevelsWithoutAds = 4;
    public static readonly float PlayerMoveSpeed = DefaultMoveSpeed;


    public static readonly string BLANK = "BLANC";
    public static readonly string blank = "blank";
    public static readonly string OBSOLETE = "OBSOLETE";
    public static readonly string ID_NOT_SET = "ID_NOT_SET";
    public static readonly string NO_LOCALIZATION_FOUND = "No localization found.";
    public static readonly string CLONE_ID_MARKER = "_clone_";
    public static readonly int WRONG_ELEMENT = -1;
    public static readonly string NO_HEX = DefaultContent.BLANK;
    public static readonly Color COLOR = Color.white;
    public static readonly Color EmptyColor = new Color(255, 255, 255, 0);
  }
}