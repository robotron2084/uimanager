using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.enemyhideout.ui
{
  public class ButtonWithLabel : MonoBehaviour
  {
    public TMP_Text Label;
    public Button Button;
    
    public void SetBehaviour(ConfirmCancelDialog.ButtonData data)
    {
      Label.text = data.Label;
    }

  }
}