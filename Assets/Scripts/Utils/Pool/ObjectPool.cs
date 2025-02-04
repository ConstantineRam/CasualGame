﻿using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
  public ObjectPoolData data;
  public GameObject parent;

  private Stack<APoolable> stack;

  private List<APoolable> itemsInUse;

  //---------------------------------------------------------------------------------------------------------------
  public ObjectPool(ObjectPoolData data, GameObject parent)
  {
    this.data = data;
    this.parent = parent;

    stack = new Stack<APoolable>(data.initialCapacity);
    itemsInUse = new List<APoolable>();

    for (int i = 0; i < data.initialCapacity; i++)
    {
      AddInstance();
    }
  }

  #region Push item

  //---------------------------------------------------------------------------------------------------------------
  public virtual void Push(APoolable item)
  {
#if DEBUG
    if (item == null || item.gameObject == null) return;

    if (stack.Contains(item))
    {
      Debug.LogError("Tried to pool already pooled object. Ignoring...Check for duplicate return to pool " + data.name);
      return;
    }
    if (!item.gameObject.activeSelf)
    {
      Debug.LogError("Tried to pool inactive object. Ignoring...Check for duplicate return to pool " + data.name);
     return;
    }

    if (itemsInUse.Count < 1)
    {
      Debug.LogError("Tried to pool object while pool had no items in use. Pool: " + data.name);
      return;
    }
#endif

    item.SetPooledStatus();
    item.gameObject.SetActive(false);
    stack.Push(item);
    itemsInUse.Remove(item);


    item.transform.SetParent(parent.transform);
  }

  #endregion

  #region Pop item

  //---------------------------------------------------------------------------------------------------------------
  public virtual APoolable Pop()
  {
    if (stack.Count == 0)
    {
      AddInstance();
    }
      

    APoolable item = stack.Pop();
    item.SetPopedStatus();
    item.OnPop();

    itemsInUse.Add(item);
    return item;
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Called by PoolManager if this pool is not attached to any root and will became Don't destroy on load.
  /// However, when scene was reloaded we need to clear the list of all objects that were not returned to pool,
  /// </summary>
  public void ClearItemsInUse()
  {
    itemsInUse.Clear();
  }

  #endregion

  //---------------------------------------------------------------------------------------------------------------
  void AddInstance()
  {
    APoolable item = (APoolable) MonoBehaviour.Instantiate(data.prefab, parent.transform);
    item.OnCreate();
    item.gameObject.SetActive(false);
    item.SetPooledStatus(); // Here was a bug: AddInstance and Pop do the same thing with semi-duplicated code, so AddInstance didn't set Pooled status to new Instances. TODO: Unify duplicated code.
    item.Pool = this;
    stack.Push(item);

#if UNITY_EDITOR
    item.name = data.prefab.name + stack.Count;
#endif
  }
}
