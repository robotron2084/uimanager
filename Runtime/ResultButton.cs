using UnityEngine;
using UnityEngine.UI;

namespace com.enemyhideout.ui
{
  /// <summary>
  /// A helper class to wire up a button to set a result on a ui screen.
  /// Wires up at startup.
  /// </summary>
  public partial class ResultButton : MonoBehaviour
  {
    [SerializeField]
    private int _result;
    [SerializeField]
    private ButtonBehavior _behaviour;
    private UIScreen _screen;
    
    public void Start()
    {
      _screen = GetComponentInParent<UIScreen>();
      var button = GetComponent<Button>();
      button.onClick.AddListener(OnClick);
    }

    public void SetBehaviour(ConfirmCancelDialog.ButtonData data)
    {
      _result = data.Result;
      _behaviour = data.Behavior;
    }

    private void OnClick()
    {
      switch (_behaviour)
      {
        case ButtonBehavior.SetResult:
          _screen.SetResult(_result);
          break;
        case ButtonBehavior.SetResultAndDismiss:
          _screen.SetResultAndDismiss(_result);
          break;
        case ButtonBehavior.SetResultAndDespawn:
          _screen.SetResultAndDespawn(_result);
          break;
      }
    }
  }
}