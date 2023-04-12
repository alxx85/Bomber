using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private bool _life;
    [SerializeField] private bool _speed;
    [SerializeField] private bool _addBombAmount;
    [SerializeField] private bool _addBombPower;
    [SerializeField] private bool _kick;
    [SerializeField] private bool _shield;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMover player))
        {
            Destroy(gameObject);
            player.PickUp(new Boost(_life, _speed, _addBombAmount, _addBombPower, _kick, _shield));
        }
    }
}
