using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Utils.ExtensionMethods;

public class Coin : APoolable
{
  private const float FlyDuration = 0.6f;
  private int CurrencyToPush;

  //---------------------------------------------------------------------------------------------------------------
  public static void Start(Vector3 destination, int CurrencyValue)
  {
    Coin coin = Game.PoolManager.Pop<Coin>(ObjectPoolName.Coin, Game.Canvas.transform);
    if (coin.IsNullOrPooled())
    {
      Debug.LogError("Unexpected Error. Can't pop coin.");
      return;
    }

    coin.CurrencyToPush = CurrencyValue;
    (coin.transform as RectTransform).MoveToWorldPosition(destination, Game.Canvas);
    (coin.transform as RectTransform).DOJump(Game.PlayRoot.CoinAcceptor.position, jumpPower: 1, numJumps: 1, duration: FlyDuration)
      .OnComplete(() => { coin.OnArrival(); } )
      .SetAutoKill();
  }

  #region Internal logic
  //---------------------------------------------------------------------------------------------------------------
  private void OnArrival()
  {
    Game.PlayRoot.Currency.Push(this.CurrencyToPush);
    this.ReturnToPool();
  }
  #endregion

  #region APoolable
  //---------------------------------------------------------------------------------------------------------------
  public override void OnReturnedToPool() { }
  #endregion
}
