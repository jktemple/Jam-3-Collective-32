using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    public float scoringWidth = 0.1f;

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
    words[] wordlist = new words[8];
    //words[0] nekki;
    //words[1] feldr;
    //words[2] vatn;
    //words[3] jord;
    //words[4] ordomr;
    //words[5] vardVeita;
    //words[6] sarLiga;
    //words[7] skjalBor;

    // Start is called before the first frame update
    void Start()
    {
        //find the width of the viewport in world units
        worldXwidth = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        //load words
        wordlist[0].sound = Resources.Load<AudioClip>("wordSounds/nekki");
        wordlist[0].timeBefore = 0.16f;
        wordlist[0].timeAfter = 0.5f - wordlist[0].timeBefore;
        wordlist[1].sound = Resources.Load<AudioClip>("wordSounds/feldr");
        wordlist[1].timeBefore = 0.05f;
        wordlist[1].timeAfter = 0.5f - wordlist[1].timeBefore;
        wordlist[2].sound = Resources.Load<AudioClip>("wordSounds/vatn");
        wordlist[2].timeBefore = 0.12f;
        wordlist[2].timeAfter = 0.5f - wordlist[2].timeBefore;
        wordlist[3].sound = Resources.Load<AudioClip>("wordSounds/jord");
        wordlist[3].timeBefore = 0.18f;
        wordlist[3].timeAfter = 0.5f - wordlist[3].timeBefore;
        wordlist[4].sound = Resources.Load<AudioClip>("wordSounds/ordomr");
        wordlist[4].timeBefore = 0.35f;
        wordlist[4].timeAfter = 0.7f - wordlist[4].timeBefore;
        wordlist[5].sound = Resources.Load<AudioClip>("wordSounds/vard-veita");
        wordlist[5].timeBefore = 0.22f;
        wordlist[5].timeAfter = 0.5f - wordlist[5].timeBefore;
        wordlist[6].sound = Resources.Load<AudioClip>("wordSounds/sar-liga");
        wordlist[6].timeBefore = 0.38f;
        wordlist[6].timeAfter = 0.7f - wordlist[6].timeBefore;
        wordlist[7].sound = Resources.Load<AudioClip>("wordSounds/skjal-bor");
        wordlist[7].timeBefore = 0.27f;
        wordlist[7].timeAfter = 0.7f - wordlist[7].timeAfter;

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
        int[] testlist = { 0, 1, 2, 3, 4, 5, 6, 7 };
        wordParse(testlist);
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
        //reset test **** TO BE REMOVED FOR IMPLEMENTATION
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                int[] testlist = { 0, 1, 2, 3, 4, 5, 6, 7 };
                wordParse(testlist);
            }
        }
        //////////////////////////
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
        List<float> outTimes = new List<float>();
        float temp = 0;
        foreach(int wordNum in inputArray)
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
    
}
