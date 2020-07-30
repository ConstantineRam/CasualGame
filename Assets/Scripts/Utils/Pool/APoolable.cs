using UnityEngine;
// Don't use Awake/Start or OnEnable for this object. It will not work. Use OnPop()
public abstract class APoolable : MonoBehaviour
{
  public ObjectPool Pool  { get; set; }

  //---------------------------------------------------------------------------------------------------------------
  //public static APoolable Pop(GameObject NewParent = null)
  //{
  //  return Game.PoolManager.Pop(, NewParent);
  //}

  private bool isPooled = false;
  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Return true if this object is not in game, but inside it's pool container and disabled.
  /// </summary>
  public bool IsPooled { get { return isPooled; }}

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Called when Game.PoolManager.Pop is called for this poolable object.
  /// </summary>
  public virtual void OnPop()
  { 
   // isPooled = false; //This functionality moved to SetPopedStatus and managed my Pool.
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Called automatically when ReturnToPool is called. You don't need to run it manually, but you have to inplement any requred cleaning routines there.
  /// Don't reassign Parent of a APoolable after pooling it. Otherwise it could be missplaced and even deleted if you store pool into DontDestroyOnLoad object.
  /// </summary>
  public abstract void OnReturnedToPool();

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Reserved by Pool Manager to set status.
  /// You shouldn't use it.
  /// </summary>
  public void SetPopedStatus()
  {
    if (!this.IsPooled)
    {
      Debug.LogError("  SetPopedStatus() called for object " + this.name + ", however its already set as poped.");
    }
    isPooled = false;
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Reserved by Pool Manager to set status.
  /// You shouldn't use it.
  /// </summary>
  public void SetPooledStatus()
  {
    if (this.IsPooled)
    {
      Debug.LogError("  SetPooledStatus() called for object " + this.name + ", however its already set as pooled.");
    }
    isPooled = true;
  }
  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Works as "Destroy" for standard prefab, but instead moves poolable Object to its container.
  /// If you need to perform any actions during object return to Pool you can do it in OnReturnedToPool();
  /// You have to clean the object for future use in OnReturnedToPool OR OnPop.
  /// Don't ressign Parent of a APoolable after pooling it. Otherwise it could be misplaced and even deleted if you store pool into DontDestroyOnLoad object.
  /// </summary>
  public virtual void ReturnToPool()
  {
    OnReturnedToPool();

    if (Pool != null)
    {
      if (!this.IsPooled)
      {
        Pool.Push(this);
      }
      
    }
    else
    {
      Debug.Log("APoolable "+ this.name + " has no Pool set. Destroyed.");
      Destroy(gameObject);
    }
      

    
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Called once this object was created.
  /// NOTE: Object is created once, but could be pooled and poped many times. Monobehavior Awake/ Start will not work for poolable objects.
  /// </summary>
  public virtual void OnCreate()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Called when object destroyed by Unity. Use it if you need, for example, remove it from any lists or registers you keep it.
  /// </summary>
  private void OnDestroy()
  {
    // Mistake was made here: OnDestroy is called for an object that is destroyed by system, so once we try to return it to pool it will no longer exists. 
    // While it will hardly happens during game it will result in a lot of errors when game is stopped.
    //if (!isPooled)
    //{
    //  ReturnToPool();
    //}
      
  }
}

public static class APoolbleUtils
{
  //---------------------------------------------------------------------------------------------------------------
  public static bool IsNullOrPooled(this APoolable ap)
  {
    if (ap == null)
    {
      return true;
    }
    if (ap.IsPooled)
    {
      return true;
    }

    return false;

  }
}
