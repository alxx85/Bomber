using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class InputViewer : MonoBehaviour
{
    [SerializeField] private InputKeyPresenter _leftKeyPresenter;
    [SerializeField] private InputKeyPresenter _rightKeyPresenter;
    [SerializeField] private InputKeyPresenter _forwardKeyPresenter;
    [SerializeField] private InputKeyPresenter _backKeyPresenter;
    [SerializeField] private InputKeyPresenter _setBombKeyPresenter;
    [SerializeField] private InputKeyPresenter _activateKeyPresenter;

    private InputSettings _settings;

    private void Start()
    {
        _settings = GameSettings.Instance.InputKeys;
        TMP_Dropdown.OptionData option;

        KeyCode[] keys = System.Enum.GetValues(typeof(KeyCode)) as KeyCode[];

        for (int i = 0; i < keys.Length; i++)
        {
            option = new TMP_Dropdown.OptionData() { text = keys[i].ToString() };
            AddNewKey(option);

            if (_settings.LeftKey == keys[i])
                _leftKeyPresenter.SetSelect(i);
            if (_settings.RightKey == keys[i])
                _rightKeyPresenter.SetSelect(i);
            if (_settings.ForwardKey == keys[i])
                _forwardKeyPresenter.SetSelect(i);
            if (_settings.BackKey == keys[i])
                _backKeyPresenter.SetSelect(i);
            if (_settings.SetBombKey == keys[i])
                _setBombKeyPresenter.SetSelect(i);
            if (_settings.ControlBombKey == keys[i])
                _activateKeyPresenter.SetSelect(i);
        }
    }

    private void OnEnable()
    {
        _leftKeyPresenter.ChangedKey += OnChangedKey;
        _rightKeyPresenter.ChangedKey += OnChangedKey;
        _forwardKeyPresenter.ChangedKey += OnChangedKey;
        _backKeyPresenter.ChangedKey += OnChangedKey;
        _setBombKeyPresenter.ChangedKey += OnChangedKey;
        _activateKeyPresenter.ChangedKey += OnChangedKey;
    }

    private void OnDisable()
    {
        _leftKeyPresenter.ChangedKey -= OnChangedKey;
        _rightKeyPresenter.ChangedKey -= OnChangedKey;
        _forwardKeyPresenter.ChangedKey -= OnChangedKey;
        _backKeyPresenter.ChangedKey -= OnChangedKey;
        _setBombKeyPresenter.ChangedKey -= OnChangedKey;
        _activateKeyPresenter.ChangedKey -= OnChangedKey;
    }

    private void AddNewKey(TMP_Dropdown.OptionData key)
    {
        _leftKeyPresenter.AddKey(key);
        _rightKeyPresenter.AddKey(key);
        _forwardKeyPresenter.AddKey(key);
        _backKeyPresenter.AddKey(key);
        _setBombKeyPresenter.AddKey(key);
        _activateKeyPresenter.AddKey(key);
    }

    private void OnChangedKey(ButtonKey button, KeyCode key)
    {
        _settings.ChangeKey(button, key);
    }
}
