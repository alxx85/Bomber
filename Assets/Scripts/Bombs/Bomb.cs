using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private const float FireDelay = .55f;

    [SerializeField] private LayerMask _blockedMask;
    [SerializeField] private Collider _collider;
    [SerializeField] private Fire _fireTemplate;

    private GameSettings _setting;
    private Renderer _model;
    private List<Vector3> _possibleDirections = new List<Vector3> { Vector3.left, Vector3.forward, Vector3.back, Vector3.right };
    private WaitForSeconds _delay = new WaitForSeconds(0.25f);
    private Vector3 _correctionUp = new Vector3(0f, 0.5f, 0f);

    public event Action<Bomb> Exploded;

    //public int BombIndex { get; private set; }

    private void Awake()
    {
        _model = GetComponent<Renderer>();
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Explode));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Fire fire))
        {
            CancelInvoke(nameof(Explode));
            Explode();
        }
    }

    public void Init()
    {
        _setting = GameSettings.Instance;
        
        if (!_setting.CanKick)
            Invoke(nameof(Explode), _setting.ActivateDelay);
    }

    public void Activate()
    {
        Explode();
    }

    private void Explode()
    {
        Instantiate(_fireTemplate, transform.position, _fireTemplate.transform.rotation);

        foreach (var direction in _possibleDirections)
        {
            StartCoroutine(CreatExplosion(direction));
        }
        GetComponent<Collider>().enabled = false;
        _model.enabled = false;
        _collider.enabled = false;
        Destroy(gameObject, .1f);
        Exploded?.Invoke(this);
    }

    private IEnumerator CreatExplosion(Vector3 direction)
    {
        RaycastHit hit;

        for (int i = 1; i < _setting.Power + 1; i++)
        {
            Physics.Raycast(transform.position + _correctionUp, direction, out hit, i, _blockedMask);

            if (hit.collider == null)
            {
                Fire fire = Instantiate(_fireTemplate, transform.position + direction * i, _fireTemplate.transform.rotation);
                fire.Init(direction);
            }
            else
            {
                if (hit.collider.TryGetComponent(out Destroyable destroy))
                {
                    Fire fire = Instantiate(_fireTemplate, transform.position + direction * i, _fireTemplate.transform.rotation);
                    fire.Init(direction);
                }
                break;
            }
        }
        yield return _delay;
    }
}
