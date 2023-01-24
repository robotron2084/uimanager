using System.Collections;
using UnityEngine;

namespace com.enemyhideout.ui
{
  public interface IUIController
  {
    void HideScreen(IUIScreen uiScreen);
    IEnumerator WaitUntilActive(GameObject gameObject);
  }
}