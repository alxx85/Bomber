using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private AnimationCurve _fireAnimation;
    
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
    }
}
