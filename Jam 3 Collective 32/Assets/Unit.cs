using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stance { NEUTRAL, OFFENSIVE, DEFENSIVE }
public enum Element { FIRE, WATER, EARTH }

public class Unit : MonoBehaviour
{
    public string unitName;
    public Stance stance;
    public Element element;

    public int damage;

    public int maxHP;
    public int currentHP;

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

    public void ChangeStance(int s)
    {
        stance = (Stance) s;
        Debug.Log("Stance: " + stance);
    }

    public void ChangeElement(int e)
    {
        element = (Element) e;
        Debug.Log("Element: " + element);
    }

    public int GetStance() { return (int) stance; }
    public int GetElement() { return (int) element; }
}
