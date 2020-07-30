namespace GameBase
{
  public interface ILoc
  {

  }


  public enum LocType
  {
    [Type (typeof(ILocName))]
    name = 0,
    [Type(typeof(ILocDesc))]
    desc = 1,
    [Type(typeof(ILocMap))]
    map = 2
  }
}
