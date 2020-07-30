using UnityEngine;

public class Game_Camera : MonoBehaviour
{
  private Camera myCam;
  public Camera Camera { get { return this.myCam; } }
  public Camera SetCamera { set { this.myCam = value; } }
  public bool HasCamera { get { return this.Camera != null; } }
}
