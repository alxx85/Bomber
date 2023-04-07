using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _stoneBlock;
    [SerializeField] private GameObject _brickBlock;
    [SerializeField] private GameObject _floorBlock;
    [SerializeField] private int _brickBlockValue;
    [SerializeField] private Transform _worldZeroPoint;
    //[SerializeField] private EnemyMovement _enemy1;
    [SerializeField] private EnemyMover _enemy2;
    [SerializeField] private int _enemyValue;

    //private Settings _settings;
    private List<Vector3Int> _blockeds;
    private List<Vector3Int> _clearBlocks;
    [SerializeField] private int _xFieldSize;
    [SerializeField] private int _zFieldSize;
    private int[,] _world;

    private void Awake()
    {
        //_settings = GetComponent<Settings>();
        //_xFieldSize = _settings.GetFloorSizeX();
        //_zFieldSize = _settings.GetFloorSizeZ();
        _blockeds = new List<Vector3Int>();
        _clearBlocks = new List<Vector3Int>();
        _world = new int[_xFieldSize, _zFieldSize];
    }

    private void Start()
    {
        CreateWorldPlace();
        CreateBricksBlock();

        ViewCreatedWorld();
    }

    private void CreateBricksBlock()
    {
        System.Random random = new System.Random();
        
        for (int x = 0; x < _xFieldSize; x++)
        {
            for (int z = 0; z < _zFieldSize; z++)
            {
                if (!_blockeds.Contains(new Vector3Int(x, 0, z)))
                {
                    if ((x == 1 & z == 1) || (x == 1 & z == 2) || (x == 2 & z == 1))
                        continue;

                    _clearBlocks.Add(new Vector3Int(x, 0, z));
                }
            }
        }

        if (_brickBlockValue + _enemyValue <= _clearBlocks.Count())
        {
            for (int i = 0; i < _brickBlockValue; i++)
            {
                int position = random.Next(_clearBlocks.Count());
                Vector3Int SetPosition = _clearBlocks[position];
                _clearBlocks.Remove(SetPosition);
                _world[SetPosition.x, SetPosition.z] = 2;
            }

            for (int i = 0; i < _enemyValue; i++)
            {
                int position = random.Next(_clearBlocks.Count());
                Vector3Int SetPosition = _clearBlocks[position];
                _clearBlocks.Remove(SetPosition);
                _world[SetPosition.x, SetPosition.z] = 3;
            }
        }
    }

    private void CreateWorldPlace()
    {
        for (int x = 0; x < _xFieldSize; x++)
        {
            for (int z = 0; z < _zFieldSize; z++)
            {
                if (x == 0 || x == _xFieldSize - 1 || z == 0 || z == _zFieldSize - 1)
                {
                    _world[x, z] = 1;
                    _blockeds.Add(new Vector3Int(x, 0, z));
                }

                if ((x % 2 == 0 && z % 2 == 0))
                {
                    _world[x, z] = 1;
                    _blockeds.Add(new Vector3Int(x, 0, z));
                }
            }
        }

    }

    private void ViewCreatedWorld()
    {
        for (int x = 0; x < _xFieldSize; x++)
        {
            for (int z = 0; z < _zFieldSize; z++)
            {
                Instantiate(_floorBlock, _worldZeroPoint.position + new Vector3(x, -0.05f, z), Quaternion.identity, _worldZeroPoint);

                if (_world[x, z] == 1)
                    Instantiate(_stoneBlock, _worldZeroPoint.position + new Vector3(x, 0.5f, z), Quaternion.identity, _worldZeroPoint);

                if (_world[x, z] == 2)
                    Instantiate(_brickBlock, _worldZeroPoint.position + new Vector3(x, 0.5f, z), Quaternion.identity, _worldZeroPoint);

                if (_world[x, z] == 3)
                    Instantiate(_enemy2, _worldZeroPoint.position + new Vector3(x, 0f, z), Quaternion.identity);

            }
        }
    }
}
