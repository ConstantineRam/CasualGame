using Assets.Scripts.Utils.ExtensionMethods;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LanguageSelectItem : ExtendedBehaviour, IPointerDownHandler
{
  [SerializeField] private Text text;
  [SerializeField] private Image image;
  [SerializeField] private Sprite onSprite;
  [SerializeField] private Sprite offSprite;
  [SerializeField] private Vector2 pressedTextPos = Vector2.zero;


  public LanguageKind languageKind;

  private Signal<LanguageKind> onClick;
  private Vector2 iniTextPos;

  #region MonoBehaviour
  //---------------------------------------------------------------------------------------------------------------
  private void Awake()
  {
    iniTextPos = text.transform.localPosition.ToV2FromXY();
  }
  #endregion

  //---------------------------------------------------------------------------------------------------------------
  public void Init(LanguageKind languageKind)
  {
    gameObject.SetActive(true);
    this.languageKind = languageKind;

    text.text = this.languageKind.ToShort().ToUpper();

    onClick = new Signal<LanguageKind>();
  }

  //---------------------------------------------------------------------------------------------------------------
  public void ListenOnClick(Action<LanguageKind> action)
  {
    onClick.Listen(action);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void SetAsSelected(bool isSelected)
  {
    image.sprite = isSelected ? offSprite : onSprite;

    text.transform.LocalMoveTo(isSelected ? iniTextPos + pressedTextPos : iniTextPos);
  }

  //---------------------------------------------------------------------------------------------------------------
  public void OnPointerDown(PointerEventData eventData)
  {
    SetAsSelected(true);
    onClick.Invoke(languageKind);
  }
}
