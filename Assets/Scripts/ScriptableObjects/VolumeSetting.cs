using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PlayerVolume", menuName = "Properties/GameVolume")]
public class VolumeSetting : ScriptableObject
{
    [SerializeField] private bool _mute = false;
    [SerializeField] private float _characters;
    [SerializeField] private float _environments;
    [SerializeField] private float _music;

    public void ChangeVolume(SoundType type, float volume)
    {
        switch (type)
        {
            case SoundType.Characters:
                _characters = volume;
                break;
            case SoundType.Environment:
                _environments = volume;
                break;
            case SoundType.Music:
                _music = volume;
                break;
        }
    }

    public void Muting(bool offSounds)
    {
        _mute = offSounds;
    }

    public float GetVolumes(SoundType type)
    {
        float volume = 0;

        switch (type)
        {
            case SoundType.Characters:
                volume = _characters;
                break;
            case SoundType.Environment:
                volume = _environments;
                break;
            case SoundType.Music:
                volume = _music;
                break;
        }

        if (_mute)
            return 0;

        return volume;
    }
}

public enum SoundType
{
    Characters,
    Environment,
    Music
}