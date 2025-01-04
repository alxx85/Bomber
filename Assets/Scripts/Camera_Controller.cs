using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Vector3 distance;

    private bool _isTracking;
    private Portal _portal;

    private void Start()
    {
        //distance = transform.position - _player.position;
    }

    private void LateUpdate()
    {
        if (_isTracking)
        {
            if (_player != null && _player.gameObject.activeSelf)
                transform.position = Vector3.Lerp(transform.position, _player.transform.position + distance, _moveSpeed * Time.deltaTime);
            else
                transform.position = Vector3.Lerp(transform.position, _portal.transform.position + distance, _moveSpeed * Time.deltaTime);
        }
    }

    public void InitPlayer(PlayerMovement player)
    {
        _player = player;
        _isTracking = true;
    }

    public void InitPortal(Portal portal)
    {
        _portal = portal;
        _isTracking = true;
    }
}
