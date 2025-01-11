using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _walkSource;
    [SerializeField] private AudioSource _setBombSource;

    public void Play(AudioName name)
    {
        switch (name)
        {
            case AudioName.Walk:
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
}

public enum AudioName
{
    Walk,
    SetBomb
}
