using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField] private PlayerMover _player;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Vector3 distance;

    private bool _isTracking;

    private void Start()
    {
        //distance = transform.position - _player.position;
    }

    private void LateUpdate()
    {
        if (_player != null && _isTracking)
            transform.position = Vector3.Lerp(transform.position, _player.transform.position + distance, _moveSpeed * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(transform.position, distance, _moveSpeed * Time.deltaTime);
    }

    public void InitPlayer(PlayerMover player)
    {
        _player = player;
        _isTracking = true;
    }
}
