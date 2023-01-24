using UnityEngine;

namespace com.enemyhideout.ui
{
  public interface IUIScreen: ITransitionable
  {
    IUIController Controller { get; set; }
  }
}