using UnityEngine;
using UnityEngine.UI;

public class Game_Canvas : MonoBehaviour
{
  private Canvas myCanvas;
  public Canvas Canvas { get { return this.myCanvas; } }
  public Canvas SetCanvas { set { this.myCanvas = value; } }
  public bool HasCanvas { get { return this.Canvas != null; } }
}
