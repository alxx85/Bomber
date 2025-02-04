using UnityEngine;
using UnityEngine.Rendering;

public class VolumeViewer : MonoBehaviour
{
    [SerializeField] private VolumeSetting _volumeSetting;
    [SerializeField] private MutePresenter _mute;
    [SerializeField] private VolumePresenter _characterVolume;
    [SerializeField] private VolumePresenter _environmentVolume;
    [SerializeField] private VolumePresenter _musicVolume;

    private GameSettings _setting;

    private void Start()
    {
        try
        { 
            _setting = GameSettings.Instance;
            _mute.Init(_setting.Muting);
            _characterVolume.Init(_setting.GetVolume(SoundType.Characters));
            _environmentVolume.Init(_setting.GetVolume(SoundType.Environment));
            _musicVolume.Init(_setting.GetVolume(SoundType.Music));
        }
        catch 
        {
            _mute.Init(_volumeSetting.Mute);
            _characterVolume.Init(_volumeSetting.GetVolumes(SoundType.Characters));
            _environmentVolume.Init(_volumeSetting.GetVolumes(SoundType.Environment));
            _musicVolume.Init(_volumeSetting.GetVolumes(SoundType.Music));
        }
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

        SaverPlayers.SaveVolumeSetting(_volumeSetting);
    }

    private void OnVolumeChanged(SoundType type, float volume)
    {
        try
        { 
            _setting.SetVolume(type, volume); 
        }
        catch 
        { 
            _volumeSetting.ChangeVolume(type, volume);
        }
    }

    private void OnMuteChanged(bool isActive)
    {
        try
        {
            _setting.SetMute(isActive);
        }
        catch
        {
            _volumeSetting.Muting(isActive);
        }
    }
}
