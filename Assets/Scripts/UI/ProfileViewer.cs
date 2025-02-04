using UnityEngine;

public class ProfileViewer : MonoBehaviour
{
    [SerializeField] private TextPresenter _lifeText;
    [SerializeField] private TextPresenter _speedText;
    [SerializeField] private TextPresenter _bobmText;
    [SerializeField] private TextPresenter _distanceText;
    [SerializeField] private TextPresenter _shieldText;
    [SerializeField] private TextPresenter _controlText;
    [SerializeField] private TextPresenter _levelText;

    private void OnEnable()
    {

        try
        {
            _lifeText.Show(GameSettings.Instance.Lifes.ToString());
            _speedText.Show(GameSettings.Instance.SpeedLevel.ToString());
            _bobmText.Show(GameSettings.Instance.Bomb.ToString());
            _distanceText.Show(GameSettings.Instance.Power.ToString());
            _shieldText.Show(GameSettings.Instance.UseShield ? "Yes" : "No");
            _controlText.Show(GameSettings.Instance.CanControl ? "Yes" : "No");
            _levelText.Show((1 + GameSettings.Instance.GetLevelNumber()).ToString());
        }
        catch
        {
            SavedProperties properties = SaverPlayers.LoadPlayer(SaverPlayers.SelectedName);

            _lifeText.Show(properties.Life.ToString());
            _speedText.Show((properties.Speed - 3).ToString());
            _bobmText.Show(properties.BombAmount.ToString());
            _distanceText.Show(properties.BombPower.ToString());
            _shieldText.Show(properties.UseShield ? "Yes" : "No");
            _controlText.Show(properties.CanActivateControlBomb ? "Yes" : "No");
            _levelText.Show((properties.Level).ToString());

        }

        //_lifeText.Show(GameSettings.Instance.Lifes.ToString());
        //_speedText.Show(GameSettings.Instance.SpeedLevel.ToString());
        //_bobmText.Show(GameSettings.Instance.Bomb.ToString());
        //_distanceText.Show(GameSettings.Instance.Power.ToString());

        //_shieldText.Show(GameSettings.Instance.UseShield? "Yes":"No");
        
        //_controlText.Show(GameSettings.Instance.CanControl ? "Yes" : "No");
        //_levelText.Show((1 + GameSettings.Instance.GetLevelNumber()).ToString());
    }
}
