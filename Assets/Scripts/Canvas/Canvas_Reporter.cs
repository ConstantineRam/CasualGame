using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Reporter : MonoBehaviour
{
  void Start()
  {
    Game.Game_Canvas.SetCanvas = this.GetComponentInParent<Canvas>() as Canvas;
  }

}
