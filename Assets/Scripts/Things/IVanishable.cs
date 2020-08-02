using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Things
{
  public interface IVanishable
  {
    void Vanish();
    bool IsVanished { get; }
  }
}
