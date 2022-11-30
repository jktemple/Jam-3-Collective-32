using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Game State Enum
public enum BattleState { START, PLAYERTURN1, PLAYERTURN2, PLAYERTURN3, PLAYERATTACK, ENEMYTURN, WON, LOST}
public enum ElementType { FIRE, WATER, EARTH }
public enum AttackType { OFFENSIVE, DEFENSIVE, BUFF }

public class BattleSystem : MonoBehaviour
{
    // Prefab Declaration
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject bossPrefab;

    // Unit Declaration 
    Unit playerUnit1;
    Unit playerUnit2;
    Unit playerUnit3;
    Unit bossUnit;

    // Dialogue Box Declaration
    public Text dialogueText;

    // Battle HUD Declaration
    public BattleHUD playerHUD1;
    public BattleHUD playerHUD2;
    public BattleHUD playerHUD3;
    public BattleHUD bossHUD;

    // Game State Declaration
    public BattleState state;

    // Game Start - Starts Game
    void Start()
    {
        // BattleState = START
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    // SetupBattle - Initializes Necessary Entities
    IEnumerator SetupBattle()
    {
        // Initializes Units using the corresponding prefab
        GameObject playerGO1 = Instantiate(playerPrefab1);
        playerUnit1 = playerGO1.GetComponent<Unit>();
        GameObject playerGO2 = Instantiate(playerPrefab2);
        playerUnit2 = playerGO2.GetComponent<Unit>();
        GameObject playerGO3 = Instantiate(playerPrefab3);
        playerUnit3 = playerGO3.GetComponent<Unit>();
        GameObject bossGO = Instantiate(bossPrefab);
        bossUnit = bossGO.GetComponent<Unit>();

        // Sets initial dialogue text
        dialogueText.text = "A " + bossUnit.unitName + " approaches...";

        // Sets initial Unit HUD Data
        playerHUD1.SetHUD(playerUnit1);
        playerHUD2.SetHUD(playerUnit2);
        playerHUD3.SetHUD(playerUnit3);
        bossHUD.SetHUD(bossUnit);

        // Waits 2 Seconds to allow the play to read
        yield return new WaitForSeconds(2f);

        // BattleState = PLAYERTURN1
        state = BattleState.PLAYERTURN1;
        PlayerTurn();
    }

    // PlayerTurn - Sets dialogue and waits for player's option selection
    void PlayerTurn()
    {
        // Changes dialogue according to player choice.
        switch (state)
        {
            case BattleState.PLAYERTURN1:
                dialogueText.text = "Choose an action 1: ";
                break;
            case BattleState.PLAYERTURN2:
                dialogueText.text = "Choose an action 2: ";
                break;
            case BattleState.PLAYERTURN3:
                dialogueText.text = "Choose an action 3: ";
                break;
        }
    }

    // This Button Can Be Seperated Accord to How We Want to 
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN1 && state != BattleState.PLAYERTURN2 && state != BattleState.PLAYERTURN3)
        {
            return;
        } else if (state != BattleState.PLAYERTURN1)
        {
            PlayerChoice(ElementType.FIRE, AttackType.OFFENSIVE);
        } else if (state != BattleState.PLAYERTURN2)
        {
            PlayerChoice(ElementType.WATER, AttackType.OFFENSIVE);
        } else if (state != BattleState.PLAYERTURN3)
        {
            PlayerChoice(ElementType.EARTH, AttackType.OFFENSIVE);
        }
    }

    // PlayerChoice - Saves Player's Choice for Player Attack
    ElementType[] elements = new ElementType[3];
    AttackType[] attacks = new AttackType[3];
    void PlayerChoice(ElementType et, AttackType at)
    {
        switch (state)
        {
            case BattleState.PLAYERTURN1:
                elements[0] = et;
                attacks[0] = at;
                state = BattleState.PLAYERTURN2;
                PlayerTurn();
                break;
            case BattleState.PLAYERTURN2:
                elements[1] = et;
                attacks[1] = at;
                state = BattleState.PLAYERTURN3;
                PlayerTurn();
                break;
            case BattleState.PLAYERTURN3:
                elements[2] = et;
                attacks[2] = at;
                state = BattleState.PLAYERATTACK;
                StartCoroutine(PlayerAttack());
                break;
        }
    }

    // PlayerAttack - Players Actions Execute (The Rhythm Game Can Be Called Prior to this Function but After the PlayerChoice Function)
    IEnumerator PlayerAttack()
    {
        // Player 1 Attacks
        bool isDead = false;
        isDead = bossUnit.TakeDamage(playerUnit1.damage);
        dialogueText.text = "The attack 1 is successful!";
        bossHUD.SetHP(bossUnit.currentHP);
        Debug.Log("Boss: " + bossUnit.currentHP);
        CheckDead(isDead);

        yield return new WaitForSeconds(2f);

        // Player 2 Attacks
        isDead = bossUnit.TakeDamage(playerUnit2.damage);
        dialogueText.text = "The attack 2 is successful!";
        bossHUD.SetHP(bossUnit.currentHP);
        Debug.Log("Boss: " + bossUnit.currentHP);
        CheckDead(isDead);

        yield return new WaitForSeconds(2f);

        // Player 3 Attacks
        isDead = bossUnit.TakeDamage(playerUnit3.damage);
        dialogueText.text = "The attack 3 is successful!";
        bossHUD.SetHP(bossUnit.currentHP);
        Debug.Log("Boss: " + bossUnit.currentHP);
        CheckDead(isDead);

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
    }

    // EnemyTurn - The Enemy's Actions Execute
    IEnumerator EnemyTurn()
    {
        if (bossUnit.GetStance() == 0) // Neutral
        {
            int attack = Random.Range(1, 4);
            switch (attack)
            {
                case 1:
                    if (bossUnit.GetElement() == 1)
                    {
                        playerUnit1.TakeDamage(2 * bossUnit.damage);
                    } else
                    {
                        playerUnit1.TakeDamage(bossUnit.damage);
                    }
                    break;
                case 2:
                    if (bossUnit.GetElement() == 2)
                    {
                        playerUnit2.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        playerUnit2.TakeDamage(bossUnit.damage);
                    }
                    break;
                case 3:
                    if (bossUnit.GetElement() == 0)
                    {
                        playerUnit3.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        playerUnit3.TakeDamage(bossUnit.damage);
                    }
                    break;
            }
        }
        else if (bossUnit.GetStance() == 1) // Offensive
        {
            switch (bossUnit.GetElement())
            {
                case 0:
                    playerUnit1.TakeDamage(bossUnit.damage);
                    playerUnit2.TakeDamage(bossUnit.damage);
                    playerUnit3.TakeDamage(2 * bossUnit.damage);
                    break;
                case 1:
                    playerUnit1.TakeDamage(2 * bossUnit.damage);
                    playerUnit2.TakeDamage(bossUnit.damage);
                    playerUnit3.TakeDamage(bossUnit.damage);
                    break;
                case 2:
                    playerUnit1.TakeDamage(bossUnit.damage);
                    playerUnit2.TakeDamage(2 * bossUnit.damage);
                    playerUnit3.TakeDamage(bossUnit.damage);
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
                        playerUnit1.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        playerUnit1.TakeDamage(bossUnit.damage);
                    }
                    break;
                case 2:
                    if (bossUnit.GetElement() == 2)
                    {
                        playerUnit2.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        playerUnit2.TakeDamage(bossUnit.damage);
                    }
                    break;
                case 3:
                    if (bossUnit.GetElement() == 0)
                    {
                        playerUnit3.TakeDamage(2 * bossUnit.damage);
                    }
                    else
                    {
                        playerUnit3.TakeDamage(bossUnit.damage);
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

        bool isDead = playerUnit1.CheckDead() && playerUnit2.CheckDead() && playerUnit3.CheckDead();
        CheckDead(isDead);

        int stance_rnd = Random.Range(0, 3);
        bossUnit.ChangeStance(stance_rnd);

        int element_rnd = Random.Range(0, 3);
        bossUnit.ChangeElement(element_rnd);

        state = BattleState.PLAYERTURN1;
        PlayerTurn();
    }

    // CheckDead - Check to See if the Units are Dead for Win or Lose Condition
    void CheckDead(bool isDead)
    {
        if (state == BattleState.PLAYERATTACK && isDead)
        {
            state = BattleState.WON;
            EndBattle();
        } else if (state == BattleState.ENEMYTURN && isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
    }

    // EndBattle - Displays the Appropriate Message for Win or Lose Condition
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
}
