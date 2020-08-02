using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GameBase;
using UnityEngine;
using Extensions;

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
      this.Things.AddRange(foundThings.FindAll(t => t.gameObject.activeSelf && t.IsSpawned).ToList());
      

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
    public bool SetColliderState
    {
      set
      {
        foreach (AThing thing in this.Things)
        {
          thing.SetColliderState = value;
        }
      }
      
    }
    //---------------------------------------------------------------------------------------------------------------
    public List<SingleModel> GetTasks(int amount)
    {
      List<SingleModel> result = new List<SingleModel>();

      if (this.Things.Count() < amount)
      {
        Debug.LogError("Unexpected error. Things Controller got request for "+ amount + " tasks, but it has only "+this.Things.Count() +" things.");
      }
      amount = Mathf.Min(amount, this.Things.Count);

      
      for (int i = 0; i < amount; i++)
      {
        AThing found = null;
        int security = 0;
        while (true)
        {
          security++;
          found = this.Things.GetRandom();
          if (!result.Contains(found.CurrentModel))
          {
            break;
          }

          if (!LoopSecurity.IsOkay(ref security))
          {
            break;
          }
        }

        result.Add(found.CurrentModel);
      }

      return result;
    }
  }
}
