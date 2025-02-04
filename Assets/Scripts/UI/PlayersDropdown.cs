using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class PlayersDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    private void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        Refresh();
    }

    public void Refresh()
    {
        _dropdown.options.Clear();

        List<string> players = SaverPlayers.GetAllProfile();
        _dropdown.options.Add(new TMP_Dropdown.OptionData("New profile"));

        for (int i = 0; i < players.Count; i++)
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData(players[i]));

            if (SaverPlayers.SelectedName == players[i])
                _dropdown.value = i + 1;
        }
    }
}
