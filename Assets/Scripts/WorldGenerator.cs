using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private const int StartEnemyIndexes = 10;

    [SerializeField] private GameObject _cobbleBlock;
    [SerializeField] private GameObject _floorBlock;
    [SerializeField] private BoostViewer _boostViewer;
    [SerializeField] private Transform _worldZeroPoint;
    [SerializeField] private Portal _portal;
    [SerializeField] private List<LevelEnemy> _levelEnemys;

    private GameSettings _settings;
    private List<Vector3Int> _blockeds = new List<Vector3Int>();
    private List<Vector3Int> _clearBlocks = new List<Vector3Int>();
    private List<LevelBoost> _levelBoosters;
    private GameObject _stoneBlock;
    private GameObject _brickBlock;
    private int _xFieldSize;
    private int _zFieldSize;
    private int _brickBlockAmount;
    private int _enemyAmount;
    private int[,] _world;
    private System.Random _random = new System.Random();

    private void Start()
    {
        _settings = GameSettings.Instance;
        LevelSetting levelSetting = _settings.GetCurrentLevel();
        InitLevelSetting(levelSetting);
    }

    public void InitLevelSetting(LevelSetting levelSetting)
    {
        _xFieldSize = levelSetting.Width;
        _zFieldSize = levelSetting.Height;
        _brickBlockAmount = levelSetting.BrickBlockAmount;
        _brickBlock = levelSetting.BrickBlock;
        _stoneBlock = levelSetting.StoneBlock;
        _levelEnemys = levelSetting.Enemys;
        _levelBoosters = levelSetting.LevelBoost.ToList();
        _world = new int[_xFieldSize, _zFieldSize];

        CreateWorldPlace();
        CreateBricksBlockAndBoosts();
        CreateEnemyPoints();
        ViewCreatedWorld();
    }

    private void CreateBricksBlockAndBoosts()
    {
        if (_brickBlockAmount + _enemyAmount <= _clearBlocks.Count())
        {
            int startHideIndex = 0;
            int maxHideIndex = 0;
            int allBoosterAmount = 0;
            int j = 0;


            foreach (var booster in _levelBoosters)
                allBoosterAmount += booster.Amount;

            if (allBoosterAmount > 0)
            {
                int[] hideBlockIndex = new int[allBoosterAmount];
                maxHideIndex = _brickBlockAmount / allBoosterAmount;

                for (int i = 0; i < hideBlockIndex.Length; i++)
                {
                    hideBlockIndex[i] = UnityEngine.Random.Range(startHideIndex, i * maxHideIndex + maxHideIndex);
                    startHideIndex = hideBlockIndex[i] + 1;
                }

                for (int i = 0; i < _brickBlockAmount; i++)
                {
                    int position = _random.Next(_clearBlocks.Count());
                    Vector3Int SetPosition = _clearBlocks[position];
                    _clearBlocks.Remove(SetPosition);

                    if (i == hideBlockIndex[j])
                    {
                        _world[SetPosition.x, SetPosition.z] = 4;

                        if (j + 1 < hideBlockIndex.Length)
                            j++;
                    }
                    else
                    {
                        _world[SetPosition.x, SetPosition.z] = 3;
                    }
                }
            }
        }
    }

    private void CreateEnemyPoints()
    {
        for( int j = 0; j < _levelEnemys.Count; j++)
        {
            for (int i = 0; i < _levelEnemys[j].Amount; i++)
            {
                int position = _random.Next(_clearBlocks.Count());
                Vector3Int SetPosition = _clearBlocks[position];
                _clearBlocks.Remove(SetPosition);
                _world[SetPosition.x, SetPosition.z] = StartEnemyIndexes + j;
            }
        }
    }

    private void GetClearBlocks()
    {
        _clearBlocks.Clear();

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
    }

    private void CreateWorldPlace()
    {
        _world[1, 1] = 5;

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
                    _world[x, z] = 2;
                    _blockeds.Add(new Vector3Int(x, 0, z));
                }
            }
        }
        GetClearBlocks();
    }

    private void ViewCreatedWorld()
    {
        for (int x = 0; x < _xFieldSize; x++)
        {
            for (int z = 0; z < _zFieldSize; z++)
            {
                Instantiate(_floorBlock, _worldZeroPoint.position + new Vector3(x, -0.05f, z), Quaternion.identity, _worldZeroPoint);

                if (_world[x, z] == 1)
                    Instantiate(_cobbleBlock, _worldZeroPoint.position + new Vector3(x, 0.5f, z), Quaternion.identity, _worldZeroPoint);

                if (_world[x, z] == 2)
                    Instantiate(_stoneBlock, _worldZeroPoint.position + new Vector3(x, 0.5f, z), Quaternion.identity, _worldZeroPoint);

                if (_world[x, z] == 3)
                    Instantiate(_brickBlock, _worldZeroPoint.position + new Vector3(x, 0.5f, z), Quaternion.identity, _worldZeroPoint);

                if (_world[x, z] == 4)
                {
                    int index = _random.Next(_levelBoosters.Count);
                    Instantiate(_brickBlock, _worldZeroPoint.position + new Vector3(x, 0.5f, z), Quaternion.identity, _worldZeroPoint);
                    var boost = Instantiate(_boostViewer, _worldZeroPoint.position + new Vector3(x, 0.5f, z), Quaternion.Euler(90f, 0, 0), _worldZeroPoint);
                    boost.Init(_levelBoosters[index].Booster);
                    _levelBoosters.RemoveAt(index);
                }

                if (_world[x, z] == 5)
                {
                    Instantiate(_portal, _worldZeroPoint.position + new Vector3(x, 0f, z), Quaternion.identity, _worldZeroPoint);
                    _portal.Init();
                }

                if (_world[x,z] >= StartEnemyIndexes)
                {
                    int index = _world[x, z] - StartEnemyIndexes;
                    Characters enemy = Instantiate(_levelEnemys[index].Enemy, _worldZeroPoint.position + new Vector3(x, 0f, z), Quaternion.identity);
                    _settings.AddEnemyOnList(enemy);
                }
            }
        }
    }
}