using UnityEngine;
using UnityEngine.UI;

public class EndGameViewer : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _getLifeButton;
    [SerializeField] private Button _restartGameButton;
    [SerializeField] private StartProperties _newGameProperties;

    private void Start()
    {
        GameSettings.Instance.InitEndGameScreen(this);
        _mainMenuButton.onClick.AddListener(OnMenuClick);
        _getLifeButton.onClick.AddListener(OnGetLifeButtonClick);
        _restartGameButton.onClick.AddListener(OnRestartGameButtonClick);
        Hide();
    }

    public void Show() => gameObject.SetActive(true);

    private void Hide() => gameObject.SetActive(false);

    private void OnGetLifeButtonClick()
    {
        GameSettings.Instance.GetExtraLife();
        Hide();
    }

    private void OnRestartGameButtonClick()
    {
        GameSettings.Instance.RestartGame(_newGameProperties);
    }

    private void OnMenuClick()
    {
        //Show menu scene
    }
}
