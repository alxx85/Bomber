using UnityEngine;

public class BombAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve _sizeAnimation;
    [SerializeField] private AnimationCurve _colorAnimation;
    [SerializeField] private Renderer[] _bombMaterial;

    private GameSettings _setting;
    private float _currentTime = 0;
    private float _currentColorTime = 0;
    private float _totalTime;

    private void Start()
    {
        _setting = GameSettings.Instance;
        _totalTime = _sizeAnimation.keys[_sizeAnimation.keys.Length - 1].time;
        _currentColorTime = -_setting.ActivateDelay + 1;
    }

    private void Update()
    {
        float currentSize = _sizeAnimation.Evaluate(_currentTime);
        transform.localScale = new Vector3(transform.localScale.x, currentSize, transform.localScale.z);
        _currentTime += Time.deltaTime;

        if (_currentTime >= _totalTime)
            _currentTime -= _totalTime;

        _currentColorTime += Time.deltaTime;

        for (int i = 0; i < _bombMaterial.Length; i++)
            _bombMaterial[i].material.color = new Color(_colorAnimation.Evaluate(_currentColorTime), 0, 0);
    }
}