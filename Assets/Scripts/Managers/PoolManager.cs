using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
  private List<ObjectPoolData> poolsInfo;
  private List<ObjectPool> pools;

  //---------------------------------------------------------------------------------------------------------------
  private void Awake()
  {
    poolsInfo = new List<ObjectPoolData>();
    pools = new List<ObjectPool>();

    foreach (ObjectPoolData data in poolsInfo)
    {
      AddPool(data, null);
    }
  }
  //---------------------------------------------------------------------------------------------------------------
  private void Start()
  {
    Game.StateManager.OnStateChanged.Listen(this.ResetRootlessPool);
  }

  //---------------------------------------------------------------------------------------------------------------
  public ObjectPool GetPool(ObjectPoolName name)
  {
    return pools.Find(p => p.data.name == name);
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// Works as "Instantiate" for standard prefab, but instead moves poolable Object from the container or creates new if there is no more poolable objects left.
  /// You need to clean the object for future use in OnReturnedToPool OR OnPop.
  /// </summary>
  public APoolable Pop(ObjectPoolName name, Transform NewParent)
  {
    APoolable result = this.Pop(name);
    result.gameObject.SetActive(true);
    result.transform.SetParent(NewParent);

    return result;

  }
  /// <summary>
  /// Does NOT set object active.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  //---------------------------------------------------------------------------------------------------------------
  public APoolable Pop(ObjectPoolName name)
  {
    return GetPool(name).Pop();
  }

  //---------------------------------------------------------------------------------------------------------------
  public T Pop<T>(ObjectPoolName name) where T : APoolable
  {
    return (T) GetPool(name).Pop();
  }

  //---------------------------------------------------------------------------------------------------------------
  public T Pop<T>(ObjectPoolName name, Transform NewParent, Vector3? LocalPosition = null) where T : APoolable
  {
    T result = this.Pop<T>(name);
    result.gameObject.SetActive(true);
    result.transform.SetParent(NewParent);
    if (LocalPosition != null)
    {
      result.transform.localPosition = (Vector3) LocalPosition;
    }
    return result;
  }
  //---------------------------------------------------------------------------------------------------------------
  public void AddPool(ObjectPoolData data, Transform container)
  {
    if (GetPool(data.name) != null)
    {
      Debug.LogError("PoolManager => pool " + data.name + " already exists. Removing old instance.");
      this.DestroyPool(data.name);
     // return; here was a bug, we destroy old, but not allow new to proceed. However, it still shouldn't happen.
    }

    if (container == null)
    {
      Debug.LogError("PoolManager => no container for pool " + data.name + ".");
      return;
    }

    if (data.HasRoot)
    {
      data.Root.ListenRootUnloaded(() => DestroyPool(data.name));

    }
      

    GameObject parent;
    if (container != null)
    {
      parent = container.gameObject;
    }
    else
    {
      parent = new GameObject(data.name + "Pool");
      parent.transform.SetParent(this.transform);
    }

    pools.Add(new ObjectPool(data, parent));
  }

  //---------------------------------------------------------------------------------------------------------------
  private void DestroyPool(ObjectPoolName name)
  {
    pools.FindAll(p => p.data.name == name).ForEach(p => Destroy(p.parent));
    pools.RemoveAll(p => p.data.name == name);
  }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// If Pool has no root assigned it wouldn't be destroyed, it means it will keep track of all items 
  /// that were poped and destroyed with scene. We need to clean them on root unload.
  /// </summary>
  private void ResetRootlessPool(GameState gs)
  {
    pools.FindAll(p => !p.data.HasRoot).ForEach(p => p.ClearItemsInUse());
  }
}
