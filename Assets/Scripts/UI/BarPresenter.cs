using UnityEngine;
using UnityEngine.UI;

public class BarPresenter : MonoBehaviour
{
    [SerializeField] private Scrollbar _bar;

    private bool _activate = false;

    private void Start()
    {
        _bar.interactable = _activate;
    }

    public void Show(float value)
    {
        _bar.size = value;
    }
}
