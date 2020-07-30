using UnityEngine;

public class ObjectPoolCreator : MonoBehaviour
{
  [SerializeField]
  private APoolable prefab;
  [SerializeField]
  private ObjectPoolName name;

  [Header("Extra settings.")]
  [SerializeField]
  [Tooltip("Amount of objects to be created upon initialization. If there aren't enough, game will create more. If you plan to have many objects it's good idea to do it during initialization.")]
  private int initCapacity;
  [SerializeField]
  private RootBase root;
  private Transform container;

  private void Start()
  {

    prefab.gameObject.SetActive(false);
    this.container = this.transform;

    ObjectPoolData data = new ObjectPoolData(name, prefab, initCapacity, root);
    Game.PoolManager.AddPool( data: data, container: container);
  }
}
