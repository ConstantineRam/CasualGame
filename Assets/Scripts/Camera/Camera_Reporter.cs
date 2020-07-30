using UnityEngine;

public class Camera_Reporter : MonoBehaviour
{
    void Start()
    {
    Game.Game_Camera.SetCamera = this.GetComponentInParent<Camera>() as Camera;
    }

}
