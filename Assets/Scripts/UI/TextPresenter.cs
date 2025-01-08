using TMPro;
using UnityEngine;

public class TextPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;

    public void Show(string text)
    {
        _textField.text = text;
    }
}
