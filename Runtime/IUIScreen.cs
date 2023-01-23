using UnityEngine;

namespace com.enemyhideout.ui
{
  public interface IUIScreen: ITransitionable
  {
    UIManager Manager { get; set; }
  }
}