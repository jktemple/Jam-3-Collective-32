using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unit Enums
public enum Stance { NEUTRAL, OFFENSIVE, DEFENSIVE }
public enum Element { FIRE, WATER, EARTH }
public enum Attack { NULL ,OFFENSIVE, DEFENSIVE, ATKBUFF, DEFBUFF }

public class Unit : MonoBehaviour
{
    public string unitName; // Unit Name

    public float maxHP;       // Unit Max HP
    public float currentHP;   // Unit Current HP

    public Stance stance;       // Unit Stance
    public Element element;     // Unit Element
    public Element weakness;    // Unit Weakness
    public Attack attack;       // Unit Attack
    public bool atkBuff = false;    // Will Next Turn Be Attack Buffed
    public bool defBuff = false;    // Will Next Turn Be Defense Buffed


    public int damage;      // Unit Attack Power

    public Words[] words;   // Unit Word Array

    public int GetStance() { return (int)stance; }      // Stance Getter
    public int GetElement() { return (int)element; }    // Element Getter
    public int GetWeakness() { return (int)weakness; }  // Weakness Getter
    public int GetAttack() { return (int)attack; }      // Attack Getter 

    // Reset
    public void WizardReset()
    {
        attack = Attack.NULL;
    }

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
        switch (element)
        {
            case Element.FIRE:
                weakness = Element.WATER;
                break;
            case Element.WATER:
                weakness = Element.EARTH;
                break;
            case Element.EARTH:
                weakness = Element.FIRE;
                break;
        }
        Debug.Log("Element: " + element);
    }
    // Element Setter
    public void ChangeWeakness(int w)
    {
        weakness = (Element)w;
        Debug.Log("Weakness: " + element);
    }

    // Attack Setter
    public void ChangeAttack(int a)
    {
        attack = (Attack)a;
        Debug.Log("Attack: " + attack);
    }

    // Function: TakeDamage
    // Description: Subtracts the damage (dmg) from the Units and Returns the HP Status of the Unit 
    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
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
