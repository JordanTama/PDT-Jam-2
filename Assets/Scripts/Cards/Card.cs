using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public new string name;
    public Color color;
    public int value;

    public bool hasComboEffect;
    public int cardValueToActivateCombo;
    public int valueToAddOnComboActivation;
}
