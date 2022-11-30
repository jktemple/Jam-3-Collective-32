using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rythemManager : MonoBehaviour
{
    //prefabs to instantiate
    public GameObject pointerSprite;
    public GameObject targetSprite;
    //the time between nodes, with the last time being interpretied as the delay after the puzzle
    public List<float> times;
    //returns a score for the run
    public float score = 0;
    //returns false while system is running
    public bool done = true;

    public float scoringWidth = 0.1f;

    GameObject realPointer;
    //I don't know why but this lists don't work if they are private
    public List<GameObject> targets;
    public List<float> dists;
    float stepDist = 0;

    // Start is called before the first frame update
    void Start()
    {
        //example of how to call, remove for actual implementation
        List<float> temp = new List<float>();
        temp.Add(10);
        temp.Add(5);
        temp.Add(10);
        temp.Add(15);
        temp.Add(3);
        temp.Add(5);
        run(temp);
    }

    // Update is called once per frame
    void Update()
    {
        //run if pointer exists
        if (realPointer != null)
        {
            //update pointer position
            realPointer.transform.Translate(new Vector3(stepDist, 0, 0));
            //check if pointer is on right edge of screen
            if (realPointer.transform.position.x > Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x)
            {
                //remove all pointer and targets, and empty target list
                Destroy(realPointer);
                while (targets.Count > 0)
                {
                    Destroy(targets[0]);
                    targets.RemoveAt(0);
                    dists.RemoveAt(0);
                }
                done = true;

            }
            //check for clicks on queue
            if (Input.GetMouseButtonDown(0))
            {
                bool didScore = false;
                for(int i = 0; i<dists.Count;i++)
                {
                    //check if scoring range is valid
                    if(dists[i] < realPointer.transform.position.x + scoringWidth && dists[i] > realPointer.transform.position.x - scoringWidth)
                    {
                        //allow score increse
                        didScore = true;
                        //change color
                        targets[i].GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.green);
                    }
                }
                //update score as necessasary
                if (didScore) { score++; }
                else { score--; }
            }
        }
    }

    void run(List<float> newTargets)
    {
        times = newTargets;
        score = 0;
        done = false;
        realPointer = Instantiate(pointerSprite, Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, Camera.main.nearClipPlane)), Quaternion.identity);
        pointerSprite.transform.position = (Camera.main.ViewportToWorldPoint(new Vector3(0,0.5f,Camera.main.nearClipPlane)));
        float totalTime = 0;
        foreach(float Time in times)
        {
            totalTime += Time;
        }
        stepDist = 1 / totalTime;
        float acumulatedTime = 0;
        foreach (float Time in times)
        {
            acumulatedTime += Time;
            if (acumulatedTime != totalTime)
            {
                targets.Add((GameObject)Instantiate(targetSprite, Camera.main.ViewportToWorldPoint(new Vector3(acumulatedTime / totalTime, 0.4f, Camera.main.nearClipPlane)), Quaternion.identity));
            }
        }
        foreach(GameObject target in targets)
        {
            dists.Add(target.transform.position.x);
        }
    }
}
