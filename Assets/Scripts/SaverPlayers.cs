using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaverPlayers
{
    public static string SelectedName = "Player";

    public static void SavePlayer(string playerName, SavedProperties properties)
    {
        PlayerPrefs.SetString("Selected", SelectedName);

        List<string> allProfile = GetAllProfile();

        if (allProfile.Contains(playerName) == false)
        {
            allProfile.Add(playerName);
            SaveAllProfile(allProfile);
        }

        PlayerPrefs.SetInt("level_" + playerName, properties.Level);
        PlayerPrefs.SetInt("life_" + playerName, properties.Life);
        PlayerPrefs.SetFloat("speed_" + playerName, properties.Speed);
        PlayerPrefs.SetInt("bombAmount_" + playerName, properties.BombAmount);
        PlayerPrefs.SetInt("bombPower_" + playerName, properties.BombPower);
        PlayerPrefs.SetInt("canControlBomb_" + playerName, properties.CanActivateControlBomb ? 1 : 0);
        PlayerPrefs.SetInt("useShield_" + playerName, properties.UseShield ? 1 : 0);

        PlayerPrefs.Save();
    }

    public static SavedProperties LoadPlayer(string playerName)
    {
        List<string> allProfile = GetAllProfile();
        
        if (allProfile.Contains(playerName))
        {
            int _level = PlayerPrefs.GetInt("level_" + playerName);
            int _life = PlayerPrefs.GetInt("life_" + playerName);
            float _speed = PlayerPrefs.GetFloat("speed_" + playerName);
            int _bombAmount = PlayerPrefs.GetInt("bombAmount_" + playerName);
            int _bombPower = PlayerPrefs.GetInt("bombPower_" + playerName);
            bool _canControlBomb = PlayerPrefs.GetInt("canControlBomb_" + playerName) == 0 ? false : true;
            bool _useShield = PlayerPrefs.GetInt("useShield_" + playerName) == 0 ? false : true;
            return new SavedProperties(_level, _life, _speed, _bombAmount, _bombPower, _canControlBomb, _useShield);
        }
        return null;
    }

    public static List<string> GetAllProfile()
    {
        List<string> allNames = new List<string>();

        if (PlayerPrefs.HasKey("Selected"))
            SelectedName = PlayerPrefs.GetString("Selected");

        if (PlayerPrefs.HasKey("Names"))
        {
            string stringNames = PlayerPrefs.GetString("Names");
            allNames = stringNames.Split(";").ToList();
        }
        return allNames;
    }

    public static void RemoveProfile(string profileName)
    {
        string name = profileName;
        List<string> allProfile = GetAllProfile();

        if (allProfile.Contains(name))
        {
            allProfile.Remove(name);
            SaveAllProfile(allProfile);

            PlayerPrefs.DeleteKey("level_" + name);
            PlayerPrefs.DeleteKey("life_" + name);
            PlayerPrefs.DeleteKey("speed_" + name);
            PlayerPrefs.DeleteKey("bombAmount_" + name);
            PlayerPrefs.DeleteKey("bombPower_" + name);
            PlayerPrefs.DeleteKey("canControlBomb_" + name);
            PlayerPrefs.DeleteKey("useShield_" + name);
        }
    }
    private static void SaveAllProfile(List<string> profileList)
    {
        string allNames = "";
        allNames = string.Join(";", profileList);
        PlayerPrefs.SetString("Names", allNames);
    }
}
