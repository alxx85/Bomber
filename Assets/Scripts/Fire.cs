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
        Material material = _renderer.material;
        float scale = _fireAnimation.Evaluate(_currentTime);
        transform.localScale = Vector3.one * scale;
        _currentTime += Time.deltaTime;
        _dissolve = Mathf.Lerp(_dissolve, 1f, _delay * Time.deltaTime);
        //_dissolve = Mathf.Clamp(_dissolve, -1f, 1f);
        material.SetFloat("_DissolveSize", _dissolve);
    }
}
