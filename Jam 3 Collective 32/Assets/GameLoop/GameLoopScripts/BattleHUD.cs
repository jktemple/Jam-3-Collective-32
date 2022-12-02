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
    public Unit unit;

    public void Awake(){
        if(unit){
         nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        }
    }
    // Function: SetHUD
    // Description: Initializes Battle HUD with Unit Information
    public void SetHUD(Unit u)
    {
        nameText.text = u.unitName;
        hpSlider.maxValue = u.maxHP;
        hpSlider.value = u.currentHP;
    }

    // Function: SetHP
    // Description: Initializes HP Slider with HP value
    public void SetHP(float hp)
    {
        hpSlider.value = hp;
    }
}
