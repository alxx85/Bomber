using UnityEngine;
using UnityEngine.UI;

public class SettingsViewer : MonoBehaviour
{
    [SerializeField] private Button _profileButton;
    [SerializeField] private Button _volumeButton;
    [SerializeField] private Button _inputButton;
    [SerializeField] private GameObject _profilePanel;
    [SerializeField] private GameObject _volumePanel;
    [SerializeField] private GameObject _inputPanel;

    private void OnEnable()
    {
        _profileButton.onClick.AddListener(() => { ChangePanel(ViewPanel.Profile); });
        _volumeButton.onClick.AddListener(() => { ChangePanel(ViewPanel.Volume); });
        _inputButton.onClick.AddListener(() => { ChangePanel(ViewPanel.Input); });
        ChangePanel(ViewPanel.Profile);
        
        try
        {
            GameSettings.Instance.GamePause(true);
        }
        catch { }
    }

    private void OnDisable()
    {
        _profileButton.onClick.RemoveListener(() => { ChangePanel(ViewPanel.Profile); });
        _volumeButton.onClick.RemoveListener(() => { ChangePanel(ViewPanel.Volume); });
        _inputButton.onClick.RemoveListener(() => { ChangePanel(ViewPanel.Input); });
        
        try
        {
            GameSettings.Instance.GamePause(false);
        }
        catch { }
    }

    private void ChangePanel(ViewPanel panel)
    {
        HidePanels();

        switch (panel)
        {
            case ViewPanel.Profile:
                _profilePanel.SetActive(true);
                break;
            case ViewPanel.Volume:
                _volumePanel.SetActive(true);
                break;
            case ViewPanel.Input:
                _inputPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void HidePanels()
    {
        _profilePanel.SetActive(false);
        _volumePanel.SetActive(false);
        _inputPanel.SetActive(false);
    }
}

public enum ViewPanel
{
    Profile,
    Volume,
    Input
}