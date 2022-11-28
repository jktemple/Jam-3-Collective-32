using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN1, PLAYERTURN2, PLAYERTURN3, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject bossPrefab;

    Unit playerUnit1;
    Unit playerUnit2;
    Unit playerUnit3;
    Unit bossUnit;

    public Text dialogueText;

    public BattleHUD playerHUD1;
    public BattleHUD playerHUD2;
    public BattleHUD playerHUD3;
    public BattleHUD bossHUD;

    public BattleState state;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO1 = Instantiate(playerPrefab1);
        playerUnit1 = playerGO1.GetComponent<Unit>();

        GameObject playerGO2 = Instantiate(playerPrefab2);
        playerUnit2 = playerGO2.GetComponent<Unit>();

        GameObject playerGO3 = Instantiate(playerPrefab3);
        playerUnit3 = playerGO3.GetComponent<Unit>();

        GameObject bossGO = Instantiate(bossPrefab);
        bossUnit = bossGO.GetComponent<Unit>();

        dialogueText.text = "A " + bossUnit.unitName + " approaches...";

        playerHUD1.SetHUD(playerUnit1);
        playerHUD2.SetHUD(playerUnit2);
        playerHUD3.SetHUD(playerUnit3);
        bossHUD.SetHUD(bossUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN1;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = false;
        if (state == BattleState.PLAYERTURN1)
        {
            isDead = bossUnit.TakeDamage(playerUnit1.damage);
            dialogueText.text = "The attack 1 is successful!";
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            isDead = bossUnit.TakeDamage(playerUnit2.damage);
            dialogueText.text = "The attack 2 is successful!";
        }
        else if (state == BattleState.PLAYERTURN3)
        {
            isDead = bossUnit.TakeDamage(playerUnit3.damage);
            dialogueText.text = "The attack 3 is successful!";
        }
        
        bossHUD.SetHP(bossUnit.currentHP);
        Debug.Log("Boss: " + bossUnit.currentHP);

        yield return new WaitForSeconds(2f);

        if(isDead)
        {
            state = BattleState.WON;
            EndBattle();
        } else
        {
            if (state == BattleState.PLAYERTURN1)
            {
                state = BattleState.PLAYERTURN2;
                PlayerTurn();
            } else if (state == BattleState.PLAYERTURN2)
            {
                state = BattleState.PLAYERTURN3;
                PlayerTurn();
            } else if (state == BattleState.PLAYERTURN3)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
    }

    IEnumerator EnemyTurn()
    {
        bool isDead = false;
        if (bossUnit.GetStance() == 0) // Neutral
        {
            int attack = Random.Range(1, 4);
            switch (attack)
            {
                case 1:
                    if (bossUnit.GetElement() == 1)
                    {
                        isDead = playerUnit1.TakeDamage(2 * bossUnit.damage);
                    } else
                    {
                        isDead = playerUnit1.TakeDamage(bossUnit.damage);
                    }
                    break;
                case 2:
                    if (bossUnit.GetElement() == 2)
                    {
                        isDead = playerUnit1.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        isDead = playerUnit1.TakeDamage(bossUnit.damage);
                    }
                    break;
                case 3:
                    if (bossUnit.GetElement() == 0)
                    {
                        isDead = playerUnit1.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        isDead = playerUnit1.TakeDamage(bossUnit.damage);
                    }
                    break;
            }
        }
        else if (bossUnit.GetStance() == 1) // Offensive
        {
            switch (bossUnit.GetElement())
            {
                case 0:
                    isDead = playerUnit1.TakeDamage(bossUnit.damage);
                    isDead = playerUnit2.TakeDamage(bossUnit.damage);
                    isDead = playerUnit3.TakeDamage(2 * bossUnit.damage);
                    break;
                case 1:
                    isDead = playerUnit1.TakeDamage(2 * bossUnit.damage);
                    isDead = playerUnit2.TakeDamage(bossUnit.damage);
                    isDead = playerUnit3.TakeDamage(bossUnit.damage);
                    break;
                case 2:
                    isDead = playerUnit1.TakeDamage(bossUnit.damage);
                    isDead = playerUnit2.TakeDamage(2 * bossUnit.damage);
                    isDead = playerUnit3.TakeDamage(bossUnit.damage);
                    break;
            }
        } else if (bossUnit.GetStance() == 2) // Defensive
        {
            int attack = Random.Range(1, 4);
            switch (attack)
            {
                case 1:
                    if (bossUnit.GetElement() == 1)
                    {
                        isDead = playerUnit1.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        isDead = playerUnit1.TakeDamage(bossUnit.damage);
                    }
                    break;
                case 2:
                    if (bossUnit.GetElement() == 2)
                    {
                        isDead = playerUnit1.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        isDead = playerUnit1.TakeDamage(bossUnit.damage);
                    }
                    break;
                case 3:
                    if (bossUnit.GetElement() == 0)
                    {
                        isDead = playerUnit1.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        isDead = playerUnit1.TakeDamage(bossUnit.damage);
                    }
                    break;
            }
        }
        dialogueText.text = bossUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        playerHUD1.SetHP(playerUnit1.currentHP);
        Debug.Log("Player 1: " + playerUnit1.currentHP);
        playerHUD2.SetHP(playerUnit2.currentHP);
        Debug.Log("Player 2: " + playerUnit2.currentHP);
        playerHUD3.SetHP(playerUnit3.currentHP);
        Debug.Log("Player 3: " + playerUnit3.currentHP);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        } else
        {
            int stance_rnd = Random.Range(0, 3);
            bossUnit.ChangeStance(stance_rnd);

            int element_rnd = Random.Range(0, 3);
            bossUnit.ChangeElement(element_rnd);

            state = BattleState.PLAYERTURN1;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogueText.text = "You win!";
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You lose!";
        }
    }

    void PlayerTurn()
    {
        if (state == BattleState.PLAYERTURN1)
        {
            dialogueText.text = "Choose an action 1: ";
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            dialogueText.text = "Choose an action 2: ";
        }
        else if (state == BattleState.PLAYERTURN3)
        {
            dialogueText.text = "Choose an action 3: ";
        }
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN1 && state != BattleState.PLAYERTURN2 && state != BattleState.PLAYERTURN3)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }
}
