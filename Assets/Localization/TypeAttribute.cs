using System;

namespace GameBase
{

  [AttributeUsage(AttributeTargets.Field)]
  public class TypeAttribute : Attribute
  {
    public Type Key { get; private set; }

    public TypeAttribute(Type key)
    {
      Key = key;
    }
  }

}