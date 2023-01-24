using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.ui
{
  public class ScreenManager : MonoBehaviour, IUIController
  {
    [SerializeField]
    private Transform _root;
    private List<IUIScreen> screens = new List<IUIScreen>();
    private List<IUIScreen> screensToShow = new List<IUIScreen>();
    private List<IUIScreen> screensToHide = new List<IUIScreen>();
    private bool _dirty = false;

    void Update()
    {
      if (_dirty)
      {
        _dirty = false;
        foreach (var uiScreen in screensToShow)
        {
          if (screens.Contains(uiScreen))
          {
            throw new Exception("Screen already shown!");
          }
          screens.Add(uiScreen);
          var pos = Vector3.zero;
          uiScreen.Transform.localPosition = pos;
          StartCoroutine(uiScreen.TransitionIn());

        }
        foreach (var uiScreen in screensToHide)
        {
          screens.Remove(uiScreen);
          StartCoroutine(uiScreen.TransitionOut());
        }

        screensToShow.Clear();
        screensToHide.Clear();
      }
    }

    private void MarkDirty()
    {
      _dirty = true;
    }
    
    public void ShowScreen(IUIScreen screen)
    {
      screen.Controller = this;
      screen.Transform.SetParent(_root, false);
      screensToShow.Add(screen);
      MarkDirty();
    }

    public T ShowPrefab<T>(T screenPrefab) where T : UIScreen
    {
      var screen = Instantiate(screenPrefab);
      ShowScreen(screen);
      return screen;
    }

    public void HideScreen(IUIScreen screen)
    {
      screensToHide.Add(screen);
      MarkDirty();
    }

    public IEnumerator WaitUntilActive(GameObject active)
    {
      yield return new WaitUntil(() => active.activeSelf);
    }

  }
}