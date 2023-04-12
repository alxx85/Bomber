using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _stoneBlock;
    [SerializeField] private GameObject _brickBlock;
    [SerializeField] private GameObject _floorBlock;
    [SerializeField] private Transform _worldZeroPoint;
    [SerializeField] private Character _enemy;

    private GameSettings _settings;
    private List<Vector3Int> _blockeds = new List<Vector3Int>();
    private List<Vector3Int> _clearBlocks = new List<Vector3Int>();
    private int _xFieldSize;
    private int _zFieldSize;
    private int _brickBlockAmount;
    private int _enemyAmount;
    private int[,] _world;

    public void InitWorldSetting(GameSettings settings, LevelSetting levelSetting)
    {
        _settings = settings;
        _xFieldSize = levelSetting.Width;
        _zFieldSize = levelSetting.Height;
        _brickBlockAmount = levelSetting.BrickBlockAmount;
        _enemyAmount = levelSetting.EnemyAmount;
        _world = new int[_xFieldSize, _zFieldSize];
        //_blockeds = new List<Vector3Int>();
        //_clearBlocks = new List<Vector3Int>();

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

        if (_brickBlockAmount + _enemyAmount <= _clearBlocks.Count())
        {
            for (int i = 0; i < _brickBlockAmount; i++)
            {
                int position = random.Next(_clearBlocks.Count());
                Vector3Int SetPosition = _clearBlocks[position];
                _clearBlocks.Remove(SetPosition);
                _world[SetPosition.x, SetPosition.z] = 2;
            }

            for (int i = 0; i < _enemyAmount; i++)
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
                else if ((x % 2 == 0 && z % 2 == 0))
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
                {
                    Character enemy = Instantiate(_enemy, _worldZeroPoint.position + new Vector3(x, 0f, z), Quaternion.identity);
                    _settings.AddEnemyOnList(enemy);
                }
            }
        }
    }
}
