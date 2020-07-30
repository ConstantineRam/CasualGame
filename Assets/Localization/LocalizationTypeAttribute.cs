using System;

namespace GameBase
{

  [AttributeUsage(AttributeTargets.Field)]
public class LocalizationTypeAttribute : Attribute
  {
    public Type Key { get; private set; }

    public LocalizationTypeAttribute(Type key)
    {
      Key = key;
    }
  }

}