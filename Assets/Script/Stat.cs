using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers;
    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }
    public void AddModifier(int _modifier)
    {
        if (_modifier == 0) return;
        modifiers.Add( _modifier );
    }
    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    public void RemoveModifier(int _modifier)
    {
        if(_modifier == 0) return;
        modifiers.RemoveAt( _modifier );
    }
}
