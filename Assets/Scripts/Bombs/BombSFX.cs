using System;
using UnityEngine;

public class BombSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _bobmSFX;
    [SerializeField][Range(0,1)] private float _maxVolume = 1f;
    [SerializeField] private SoundType _type = SoundType.Environment;

    private GameSettings _setting;

    private void OnEnable()
    {
        _setting = GameSettings.Instance;
        _setting.ChangedVolume += OnChangedVolume;
        OnChangedVolume();
    }

    private void OnDisable()
    {
        _setting.ChangedVolume -= OnChangedVolume;
    }


    private void Start()
    {
        float delay = _bobmSFX.clip.length / _bobmSFX.pitch;
        Destroy(gameObject, delay);
    }

    private void OnChangedVolume()
    {
        _bobmSFX.volume = _setting.GetVolume(_type) * _maxVolume;
    }
}
