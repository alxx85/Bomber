using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float _moveSpeed;

    private Vector3 distance;

    private void Start()
    {
        distance = transform.position - player.position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + distance, _moveSpeed * Time.deltaTime);
    }
}
