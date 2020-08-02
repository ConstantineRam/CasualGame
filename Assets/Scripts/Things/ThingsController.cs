using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GameBase;
using UnityEngine;

namespace Things
{
  public class ThingsController : MonoBehaviour
  {
    #region Internal data
    protected readonly List<AThing> Things = new List<AThing>();
    private readonly ConcurrentDictionary<SingleModel, int> models = new ConcurrentDictionary<SingleModel, int>();
    #endregion

    #region MonoBehaviour
    //---------------------------------------------------------------------------------------------------------------
    void Start()
    {
      this.OnStart();
    }
    #endregion

    #region Protected logic
    //---------------------------------------------------------------------------------------------------------------
    protected virtual void OnStart()
    {

    }
    #endregion

    #region Private logic
    //---------------------------------------------------------------------------------------------------------------
    public void ProcessChildrenThings()
    {
      List<AThing> foundThings = Game.PlayRoot.World.Things.GetComponentsInChildren<AThing>().ToList();
#if UNITY_EDITOR
      Debug.Log(" Found " + foundThings.Count() + " things in the world.");
#endif
      this.Things.AddRange(foundThings.FindAll(t => t.gameObject.activeSelf).ToList());
      

      if (this.Things.Count < 1)
      {
        Debug.LogError("No Things were found. Consider removing this script if it's not needed.");
      }

#if UNITY_EDITOR
      Debug.Log(" Things found: " + this.Things.Count + ".");
      this.Things.ForEach(thing => { Debug.Log(thing.name); });
#endif
        this.Things.ForEach(thing =>
      {
        this.models.AddOrUpdate(thing.CurrentModel, 1, (key, oldValue) => oldValue + 1); }
      );


    }
    #endregion

    //---------------------------------------------------------------------------------------------------------------
    public List<SingleModel> GetTasks(int amount)
    {
      List<SingleModel> result = new List<SingleModel>();

      amount = Mathf.Min(amount, this.Things.Count);
      for (int i = 0; i < amount; i++)
      {
        result.Add(this.Things.ElementAt(i).CurrentModel );
      }

      return result;
    }
  }
}
