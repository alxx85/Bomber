using System;
using UnityEngine;
using UnityEngine.UI;

public class MutePresenter : MonoBehaviour
{
    [SerializeField] private Toggle _mute;

    public event Action<bool> MuteChanged;

    private void OnEnable()
    {
        _mute.onValueChanged.AddListener(MuteChange);
    }

    private void MuteChange(bool isMute)
    {
        MuteChanged?.Invoke(isMute);
    }

    public void Init(bool isMute)
    {
        _mute.isOn = isMute;
    }

}
