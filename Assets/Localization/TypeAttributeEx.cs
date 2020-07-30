using System;
using System.Linq;
using System.Reflection;

namespace GameBase
{
  public static class TypeAttributeEx
  {
    public static Type GetAttributeType<T>(this T id)
    {
      MemberInfo memberInfo = typeof(T).GetMember(id.ToString()).FirstOrDefault();

      if (memberInfo != null)
      {
        TypeAttribute attribute = (TypeAttribute) memberInfo.GetCustomAttributes(typeof(TypeAttribute), false).FirstOrDefault();
        return attribute.Key;
      }

      return default;
    }

  }


}