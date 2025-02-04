using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    private const string GAMESCENE = "GamePlayScene";

    [SerializeField] private TMP_Text _profileName;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _profileButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private ProfileMenuViewer _profileMenu;
    [SerializeField] private SettingsViewer _settingsMenu;
    [SerializeField] private VolumeSetting _defaultVolume;

    private void Start()
    {
        _profileName.text = SaverPlayers.SelectedName;
        _startButton.onClick.AddListener(OnStartUsed);
        _profileButton.onClick.AddListener(OnProfileUsed);
        _settingButton.onClick.AddListener(OnSettingUsed);
        _quitButton.onClick.AddListener(OnQuitUsed);

        SaverPlayers.LoadVolumeSetting(_defaultVolume);
    }

    private void OnQuitUsed()
    {
        Application.Quit();
    }

    private void OnSettingUsed()
    {
        _settingsMenu.gameObject.SetActive(true);
    }

    private void OnProfileUsed()
    {
        _profileMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnStartUsed()
    {
        SaverPlayers.SelectedName = _profileName.text;
        SceneManager.LoadScene(GAMESCENE);
    }

}
