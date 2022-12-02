using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVector : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject target;
    private Vector3 startPos;
    private Vector3 endPos;
    private float targetDir;

    public float attackTime;


    //for setting a particular particle 
    public GameObject particleFab;
    private GameObject particle;


    //find the direction of the target
    //point the particle in that direction
    //make the particle a child of the current game object.




    public static float EaseInCubic(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value + start;
    }


    IEnumerator Attack()
    {
        float time = 0.0f;

        while (time < attackTime)
        {
            time += Time.deltaTime;
            float x = EaseInCubic(startPos.x, endPos.x, time / attackTime);
            float y = EaseInCubic(startPos.y, endPos.y, time / attackTime);

            particle.transform.position = new Vector3(x, y, startPos.z);
            this.transform.position = new Vector3(x, y, startPos.z);
            yield return null;
        }
        Destroy(particle);
        Destroy(this);
        
    }

    //create a trigger idk
    //later instantiate the particle system as a child of the attack sprite
    void Start()
    {
        print("attack vector!!!");
        startPos = this.transform.position;
        endPos = target.GetComponent<Transform>().position;

        targetDir = (Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg) ;



        particle = Instantiate(particleFab, startPos, Quaternion.identity); 

        //this.transform.rotation = Quaternion.Inverse(Quaternion.Euler(0, 0, targetDir));
        transform.rotation = Quaternion.Euler(0, 0, targetDir);
        


        //particle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        StartCoroutine(Attack());
    }


    // Update is called once per frame
    void Update()
    {
        



    }
}
