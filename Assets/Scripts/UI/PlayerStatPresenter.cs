using UnityEngine;
using TMPro;

public class PlayerStatPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void ChangeStat(float value)
    {
        if (_text != null)
        {
            _text.text = value.ToString();
        }
    }
}
