using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _walkSource;
    [SerializeField] private AudioSource _setBombSource;
    [SerializeField][Range(0, 1)] private float _maxVolume = .8f;
    [SerializeField] private SoundType _soundType = SoundType.Characters;

    private GameSettings _settings;
    [SerializeField] private float _audioModifiere = 0.1f;

    private void OnEnable()
    {
        _settings = GameSettings.Instance;
        _settings.ChangedVolume += OnChangedVolume;
        _settings.ChangedPause += OnChangedPause;
        OnChangedVolume();
    }

    private void _settings_ChangedPause(bool obj)
    {
        throw new System.NotImplementedException();
    }

    private void OnDisable()
    {
        _settings.ChangedVolume -= OnChangedVolume;
        _settings.ChangedPause -= OnChangedPause;
    }

    public void Play(AudioName name)
    {
        switch (name)
        {
            case AudioName.Walk:
                _walkSource.pitch = 1 + (_settings.SpeedLevel * _audioModifiere); 
                _walkSource.Play();
                break;
            case AudioName.SetBomb:
                _setBombSource.Play(); 
                break;
        }
    }

    public void Stop()
    {
        _walkSource.Stop();
    }

    private void OnChangedVolume()
    {
        _walkSource.volume = _settings.GetVolume(_soundType) * _maxVolume;
        _setBombSource.volume = _settings.GetVolume(_soundType) * _maxVolume;
    }

    private void OnChangedPause(bool pause)
    {
        if (pause)
        {
            _walkSource.Pause();
            _setBombSource.Pause();
        }
        else
        {
            _walkSource.UnPause();
            _setBombSource.UnPause();
        }
    }
}

public enum AudioName
{
    Walk,
    SetBomb
}
