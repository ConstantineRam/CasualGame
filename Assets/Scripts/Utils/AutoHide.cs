using UnityEngine;

/// <summary>
/// Hides Game Object attached to it
/// </summary>
public class AutoHide : MonoBehaviour
{
    void Awake()
    {
    this.gameObject.SetActive (false);
    }

}
