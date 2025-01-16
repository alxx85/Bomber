using UnityEngine;
using TMPro;
using System;

public class InputKeyPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropList;
    [SerializeField] private ButtonKey _currentKey;

    public event Action<ButtonKey, KeyCode> ChangedKey;

    private void OnEnable()
    {
        _dropList.onValueChanged.AddListener(ChangeKey);
    }

    private void OnDisable()
    {
        _dropList.onValueChanged.RemoveListener(ChangeKey);
    }

    public void AddKey(TMP_Dropdown.OptionData key)
    {
        _dropList.options.Add(key);
    }

    public void SetSelect(int index)
    {
        _dropList.value = index;
    }

    public void ChangeKey(int index)
    {
        KeyCode key = (KeyCode)Enum.Parse(typeof(KeyCode), _dropList.options[index].text);
        ChangedKey?.Invoke(_currentKey, key);
    }
}
