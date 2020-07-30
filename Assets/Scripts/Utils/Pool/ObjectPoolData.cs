using System;

[Serializable]
public class ObjectPoolData
{
  public ObjectPoolName name;
  public APoolable prefab;
  public int initialCapacity;
  public RootBase Root;
  public bool HasRoot => this.Root != null;

  public ObjectPoolData()
  {
  }

  public ObjectPoolData(ObjectPoolName name, APoolable prefab, int initialCapacity, RootBase root)
  {
    this.name = name;
    this.prefab = prefab;
    this.initialCapacity = initialCapacity;
    this.Root = root;
  }
}

