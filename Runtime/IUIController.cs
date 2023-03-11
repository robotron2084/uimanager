using System.Collections;
using UnityEngine;

namespace com.enemyhideout.ui
{
  public interface IUIController
  {
    void ShowScreen(IUIScreen screen);
    T ShowPrefab<T>(T screenPrefab) where T : UIScreen;
    void HideScreen(IUIScreen uiScreen);
    IEnumerator WaitUntilActive(GameObject gameObject);
  }
}