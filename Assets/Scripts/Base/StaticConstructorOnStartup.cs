using System;
namespace GameBase
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public class StaticConstructorOnStartup : Attribute
  {
  }
}
