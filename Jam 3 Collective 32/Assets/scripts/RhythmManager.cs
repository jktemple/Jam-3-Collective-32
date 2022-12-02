using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TO use:
//call "wordParse(input)" where input is an array of ints as follows:
//0 - nekki     void
//1 - feldr     fire
//2 - vatn      water
//3 - jord      earth
//4 - ordomr    offensive
//5 - vardVeita defensive
//6 - sarLiga   atk buff
//7 - skjalBor  def buff


public class RhythmManager : MonoBehaviour
{
    //prefabs to instantiate
    public GameObject pointerSprite;
    public GameObject targetSprite;
    public GameObject backgroundLine;
    public GameObject screenBlank;
    public Sprite doneTarget;
    public AudioSource spellNoise;
    //the time between nodes, with the last time being interpretied as the delay after the puzzle
    public List<float> times;
    //returns a score for the run
    public float score = 0;
    //returns false while system is running
    public bool done = true;

    public float scoringWidth = 10f;

    GameObject realLine;
    GameObject realPointer;
    GameObject realBlank;
    //I don't know why but this lists don't work if they are private
    public List<GameObject> targets;
    public List<float> dists;
    float stepDist = 0;
    float worldXwidth;
    Queue<int> wordRef = new Queue<int>();

    //words
    words[] wordlist = new words[9];

    //consts for ease of access
    public const int NEKKINULL = 0;
    public const int FELDRFIRE = 1;
    public const int VATNWATER = 2;
    public const int JORDEARTH = 3;
    public const int ORDOMROFF = 4;
    public const int VARDDEF = 5;
    public const int SARABUFF = 6;
    public const int SKJALDBUFF = 7;


    // Start is called before the first frame update
    void Start()
    {
        //find the width of the viewport in world units
        worldXwidth = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        Debug.Log(worldXwidth);

        //load words (yey data entry)
        loadWord(NEKKINULL,  "wordSounds/nekki",      0.16f, 0.34f);
        loadWord(FELDRFIRE,  "wordSounds/feldr",      0.05f, 0.45f);
        loadWord(VATNWATER,  "wordSounds/vatn",       0.12f, 0.38f);
        loadWord(JORDEARTH,  "wordSounds/jord",       0.18f, 0.32f);
        loadWord(ORDOMROFF,  "wordSounds/ordomr",     0.35f, 0.35f);
        loadWord(VARDDEF,    "wordSounds/vard-veita", 0.22f, 0.38f);
        loadWord(SARABUFF,   "wordSounds/sar-liga",   0.38f, 0.32f);
        loadWord(SKJALDBUFF, "wordSounds/skjal-bor",  0.27f, 0.43f);
        wordlist[8].sound = Resources.Load<AudioClip>("wordSounds/halfSecSil");

        //////////////////////////
        //example of how to call, remove for actual implementation
        /*
        //call with run
        List<float> temp = new List<float>();
        temp.Add(0.5f);
        temp.Add(0.5f);
        temp.Add(0.5f);
        temp.Add(0.5f);
        temp.Add(0.5f);
        temp.Add(0.5f);
        run(temp);
        */
        //call with wordParse
        //int[] testlist = { 0, 1, 2, 3, 4, 5, 6, 7 };
        //wordParse(testlist);
        //End example
        //////////////////////////
    }

    // Update is called once per frame
    void Update()
    {
        //run if pointer exists
        if (realPointer != null)
        {
            runSound();
            //update pointer position
            realPointer.transform.Translate(new Vector3(stepDist * Time.deltaTime, 0, 0));
            //check if pointer is on right edge of screen
            if (realPointer.transform.position.x > Camera.main.ViewportToWorldPoint(new Vector3(0.9f, 0, 0)).x)
            {
                deconstruct();
            }
            //check for clicks on queue
            if (Input.GetMouseButtonDown(0))
            {
                checkScore();
            }
        }
    }

    //runns through the queue of words
    void runSound()
    {
        if (!spellNoise.isPlaying)
        {
            if(wordRef.Count < 1) { return; }
            spellNoise.clip = wordlist[wordRef.Peek()].sound;
            spellNoise.Play();
            wordRef.Dequeue();
        }
    }

    //removes all minigame objects, and fixes score
    void deconstruct()
    {
        //clamp score between 0 and 1
        if (score <= 0) {score = 0;}
        else{score = score / targets.Count;}

        //remove things
        Destroy(realPointer);
        Destroy(realLine);
        Destroy(realBlank);
        while (targets.Count > 0)
        {
            Destroy(targets[0]);
            targets.RemoveAt(0);
            dists.RemoveAt(0);
        }
        while(wordRef.Count > 0)
        {
            wordRef.Dequeue();
        }

        done = true;
    }

    //checks if scoring is currently valid
    void checkScore()
    {
        bool didScore = false;
        for (int i = 0; i < dists.Count; i++)
        {
            //check if scoring range is valid
            if (dists[i] < realPointer.transform.position.x + scoringWidth && dists[i] > realPointer.transform.position.x - scoringWidth)
            {
                //allow score increse
                didScore = true;
                //change color
                targets[i].GetComponentInChildren<SpriteRenderer>().sprite = doneTarget;
            }
        }
        //update score as necessasary
        if (didScore) { score++; }
        else { score--; }
    }

    //runs and updates the wordlist
    public void wordParse(int[] inputArray)
    {
        done = false;
        List<float> outTimes = new List<float>();
        float temp = 1;
        wordRef.Enqueue(8);
        wordRef.Enqueue(8);
        foreach (int wordNum in inputArray)
        {
            wordRef.Enqueue(wordNum);
            outTimes.Add(temp+wordlist[wordNum].timeBefore);
            temp = wordlist[wordNum].timeAfter;
        }
        outTimes.Add(temp);
        run(outTimes);
    }

    //sets up the minigame
    void run(List<float> newTargets)
    {
        times = newTargets;
        score = 0;
        done = false;
        realPointer = Instantiate(pointerSprite, Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.5f, Camera.main.nearClipPlane)), Quaternion.identity);
        pointerSprite.transform.position = (Camera.main.ViewportToWorldPoint(new Vector3(0,0.5f,Camera.main.nearClipPlane)));
        float totalTime = 0;
        foreach(float Time in times)
        {
            totalTime += Time;
        }
        stepDist = worldXwidth * 0.8f / totalTime; //distance to be traveled per second
        Debug.Log(stepDist);
        float acumulatedTime = 0;
        foreach (float Time in times)
        {
            acumulatedTime += Time;
            if (acumulatedTime != totalTime)
            {
                targets.Add((GameObject)Instantiate(targetSprite, Camera.main.ViewportToWorldPoint(new Vector3((acumulatedTime / totalTime)*0.8f+0.1f, 0.5f, Camera.main.nearClipPlane + 0.05f)), Quaternion.identity));
            }
        }
        foreach(GameObject target in targets)
        {
            dists.Add(target.transform.position.x);
        }
        realLine = Instantiate(backgroundLine, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane + 0.1f)), Quaternion.identity);
        realBlank = Instantiate(screenBlank, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane + 0.2f)), Quaternion.identity);
    }

    //bundle word information together
    struct words
    {
        public AudioClip sound;
        public float timeBefore;
        public float timeAfter;
    }

    //data entry helper function to fill word structs
    void loadWord(int Word, string path, float before, float after)
    {
        wordlist[Word].sound = Resources.Load<AudioClip>(path);
        wordlist[Word].timeBefore = before;
        wordlist[Word].timeAfter = after;
    }

}
