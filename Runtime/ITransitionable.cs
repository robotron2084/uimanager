using System.Collections;
using UnityEngine;

namespace com.enemyhideout.ui
{
  public interface ITransitionable
  {
    Transform Transform { get; }

    IEnumerator TransitionIn();
    IEnumerator TransitionOut();
  }
}