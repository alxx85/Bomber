using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _moveSpeed;

    private Vector3 distance;

    private void Start()
    {
        distance = transform.position - _player.position;
    }

    private void LateUpdate()
    {
        if (_player != null)
            transform.position = Vector3.Lerp(transform.position, _player.position + distance, _moveSpeed * Time.deltaTime);
    }
}
