using UnityEngine;

public class VolumeViewer : MonoBehaviour
{
    [SerializeField] private MutePresenter _mute;
    [SerializeField] private VolumePresenter _characterVolume;
    [SerializeField] private VolumePresenter _environmentVolume;
    [SerializeField] private VolumePresenter _musicVolume;

    private GameSettings _setting;

    private void Start()
    {
        _setting = GameSettings.Instance;
        _mute.Init(_setting.Muting);
        _characterVolume.Init(_setting.GetVolume(SoundType.Characters));
        _environmentVolume.Init(_setting.GetVolume(SoundType.Environment));
        _musicVolume.Init(_setting.GetVolume(SoundType.Music));
    }

    private void OnEnable()
    {
        _mute.MuteChanged += OnMuteChanged;
        _characterVolume.VolumeChanged += OnVolumeChanged;
        _environmentVolume.VolumeChanged += OnVolumeChanged;
        _musicVolume.VolumeChanged += OnVolumeChanged;
    }

    private void OnDisable()
    {
        _mute.MuteChanged -= OnMuteChanged;
        _characterVolume.VolumeChanged -= OnVolumeChanged;
        _environmentVolume.VolumeChanged -= OnVolumeChanged;
        _musicVolume.VolumeChanged -= OnVolumeChanged;

    }

    private void OnVolumeChanged(SoundType type, float volume)
    {
        _setting.SetVolume(type, volume);
    }

    private void OnMuteChanged(bool isActive)
    {
        _setting.SetMute(isActive);
    }
}
