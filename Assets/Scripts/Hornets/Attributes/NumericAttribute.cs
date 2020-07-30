using System;

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
public class NumericAttribute : Attribute
{
  public int Key { get; private set; }

  public NumericAttribute(int key)
  {
    Key = key;
  }
}
