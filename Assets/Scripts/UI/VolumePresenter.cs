using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumePresenter : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private SoundType _type;

    public event Action<SoundType, float> VolumeChanged;

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(ChangeValue);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(ChangeValue);
    }

    public void Init(float volume)
    {
        _slider.value = volume;
    }

    private void ChangeValue(float value)
    {
        VolumeChanged?.Invoke(_type, value);
    }
}
