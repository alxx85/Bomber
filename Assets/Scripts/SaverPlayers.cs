using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaverPlayers
{
    public static string SelectedName = "Player";

    public static void SavePlayer(string playerName, SavedProperties properties, InputSettings keys)
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
        PlayerPrefs.SetInt("keyLeft" + playerName,(int)keys.LeftKey);
        PlayerPrefs.SetInt("keyRight" + playerName, (int)keys.RightKey);
        PlayerPrefs.SetInt("keyUp" + playerName, (int)keys.ForwardKey);
        PlayerPrefs.SetInt("keyDown" + playerName, (int)keys.BackKey);
        PlayerPrefs.SetInt("keySet" + playerName, (int)keys.SetBombKey);
        PlayerPrefs.SetInt("keyUse" + playerName,(int)keys.ControlBombKey);

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

    public static void LoadPlayerKeys(string playerName, InputSettings playerInputs)
    {
        List<string> allProfile = GetAllProfile();

        if (allProfile.Contains(playerName))
        {
            playerInputs.ChangeKey(ButtonKey.Left, (KeyCode)PlayerPrefs.GetInt("keyLeft" + playerName));
            playerInputs.ChangeKey(ButtonKey.Right, (KeyCode)PlayerPrefs.GetInt("keyRight" + playerName));
            playerInputs.ChangeKey(ButtonKey.Forward, (KeyCode)PlayerPrefs.GetInt("keyUp" + playerName));
            playerInputs.ChangeKey(ButtonKey.Back, (KeyCode)PlayerPrefs.GetInt("keyDown" + playerName));
            playerInputs.ChangeKey(ButtonKey.SetBomb, (KeyCode)PlayerPrefs.GetInt("keySet" + playerName));
            playerInputs.ChangeKey(ButtonKey.ControlBomb, (KeyCode)PlayerPrefs.GetInt("keyUse" + playerName));
        }
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
            PlayerPrefs.DeleteKey("keyLeft" + name);
            PlayerPrefs.DeleteKey("keyRight" + name);
            PlayerPrefs.DeleteKey("keyUp" + name);
            PlayerPrefs.DeleteKey("keyDown" + name);
            PlayerPrefs.DeleteKey("keySet" + name);
            PlayerPrefs.DeleteKey("keyUse" + name);
        }
    }

    public static void SaveVolumeSetting(VolumeSetting setting)
    {
        PlayerPrefs.SetInt("Muting", setting.Mute ? 1 : 0);
        PlayerPrefs.SetFloat("Volume_Char", setting.GetVolumes(SoundType.Characters));
        PlayerPrefs.SetFloat("Volume_Env", setting.GetVolumes(SoundType.Environment));
        PlayerPrefs.SetFloat("Volume_Music", setting.GetVolumes(SoundType.Music));
    }

    public static void LoadVolumeSetting(VolumeSetting setting)
    {
        setting.ChangeVolume(SoundType.Characters, PlayerPrefs.GetFloat("Volume_Char"));
        setting.ChangeVolume(SoundType.Environment, PlayerPrefs.GetFloat("Volume_Env"));
        setting.ChangeVolume(SoundType.Music, PlayerPrefs.GetFloat("Volume_Music"));
        setting.Muting(PlayerPrefs.GetInt("Muting") == 0 ? false : true);
    }

    private static void SaveAllProfile(List<string> profileList)
    {
        string allNames = "";
        allNames = string.Join(";", profileList);
        PlayerPrefs.SetString("Names", allNames);
    }
}
