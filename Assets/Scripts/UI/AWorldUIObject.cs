using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils.ExtensionMethods;
namespace UI
{
  // this object is Unity.UI element that pops at world position
  public abstract class AWorldUIObject : APoolable
  {
    //---------------------------------------------------------------------------------------------------------------
    public static AWorldUIObject Instance(ObjectPoolName PoolName, Vector3 destination)
    {
      if (PoolName == ObjectPoolName.ERROR)
      {
        return null;
      }
      AWorldUIObject UIObj = Game.PoolManager.Pop<AWorldUIObject>(PoolName, Game.Canvas.transform);

      if (UIObj.IsNullOrPooled())
      {
        Debug.LogError("Unexpected Error. Can't pop UI obj.");
        return null;
      }
      if (UIObj.transform is RectTransform rt)
      {
        rt.MoveToWorldPosition(destination, Game.Canvas);
      }
      else
      {
        UIObj.transform.MoveTo(destination);
        Debug.LogError("Unexpected Error. Poped object is not Unity UI. Cant.");
      }
   
      return UIObj;
    }
  }
}