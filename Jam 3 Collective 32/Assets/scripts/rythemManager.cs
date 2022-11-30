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
        run();
    }

    // Update is called once per frame
    void Update()
    {
        if (realPointer != null)
        {
            realPointer.transform.Translate(new Vector3(stepDist, 0, 0));
            if (realPointer.transform.position.x > Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x)
            {
                Destroy(realPointer);
                foreach(GameObject target in targets)
                {
                    Destroy(target);
                }
                while (targets.Count > 0)
                {
                    targets.RemoveAt(0);
                }
                done = true;

            }
            if (Input.GetMouseButtonDown(0))
            {
                bool didScore = false;
                for(int i = 0; i<dists.Count;i++)
                {
                    if(dists[i] < realPointer.transform.position.x + scoringWidth && dists[i] > realPointer.transform.position.x - scoringWidth)
                    {
                        didScore = true;
                        targets[i].GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.green);
                    }
                }
                if (didScore) { score++; }
                else { score--; }
            }
        }
    }

    void run()
    {
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
