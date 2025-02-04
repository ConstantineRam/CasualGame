﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootBase : MonoBehaviour
{
  private Signal onRootUnloaded;

  private GameObject[] roots;

  //---------------------------------------------------------------------------------------------------------------
  protected void Awake()
  {
    onRootUnloaded = new Signal();

    roots = SceneManager.GetActiveScene().GetRootGameObjects();
    this.OnAwake();
  }
  //---------------------------------------------------------------------------------------------------------------
  protected virtual void OnAwake() { }
  //---------------------------------------------------------------------------------------------------------------
  private void Activate()
  {
    if (GetType() == typeof(RootBase))
    {
      gameObject.SetActive(true);
    }
    if (roots != null)
      foreach (var root in roots)
        root.SetActive(true);
  }
  //---------------------------------------------------------------------------------------------------------------
  private void Deactivate()
  {
    if (GetType() == typeof(RootBase))
    {
      gameObject.SetActive(false);
    }
    if (roots != null)
      foreach (var root in roots)
      root.SetActive(false);
  }
  //---------------------------------------------------------------------------------------------------------------
  public void Load()
  {
    Activate();
  }

  //---------------------------------------------------------------------------------------------------------------
  public virtual void Unload()
  {
    //Deactivate(); It's for the purpose to have more than one root in a scene

    onRootUnloaded.Invoke();

   // SceneManager.UnloadSceneAsync(gameObject.scene); no longer supported by Unity
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ListenRootUnloaded(Action action)
  {
    onRootUnloaded.Listen(action);
  }
}

//---------------------------------------------------------------------------------------------------------------
public static class RootUtils
{
  public static bool IsNull(this RootBase rb)
  {
    if (rb == null)
    {
      return false;
    }

    return true;
  }
}
