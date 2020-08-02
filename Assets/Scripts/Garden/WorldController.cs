using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
  [SerializeField]
  private GameObject[] worlds;
  //---------------------------------------------------------------------------------------------------------------
  public AWorld Activate(int WorldNum)
  {
    int usedLevel = 1;
    if (Game.Settings.GameProgress <= worlds.Length)
    {
      usedLevel = Game.Settings.GameProgress-1;
    }
    else
    {
      Game.Settings.GameProgress = 1;
      usedLevel = 0;
    }

    GameObject levelObject = Instantiate(worlds[usedLevel]);
    AWorld world = levelObject.GetComponent<AWorld>();
    if (world == null)
    {
      Debug.LogError("Error with world # " + usedLevel + ". Its not a AWorld");
      return null;
    }

    world.Activate();
    return world;
  }
}
