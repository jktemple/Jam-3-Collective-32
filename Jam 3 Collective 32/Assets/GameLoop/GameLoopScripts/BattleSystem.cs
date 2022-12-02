using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Game State Enum
public enum BattleState { START, PLAYERTURN1, PLAYERTURN2, PLAYERTURN3, RHYTHMTURN ,PLAYERATTACK, ENEMYTURN, WON, LOST}
public enum ElementType { FIRE, WATER, EARTH }
public enum AttackType { OFFENSIVE, DEFENSIVE, BUFF }

public class BattleSystem : MonoBehaviour
{
    //Animators
    Animator bossAnimator;
    
    // Prefab Declaration
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject bossPrefab;
    public RhythmManager rhythmSystem;


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

    // Array for Rhythm minigame decloration
    int[] wordArray = { RhythmManager.FELDRFIRE,
                        RhythmManager.NEKKINULL,
                        RhythmManager.VATNWATER,
                        RhythmManager.NEKKINULL,
                        RhythmManager.JORDEARTH,
                        RhythmManager.NEKKINULL};

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
        bossGO.transform.position = bossGO.transform.position + new Vector3(140,-50,10);
        bossUnit = bossGO.GetComponent<Unit>();
        bossAnimator = bossGO.GetComponent<Animator>();

        // Set Boss Animation
        if (bossUnit.element == Element.FIRE)
        {
            bossAnimator.SetTrigger("BossToFire");
        }
        else if (bossUnit.element == Element.WATER)
        {
            bossAnimator.SetTrigger("BossToWater");
        }
        else
        {
            bossAnimator.SetTrigger("BossToEarth");
        }

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
                if (playerUnit1.CheckDead() == true) {
                    state = BattleState.PLAYERTURN2;
                    PlayerTurn();
                } else {
                    dialogueText.text = "Select Fire Wizard's Spell: ";
                }
                break;
            case BattleState.PLAYERTURN2:
                if (playerUnit2.CheckDead() == true) {
                    state = BattleState.PLAYERTURN3;
                    PlayerTurn();
                } else
                {
                    dialogueText.text = "Select Water Wizard's Spell ";
                }
                break;
            case BattleState.PLAYERTURN3:
                if (playerUnit3.CheckDead() == true)
                {
                    state = BattleState.RHYTHMTURN;
                    StartCoroutine(RhythmTurn());
                }
                else
                {
                    dialogueText.text = "Select Earth Wizard's Spell: ";
                }
                break;
        }
    }

    // OnOffensiveButton - Player selects offensive attack
    public void OnOffensiveButton()
    {
        if (state != BattleState.PLAYERTURN1 && state != BattleState.PLAYERTURN2 && state != BattleState.PLAYERTURN3)
        {
            return;
        }
        else if (state == BattleState.PLAYERTURN1)
        {
            playerUnit1.ChangeAttack(1);
            wordArray[1] = RhythmManager.ORDOMROFF;
            Debug.Log("Player1: Offensive");
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            playerUnit2.ChangeAttack(1);
            wordArray[3] = RhythmManager.ORDOMROFF;
            Debug.Log("Player2: Offensive");
        }
        else if (state == BattleState.PLAYERTURN3)
        {
            playerUnit3.ChangeAttack(1);
            wordArray[5] = RhythmManager.ORDOMROFF;
            Debug.Log("Player3: Offensive");
        }
    }

    // OnDefensiveButton - Player selects defensive attack
    public void OnDefensiveButton()
    {
        if (state != BattleState.PLAYERTURN1 && state != BattleState.PLAYERTURN2 && state != BattleState.PLAYERTURN3)
        {
            return;
        }
        else if (state == BattleState.PLAYERTURN1)
        {
            playerUnit1.ChangeAttack(2);
            wordArray[1] = RhythmManager.VARDDEF;
            Debug.Log("Player1: Defensive");
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            playerUnit2.ChangeAttack(2);
            wordArray[3] = RhythmManager.VARDDEF;
            Debug.Log("Player2: Defensive");
        }
        else if (state == BattleState.PLAYERTURN3)
        {
            playerUnit3.ChangeAttack(2);
            wordArray[5] = RhythmManager.VARDDEF;
            Debug.Log("Player3: Defensive");
        }
    }

    // OnAtkBuffButton - Player selects attack buff
    public void OnAtkBuffButton()
    {
        if (state != BattleState.PLAYERTURN1 && state != BattleState.PLAYERTURN2 && state != BattleState.PLAYERTURN3)
        {
            return;
        }
        else if (state == BattleState.PLAYERTURN1)
        {
            playerUnit1.ChangeAttack(3);
            wordArray[1] = RhythmManager.SARABUFF;
            Debug.Log("Player1: AtkBuff");
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            playerUnit2.ChangeAttack(3);
            wordArray[3] = RhythmManager.SARABUFF;
            Debug.Log("Player2: AtkBuff");
        }
        else if (state == BattleState.PLAYERTURN3)
        {
            playerUnit3.ChangeAttack(3);
            wordArray[5] = RhythmManager.SARABUFF;
            Debug.Log("Player3: AtkBuff");
        }
    }

    // OnBuffButton - Player selects defense buff
    public void OnDefBuffButton()
    {
        if (state != BattleState.PLAYERTURN1 && state != BattleState.PLAYERTURN2 && state != BattleState.PLAYERTURN3)
        {
            return;
        }
        else if (state == BattleState.PLAYERTURN1)
        {
            playerUnit1.ChangeAttack(4);
            wordArray[1] = RhythmManager.SKJALDBUFF;
            Debug.Log("Player1: DefBuff");
        }
        else if (state == BattleState.PLAYERTURN2)
        {
            playerUnit2.ChangeAttack(4);
            wordArray[3] = RhythmManager.SKJALDBUFF;
            Debug.Log("Player2: DefBuff");
        }
        else if (state == BattleState.PLAYERTURN3)
        {
            playerUnit3.ChangeAttack(4);
            wordArray[5] = RhythmManager.SKJALDBUFF;
            Debug.Log("Player3: DefBuff");
        }
    }

    // This Button Can Be Seperated Accord to How We Want to 
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN1 && state != BattleState.PLAYERTURN2 && state != BattleState.PLAYERTURN3)
        {
            return;
        } else if (state == BattleState.PLAYERTURN1 && playerUnit1.GetAttack() != 0)
        {
            state = BattleState.PLAYERTURN2;
            PlayerTurn();
        } else if (state == BattleState.PLAYERTURN2 && playerUnit2.GetAttack() != 0)
        {
            state = BattleState.PLAYERTURN3;
            PlayerTurn();
        } else if (state == BattleState.PLAYERTURN3 && playerUnit3.GetAttack() != 0)
        {
            state = BattleState.RHYTHMTURN;
            StartCoroutine(RhythmTurn());
        }
    }

    IEnumerator RhythmTurn()
    {
        // Rhythm Stuff Goes Here
        dialogueText.text = "Casting...";  
        rhythmSystem.wordParse(wordArray);
        while (!rhythmSystem.done)
        {
            yield return new WaitForSeconds(0.25f);
        }
        state = BattleState.PLAYERATTACK;
        StartCoroutine(PlayerAttack());
    }


    // PlayerAttack - Players Actions Execute (The Rhythm Game Can Be Called Prior to this Function but After the PlayerChoice Function)
    IEnumerator PlayerAttack()
    {
        // Player 1 (Fire Wizard) Attacks
        float dmgMultipler = 1;
        bool isDead = false;
        switch (playerUnit1.GetAttack())
        {
            // Offensive
            case 1:
                if (bossUnit.GetStance() == 2)
                {
                    if (bossUnit.GetWeakness() == playerUnit1.GetElement()) // Super Effective Attack on Defensive Stance
                    {
                        dmgMultipler = 1;
                        dialogueText.text = "The Flame Wizard's attack PENETRATES THE BOSS'S DEFENSES!";
                    } else // Effective / Not Very Effective Attack on Defensive Stance
                    {
                        dmgMultipler = 0;
                        dialogueText.text = "The Flame Wizard's attack BOUNCES OFF THE BOSS'S DEFENSES...";
                    }
                } else if (bossUnit.GetStance() != 2)
                {
                    if (bossUnit.GetWeakness() == playerUnit1.GetElement()) // Super Effective Attack
                    {
                        dmgMultipler = 2;
                        dialogueText.text = "The Flame Wizard's attack was SUPER EFFECTIVE!";
                    } else if (bossUnit.GetElement() == playerUnit1.GetElement()) // Effective Attack
                    {
                        dmgMultipler = 1;
                        dialogueText.text = "The Flame Wizard's attack was EFFECTIVE.";
                    } else if (bossUnit.GetElement() == playerUnit1.GetWeakness()) // Not Very Effective Atack
                    {
                        dmgMultipler = (float) 0.5;
                        dialogueText.text = "The Flame Wizard's attack was NOT VERY EFFECTIVE...";
                    }
                }

                if (playerUnit1.atkBuff == true)
                {
                    dmgMultipler *= 2;
                    playerUnit1.atkBuff = false;
                    playerUnit1.buffActive = 0;
                }
                break;
            // Defensive
            case 2:
                dmgMultipler = 0;
                dialogueText.text = "The Flame Wizard's conjures a FIRE SHIELD!";
                break;
            // AtkBuff
            case 3:
                dmgMultipler = 0;
                dialogueText.text = "The Flame Wizard's casts a ATTACK BUFF!";
                playerUnit1.atkBuff = true;
                playerUnit1.buffActive = 1;
                break;
            // DefBuff
            case 4:
                dmgMultipler = 0;
                dialogueText.text = "The Flame Wizard's casts a DEFENSE BUFF!";
                playerUnit1.defBuff = true;
                playerUnit1.buffActive = 1;
                break;
            // NULL (Dead)
            default:
                dmgMultipler = 0;
                break;
        }
        Debug.Log("Dmg Mult P1: " + dmgMultipler);
        bossUnit.TakeDamage(rhythmSystem.score * dmgMultipler * playerUnit1.damage);
        isDead = bossUnit.CheckDead();
        bossHUD.SetHP(bossUnit.currentHP);
        Debug.Log("Boss: " + bossUnit.currentHP);
        CheckDead(isDead);
        playerPrefab1.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(2f);

        // Player 2 Attacks
        dmgMultipler = 1;
        switch (playerUnit2.GetAttack())
        {
            // Offensive
            case 1:
                if (bossUnit.GetStance() == 2)
                {
                    if (bossUnit.GetWeakness() == playerUnit2.GetElement()) // Super Effective Attack on Defensive Stance
                    {
                        dmgMultipler = 1;
                        dialogueText.text = "The Aqua Wizard's attack PENETRATES THE BOSS'S DEFENSES!";
                    }
                    else // Effective / Not Very Effective Attack on Defensive Stance
                    {
                        dmgMultipler = 0;
                        dialogueText.text = "The Aqua Wizard's attack BOUNCES OFF THE BOSS'S DEFENSES...";
                    }
                }
                else if (bossUnit.GetStance() != 2)
                {
                    if (bossUnit.GetWeakness() == playerUnit2.GetElement()) // Super Effective Attack
                    {
                        dmgMultipler = 2;
                        dialogueText.text = "The Aqua Wizard's attack was SUPER EFFECTIVE!";
                    }
                    else if (bossUnit.GetElement() == playerUnit2.GetElement()) // Effective Attack
                    {
                        dmgMultipler = 1;
                        dialogueText.text = "The Aqua Wizard's attack was EFFECTIVE.";
                    }
                    else if (bossUnit.GetElement() == playerUnit2.GetWeakness()) // Not Very Effective Atack
                    {
                        dmgMultipler = (float) 0.5;
                        dialogueText.text = "The Aqua Wizard's attack was NOT VERY EFFECTIVE...";
                    }
                }

                if (playerUnit2.atkBuff == true)
                {
                    dmgMultipler *= 2;
                    playerUnit1.atkBuff = false;
                    playerUnit1.buffActive = 0;
                }
                break;
            // Defensive
            case 2:
                dmgMultipler = 0;
                dialogueText.text = "The Aqua Wizard's conjures a WATER SHIELD!";
                break;
            // AtkBuff
            case 3:
                dmgMultipler = 0;
                dialogueText.text = "The Aqua Wizard's casts a ATTACK BUFF!";
                playerUnit2.atkBuff = true;
                playerUnit2.buffActive = 1;
                break;
            // DefBuff
            case 4:
                dmgMultipler = 0;
                dialogueText.text = "The Aqua Wizard's casts a DEFENSE BUFF!";
                playerUnit2.defBuff = true;
                playerUnit2.buffActive = 1;
                break;
            // NULL (Dead)
            default:
                dmgMultipler = 0;
                break;
        }
        Debug.Log("Dmg Mult P2: " + dmgMultipler);
        bossUnit.TakeDamage(rhythmSystem.score * dmgMultipler * playerUnit2.damage);
        isDead = bossUnit.CheckDead();
        bossHUD.SetHP(bossUnit.currentHP);
        Debug.Log("Boss: " + bossUnit.currentHP);
        CheckDead(isDead);
        playerPrefab2.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(2f);

        // Player 3 Attacks
        dmgMultipler = 1;
        switch (playerUnit3.GetAttack())
        {
            // Offensive
            case 1:
                if (bossUnit.GetStance() == 2)
                {
                    if (bossUnit.GetWeakness() == playerUnit3.GetElement()) // Super Effective Attack on Defensive Stance
                    {
                        dmgMultipler = 1;
                        dialogueText.text = "The Earth Wizard's attack PENETRATES THE BOSS'S DEFENSES!";
                    }
                    else // Effective / Not Very Effective Attack on Defensive Stance
                    {
                        dmgMultipler = 0;
                        dialogueText.text = "The Earth Wizard's attack BOUNCES OFF THE BOSS'S DEFENSES...";
                    }
                }
                else if (bossUnit.GetStance() != 2)
                {
                    if (bossUnit.GetWeakness() == playerUnit3.GetElement()) // Super Effective Attack
                    {
                        dmgMultipler = 2;
                        dialogueText.text = "The Earth Wizard's attack was SUPER EFFECTIVE!";
                    }
                    else if (bossUnit.GetElement() == playerUnit3.GetElement()) // Effective Attack
                    {
                        dmgMultipler = 1;
                        dialogueText.text = "The Earth Wizard's attack was EFFECTIVE.";
                    }
                    else if (bossUnit.GetElement() == playerUnit3.GetWeakness()) // Not Very Effective Atack
                    {
                        dmgMultipler = (float) 0.5;
                        dialogueText.text = "The Earth Wizard's attack was NOT VERY EFFECTIVE...";
                    }
                }

                if (playerUnit3.atkBuff == true)
                {
                    dmgMultipler *= 2;
                    playerUnit1.atkBuff = false;
                    playerUnit1.buffActive = 0;
                }
                break;
            // Defensive
            case 2:
                dmgMultipler = 0;
                dialogueText.text = "The Earth Wizard's conjures a ROCK SHIELD!";
                break;
            // AtkBuff
            case 3:
                dmgMultipler = 0;
                dialogueText.text = "The Earth Wizard's casts a ATTACK BUFF!";
                playerUnit3.atkBuff = true;
                playerUnit3.buffActive = 1;
                break;
            // DefBuff
            case 4:
                dmgMultipler = 0;
                dialogueText.text = "The Flame Wizard's casts a DEFENSE BUFF!";
                playerUnit3.defBuff = true;
                playerUnit3.buffActive = 1;
                break;
            // NULL (Dead)
            default:
                dmgMultipler = 0;
                break;
        }
        Debug.Log("Dmg Mult P3: " + dmgMultipler);
        bossUnit.TakeDamage(rhythmSystem.score * dmgMultipler * playerUnit3.damage);
        isDead = bossUnit.CheckDead();
        bossHUD.SetHP(bossUnit.currentHP);
        Debug.Log("Boss: " + bossUnit.currentHP);
        CheckDead(isDead);
        playerPrefab3.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
    }

    // EnemyTurn - The Enemy's Actions Execute
    IEnumerator EnemyTurn()
    {
        
        int currentStance = bossUnit.GetStance();
        int currentElement = bossUnit.GetElement();
        string trigger = "BossTo"; 
        if(currentElement == 0){
            trigger += "Fire";
        } else if(currentElement == 1){
            trigger += "Water";
        } else {
            trigger += "Earth";
        }

        if (currentStance == 1){
            trigger+="Offense";
        } else if(currentStance == 2){
            trigger+="Defense";
        }
        Debug.Log("trigger = " + trigger);
        bossAnimator.SetTrigger(trigger);
        

        float dmgMultipler = 1;
        switch (bossUnit.GetStance())
        {
            case 1: // Offensive
                dmgMultipler = 1;
                if (playerUnit1.CheckDead() == false)
                { 
                    if (playerUnit1.GetAttack() == 2)
                    {
                        if (playerUnit1.defBuff == true)
                        {
                            dialogueText.text = "The Boss is unable to penetrate the buffed fire defense";
                            dmgMultipler = 0;
                            playerUnit1.defBuff = false;
                            playerUnit1.buffActive = 0;
                        }
                        else if (bossUnit.GetElement() == playerUnit1.GetWeakness())
                        {
                            dialogueText.text = "The Boss penetrates fire defense";
                            dmgMultipler = 1;
                        } else
                        {
                            dialogueText.text = "The Boss attacks fire shield";
                            dmgMultipler = (float)0.5;
                        }
                    } else
                    {
                        if (bossUnit.GetElement() == playerUnit1.GetWeakness())
                        {
                            dialogueText.text = "The Boss critically hits the fire wizard";
                            dmgMultipler = 2;
                        }
                        else
                        {
                            dialogueText.text = "The Boss hits the fire wizard";
                            dmgMultipler = 1;
                        }
                    }
                    playerUnit1.TakeDamage(dmgMultipler * bossUnit.damage);
                    yield return new WaitForSeconds(2f);
                }

                dmgMultipler = 1;
                if (playerUnit2.CheckDead() == false)
                {
                    if (playerUnit2.GetAttack() == 2)
                    {
                        if (playerUnit2.defBuff == true)
                        {
                            dialogueText.text = "The Boss is unable to penetrate the buffed water defense";
                            dmgMultipler = 0;
                            playerUnit2.defBuff = false;
                            playerUnit2.buffActive = 0;
                        }
                        else if (bossUnit.GetElement() == playerUnit2.GetWeakness())
                        {
                            dialogueText.text = "The Boss penetrates water defense";
                            dmgMultipler = 1;
                        }
                        else
                        {
                            dialogueText.text = "The Boss attacks water shield";
                            dmgMultipler = (float)0.5;
                        }
                    }
                    else
                    {
                        if (bossUnit.GetElement() == playerUnit2.GetWeakness())
                        {
                            dialogueText.text = "The Boss critically hits the water wizard";
                            dmgMultipler = 2;
                        }
                        else
                        {
                            dialogueText.text = "The Boss hits the water wizard";
                            dmgMultipler = 1;
                        }
                    }
                    playerUnit2.TakeDamage(dmgMultipler * bossUnit.damage);
                    yield return new WaitForSeconds(2f);
                }

                dmgMultipler = 1;
                if (playerUnit3.CheckDead() == false)
                {
                    if (playerUnit3.GetAttack() == 2)
                    {
                        if (playerUnit1.defBuff == true)
                        {
                            dialogueText.text = "The Boss is unable to penetrate the buffed earth defense";
                            dmgMultipler = 0;
                            playerUnit1.defBuff = false;
                            playerUnit1.buffActive = 0;
                        }
                        else if (bossUnit.GetElement() == playerUnit3.GetWeakness())
                        {
                            dialogueText.text = "The Boss penetrates earth defense";
                            dmgMultipler = 1;
                        }
                        else
                        {
                            dialogueText.text = "The Boss attacks earth shield";
                            dmgMultipler = (float)0.5;
                        }
                    }
                    else
                    {
                        if (bossUnit.GetElement() == playerUnit3.GetWeakness())
                        {
                            dialogueText.text = "The Boss critically hits the earth wizard";
                            dmgMultipler = 2;
                        }
                        else
                        {
                            dialogueText.text = "The Boss hits the earth wizard";
                            dmgMultipler = 1;
                        }
                    }
                    playerUnit3.TakeDamage(dmgMultipler * bossUnit.damage);
                    yield return new WaitForSeconds(2f);
                }
                break;
            default: // Neutral and Defensive

                int target = Random.Range(1, 4);
                bool validTarget = true;
                switch (target)
                {
                    case 1:
                        if (playerUnit1.CheckDead() == true) { validTarget = false; }
                        break;
                    case 2:
                        if (playerUnit2.CheckDead() == true) { validTarget = false; }
                        break;
                    case 3:
                        if (playerUnit3.CheckDead() == true) { validTarget = false; }
                        break;
                }
                while (validTarget == false)
                {
                    target = Random.Range(1, 4);
                    validTarget = true;
                    switch (target)
                    {
                        case 1:
                            if (playerUnit1.CheckDead() == true) { validTarget = false; }
                            break;
                        case 2:
                            if (playerUnit2.CheckDead() == true) { validTarget = false; }
                            break;
                        case 3:
                            if (playerUnit3.CheckDead() == true) { validTarget = false; }
                            break;
                    }
                }

                dmgMultipler = 1;
                switch (target)
                {
                    case 1:
                        if (playerUnit1.GetAttack() == 2)
                        {
                            if (playerUnit1.defBuff == true)
                            {
                                dialogueText.text = "The Boss is unable to penetrate the buffed fire defense";
                                dmgMultipler = 0;
                                playerUnit1.defBuff = false;
                                playerUnit1.buffActive = 0;
                            }
                            else if (bossUnit.GetElement() == playerUnit1.GetWeakness())
                            {
                                dialogueText.text = "The Boss penetrates fire defense";
                                dmgMultipler = 1;
                            }
                            else
                            {
                                dialogueText.text = "The Boss attacks fire shield";
                                dmgMultipler = (float) 0.5;
                            }
                        }
                        else
                        {
                            if (bossUnit.GetElement() == playerUnit1.GetWeakness())
                            {
                                dialogueText.text = "The Boss critically hits the fire wizard";
                                dmgMultipler = 2; 
                            }
                            else
                            {
                                dialogueText.text = "The Boss hits the fire wizard";
                                dmgMultipler = 1;
                            }
                        }
                        playerUnit1.TakeDamage(dmgMultipler * bossUnit.damage);
                        yield return new WaitForSeconds(2f);
                        break;
                    case 2:
                        if (playerUnit2.GetAttack() == 2)
                        {
                            if (playerUnit2.defBuff == true)
                            {
                                dialogueText.text = "The Boss is unable to penetrate the buffed water defense";
                                dmgMultipler = 0;
                                playerUnit2.defBuff = false;
                                playerUnit2.buffActive = 0;
                            }
                            else if (bossUnit.GetElement() == playerUnit2.GetWeakness())
                            {
                                dialogueText.text = "The Boss penetrates water defense";
                                dmgMultipler = 1;
                            }
                            else
                            {
                                dialogueText.text = "The Boss attacks water shield";
                                dmgMultipler = (float) 0.5;
                            }
                        }
                        else
                        {
                            if (bossUnit.GetElement() == playerUnit2.GetWeakness())
                            {
                                dialogueText.text = "The Boss critically hits the water wizard";
                                dmgMultipler = 2;
                            }
                            else
                            {
                                dialogueText.text = "The Boss hits the water wizard";
                                dmgMultipler = 1;
                            }
                        }
                        playerUnit2.TakeDamage(dmgMultipler * bossUnit.damage);
                        yield return new WaitForSeconds(2f);
                        break;
                    case 3:
                        if (playerUnit3.GetAttack() == 2)
                        {
                            if (playerUnit3.defBuff == true)
                            {
                                dialogueText.text = "The Boss is unable to penetrate the buffed fire defense";
                                dmgMultipler = 0;
                                playerUnit3.defBuff = false;
                                playerUnit3.buffActive = 0;
                            }
                            else if (bossUnit.GetElement() == playerUnit3.GetWeakness())
                            {
                                dialogueText.text = "The Boss penetrates earth defense";
                                dmgMultipler = 1;
                            }
                            else
                            {
                                dialogueText.text = "The Boss attacks earth shield";
                                dmgMultipler = (float) 0.5;
                            }
                        }
                        else
                        {
                            if (bossUnit.GetElement() == playerUnit3.GetWeakness())
                            {
                                dialogueText.text = "The Boss critically hits the earth wizard";
                                dmgMultipler = 2;
                            }
                            else
                            {
                                dialogueText.text = "The Boss hits the earth wizard";
                                dmgMultipler = 1;
                            }
                        }
                        playerUnit3.TakeDamage(dmgMultipler * bossUnit.damage);
                        yield return new WaitForSeconds(2f);
                        break;
                }
                break;
        }

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
        
        

        ResetRound();
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

    void ResetRound()
    {
        playerUnit1.WizardReset();
        playerUnit2.WizardReset();
        playerUnit3.WizardReset();
    }

    // EndBattle - Displays the Appropriate Message for Win or Lose Condition
    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            SceneManager.LoadScene("VictoryScene");
            dialogueText.text = "You win!";
        } else if (state == BattleState.LOST)
        {
            SceneManager.LoadScene("DefeatScene");
            dialogueText.text = "You lose!";
        }
    }
}
