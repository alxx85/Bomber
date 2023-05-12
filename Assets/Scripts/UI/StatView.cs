using UnityEngine;
using TMPro;

public class StatView : MonoBehaviour
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
