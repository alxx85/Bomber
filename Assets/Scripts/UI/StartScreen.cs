using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    private void Start()
    {
        _profileName.text = SaverPlayers.SelectedName;
        _startButton.onClick.AddListener(OnStartUsed);
        _profileButton.onClick.AddListener(OnProfileUsed);
        _settingButton.onClick.AddListener(OnSettingUsed);
        _quitButton.onClick.AddListener(OnQuitUsed);
    }

    private void OnQuitUsed()
    {
        Application.Quit();
    }

    private void OnSettingUsed()
    {
        throw new NotImplementedException();
    }

    private void OnProfileUsed()
    {
        throw new NotImplementedException();
    }

    private void OnStartUsed()
    {
        SaverPlayers.SelectedName = _profileName.text;
        SceneManager.LoadScene(GAMESCENE);
    }

}
