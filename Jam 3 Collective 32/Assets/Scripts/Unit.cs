using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unit Enums
public enum Stance { NEUTRAL, OFFENSIVE, DEFENSIVE }
public enum Element { FIRE, WATER, EARTH }

public class Unit : MonoBehaviour
{
    public string unitName; // Unit Name
    public Stance stance;   // Unit Stance
    public Element element; // Unit Element

    public int damage;      // Unit Attack Power

    public int maxHP;       // Unit Max HP
    public int currentHP;   // Unit Current HP

    public int GetStance() { return (int)stance; }      // Stance Getter
    public int GetElement() { return (int)element; }    // Element Getter

    // Stance Setter
    public void ChangeStance(int s)
    {
        stance = (Stance)s;
        Debug.Log("Stance: " + stance);
    }

    // Element Setter
    public void ChangeElement(int e)
    {
        element = (Element)e;
        Debug.Log("Element: " + element);
    }

    // Function: TakeDamage
    // Description: Subtracts the damage (dmg) from the Units and Returns the HP Status of the Unit 
    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    // Function: TakeDamage
    // Description: Returns the HP Status of the Unit 
    public bool CheckDead()
    {
        if (currentHP <= 0)
        {
            return true;
        } else
        { 
            return false;
        }
    }
}
