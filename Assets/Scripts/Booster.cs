using UnityEngine;

[CreateAssetMenu(fileName = "Boost", menuName = "Boosts/NewBoost", order = 50)]
public class Booster : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Color _color;
    [SerializeField] private bool _life;
    [SerializeField] private bool _speed;
    [SerializeField] private bool _addBombAmount;
    [SerializeField] private bool _addBombPower;
    [SerializeField] private bool _kick;
    [SerializeField] private bool _shield;

    public Sprite Sprite => _sprite;
    public Color BackgroundColor => _color;
    public Boost AddBoost => new Boost(_life, _speed, _addBombAmount, _addBombPower, _kick, _shield);
}
