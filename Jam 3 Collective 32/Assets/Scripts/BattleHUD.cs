using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class: BattleHUD
// Description: Class Handling Unit Name and Health Information
public class BattleHUD : MonoBehaviour
{
    public Text nameText;   // Unit Name Attribute
    public Slider hpSlider; // Unit HP Slider Attribute

    // Function: SetHUD
    // Description: Initializes Battle HUD with Unit Information
    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    // Function: SetHP
    // Description: Initializes HP Slider with HP value
    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
