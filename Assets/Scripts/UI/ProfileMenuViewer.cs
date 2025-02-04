using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProfileMenuViewer : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputNameText;
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private Button _removeButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private StartScreen _mainMenuScreen;

    private void OnEnable()
    {
        _dropdown.onValueChanged.AddListener(OnChangedSelect);
        _removeButton.onClick.AddListener(OnRemovedSelection);
        _backButton.onClick.AddListener(OnBack);
        _selectButton.onClick.AddListener(OnSelectClick);
    }

    private void OnDisable()
    {
        _dropdown.onValueChanged.RemoveListener(OnChangedSelect);
        _removeButton.onClick.RemoveListener(OnRemovedSelection);
        _backButton.onClick.RemoveListener(OnBack);
        _selectButton.onClick.RemoveListener(OnSelectClick);
    }

    private void OnChangedSelect(int index)
    {
        if (index == 0)
        {
            _inputNameText.interactable = true;
            _inputNameText.text = "";
        }
        else
        {
            _inputNameText.interactable = false;
            _inputNameText.text = _dropdown.options[index].text;
        }
    }

    private void OnRemovedSelection()
    {
        string name = _dropdown.options[_dropdown.value].text;
        SaverPlayers.RemoveProfile(name);
        _dropdown.GetComponent<PlayersDropdown>().Refresh();
        List<string> players = SaverPlayers.GetAllProfile();

        if (players.Count > 0)
            name = players[0];
        else
            name = "Player";

        SaverPlayers.SelectedName = name;
    }

    private void OnBack()
    {
        _mainMenuScreen.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnSelectClick()
    {
        string name = "";

        if (_dropdown.value == 0)
        {
            name = _inputNameText.text;
        }
        else
        {
            name = _dropdown.options[_dropdown.value].text;
        }
        
        SaverPlayers.SelectedName = name;
        OnBack();
    }
}
