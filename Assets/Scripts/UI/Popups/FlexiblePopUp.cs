using System;
using System.Collections.Generic;
using UnityEngine;

// It's a foundation for less bulky popups comparing to these, provided by UI manager.
public class FlexiblePopUp : MonoBehaviour
{
  #region Internal data
  [SerializeField]
  private Collider2D Collider;
  private bool TimerLock = false;
  private TimerManager.Timer LockTimer;
  /// <summary>
  /// Amount of time popup will no accept clicks. 
  /// In case of player already touching screen it will prevent msg to appear and immedeatly disapper.
  /// </summary>
  public float LockTime { get; internal set; }
  public const float DefaultLockTime = 0.5f;
  public bool CloseOnUnlockFlag { get; internal set; } = false;
  public float RemainingLockTime => this.LockTimer != null ? this.LockTimer.GetTimeLeft() : 0;

  public FlexiblePopUpCallback Callback { get; internal set; }
  #endregion
  //---------------------------------------------------------------------------------------------------------------
  public static FlexiblePopUp Instantiate(string prefabName, Transform parent = null, string path = "", object data = null, float lockTime = DefaultLockTime)
  {

    if (path == "")
    {
      path = "Prefabs/";
    }

    GameObject prefab = Resources.Load(path + prefabName) as GameObject;
    if (prefab == null)
    {
      Debug.Log("Attempt to Instantiate popup with path " + path + prefabName + ", but Resources.Load returned null.");
      return null;
    }

    if (parent == null)
    {
      parent = Game.Canvas.transform;
    }

    
    FlexiblePopUp result = GameObject.Instantiate(prefab, parent).GetComponent<FlexiblePopUp>();
    if (!result.IsNull())
    {
      if (result.Collider == null)
      {
        result.Collider = result.GetComponent<Collider2D>();
      }
    }
    result.LockTime = lockTime;
    result.TimerLock = true;
    result.LockTimer = Game.TimerManager.Start(result.LockTime, callback: () => { result.UnlockSelf(); });

    if (data != null)
    {
      result.Activate(data);
    }
    
    return result;


  }
  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Closess popup without invoking Callback
  /// </summary>
  public void Kill()
  {
    this.Callback = null;
    this.CloseSelf();
  }
  //---------------------------------------------------------------------------------------------------------------
  public virtual void Activate(object data = null)
  {
    if (data != null)
    {
      Debug.LogError("Data was passed to popup " + this.name + ", but it has no logic to process it.");
    }
  }
  //---------------------------------------------------------------------------------------------------------------
  public void CloseSelf()
  {

    if (this.LockTimer != null)
    {
      this.LockTimer.Halt();
    }

    if (this.Callback != null)
    {
      this.Callback.Invoke();
    }

    Destroy(this.gameObject);
  }
#region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  void Update()
  {
    this.OnUpdate();
  }
  #endregion

  #region Internal logic
  //---------------------------------------------------------------------------------------------------------------
  private void UnlockSelf()
  {
    this.TimerLock = false;
    if (this.CloseOnUnlockFlag)
    {
      this.CloseSelf();
    }
  }
  //---------------------------------------------------------------------------------------------------------------
    private void OnUpdate()
  {
#if UNITY_EDITOR || UNITY_STANDALONE
    if (!Input.GetMouseButton(0))
    {
     // this.WaitForTouchUp = false;
      return;
    }
#else
    if (Input.touchCount == 0)
    {
    //  this.WaitForTouchUp = false;
      return;
    }
#endif

    if (this.TimerLock)
    {
      return;
    }

    this.CloseSelf();
  }
#endregion
}

public delegate void FlexiblePopUpCallback();

public static class FlexiblePopUpUtils
{
  //---------------------------------------------------------------------------------------------------------------
  public static bool IsNull(this FlexiblePopUp t) 
  {
    if (t == null)
    {
      return true;
    }

    return false;
  }

  //---------------------------------------------------------------------------------------------------------------
  public static T OnClose<T>(this T t, FlexiblePopUpCallback action) where T : FlexiblePopUp
  {
    if (t.IsNull())
    {
      return t;
    }
    t.Callback = action;
    return t;
  }

  //---------------------------------------------------------------------------------------------------------------
  public static T CloseOnUnlock<T>(this T t) where T : FlexiblePopUp
  {
    if (t.IsNull())
    {
      return t;
    }
    t.CloseOnUnlockFlag = true;
    return t;
  }
}