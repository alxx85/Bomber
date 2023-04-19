using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private AnimationCurve _fireAnimation;
    [SerializeField] private Renderer _renderer;

    private float _dissolve = -1;
    private float _currentTime = 0;

    private void Start()
    {
        Destroy(gameObject, _delay);

    }

    private void Update()
    {
        float scale = _fireAnimation.Evaluate(_currentTime);
        transform.localScale = Vector3.one * scale;
        _currentTime += Time.deltaTime;

        Material material = _renderer.material;
        _dissolve = Mathf.Lerp(_dissolve, 1f, Time.deltaTime);
        material.SetFloat("_DissolveSize", _dissolve);
    }
}
