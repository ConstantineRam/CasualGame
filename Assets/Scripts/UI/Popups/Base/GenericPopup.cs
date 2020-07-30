using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class GenericPopup : MonoBehaviour
{
  public bool CanBeClosedByClickOnFade = true;
  public bool HasCustomDecor = false;
  public bool DisableBlur = false;

  [HideInInspector]
  // CallBack popup is a popup that would be explicitly call after closing this popup. Use it if you need to return back to very specific popup.
  // You can do it through OnClose too, however using popup callback is a dedicated way to so and can be checked by HasCallBack property.
  public GenricPopUpCallback CallBack = null;
  public bool HasCallback => this.CallBack != null;

  [HideInInspector]
  public object CallBackData = null;

  //---------------------------------------------------------------------------------------------------------------
  public virtual void Activate(object data) { }

  //---------------------------------------------------------------------------------------------------------------
  /// <summary>
  /// The difference beteen OnCLose and CloseSelf is that OnClose has all things you want to perform when this PopUp closes, no matter did it close itself via btn or by manager due to opening other popUp.
  /// CloseSelf is to be called when PopUp is intentionally closed by user or game. So CloseSelf will be called 100% time including application stop, scene change etc, when OnClose only when it planned by User inside existing flow.
  /// </summary>
  public virtual void OnClose()
  {

  }

  //---------------------------------------------------------------------------------------------------------------
  public virtual void CloseSelf()
  {

    this.OnClose();

    if (this.CallBack != null)
    {
      this.CallBack.Invoke();
    }
    Game.UiManager.Hide();
  }










  // Check GenericPopupEditor for Editor details.
  // Creates a temporary Canvas to edit this popup in Editor.
#if UNITY_EDITOR
  public void OpenForEdit()
  {
    FakePopupsCanvas fakeCanvas;
    fakeCanvas = FindObjectOfType<FakePopupsCanvas>();
    if (fakeCanvas == null)
    {
      //var pew = Resources.FindObjectsOfTypeAll(typeof(FakePopupsCanvas)).First() as FakePopupsCanvas;
      var pew = Resources.LoadAll<FakePopupsCanvas>("").First();
      fakeCanvas = Instantiate(pew);
    }
    PrefabUtility.DisconnectPrefabInstance(fakeCanvas);
    fakeCanvas.transform.SetAsLastSibling();

    var oldObject = fakeCanvas.Container.GetComponentInChildren(GetType());
    if (oldObject != null)
    {
      EditorGUIUtility.PingObject(oldObject.gameObject);
      Selection.activeGameObject = oldObject.gameObject;
      return;
    }

    var popup = PrefabUtility.InstantiatePrefab(gameObject) as GameObject;
    popup.transform.parent = fakeCanvas.Container;
    popup.transform.localScale = Vector3.one;

    EditorGUIUtility.PingObject(popup.gameObject);
    Selection.activeGameObject = popup.gameObject;
  }
#endif

}
