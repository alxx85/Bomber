using UnityEngine;

public class BombSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _bobmSFX;

    private void Start()
    {
        float delay = _bobmSFX.clip.length / _bobmSFX.pitch;
        Destroy(gameObject, delay);
    }
}
