using System.Collections;
using GameJamStarterKit.FXSystem;
using UnityEngine;

namespace com.enemyhideout.ui
{
  [RequireComponent(typeof(Animator))]
  [RequireComponent(typeof(Signaller))]
  public class ClickShield : MonoBehaviour, ITransitionable
  {
    private Animator _animator;
    private Signaller _signaller;

    void Awake()
    {
      _animator = GetComponent<Animator>();
      _signaller = GetComponent<Signaller>();
    }

    public Transform Transform
    {
      get
      {
        return transform;
      }
    }

    public bool Visible
    {
      get
      {
        return _animator.GetBool("Visible");
      }
    }

    public IEnumerator TransitionIn()
    {
      gameObject.SetActive(true);
      if (!Visible)
      {
        _animator.SetBool("Visible", true);
        yield return _signaller.WaitForSignal(Signaller.Complete, "TransitionInWait");
      }
    }

    public IEnumerator TransitionOut()
    {
      if (Visible)
      {
        _animator.SetBool("Visible", false);
        yield return _signaller.WaitForSignal(Signaller.Complete, "TransitionInWait");
        gameObject.SetActive(true);
      }

    }
  }
}