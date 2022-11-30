using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour
{
  


    public List<Words> wordList = new List<Words>();
    
    public static WordManager instance;

    void Awake()
    {

        instance = this;
         //probably pretty inefficient
        foreach(Words word in wordList)
        {
            word.Notes = new float[CountSyllables(word.word)];
            int x = CountSyllables(word.word);
            for(int i = 0; i < x; i++){
                //variable for if we want to add a more in depeth note duration generation
                float a = 1.5f;
                word.Notes[i] = a;
            }
        }
    }




    
    //Estimates the number of syllables based off of the vowels
    //Not completely accurate, but we can mitigate this by only including words where this is true.
    private int CountSyllables(string word)
    {
        char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'y' };
        string currentWord = word;
        int numVowels = 0;
        bool lastWasVowel = false;
        foreach (char wc in currentWord)
        {
            bool foundVowel = false;
            foreach (char v in vowels)
            {
                //don't count diphthongs
                if (v == wc && lastWasVowel)
                {
                    foundVowel = true;
                    lastWasVowel = true;
                    break;
                }
                else if (v == wc && !lastWasVowel)
                {
                    numVowels++;
                    foundVowel = true;
                    lastWasVowel = true;
                    break;
                }
            }

            //if full cycle and no vowel found, set lastWasVowel to false;
            if (!foundVowel)
                lastWasVowel = false;
        }
        //remove es, it's _usually? silent
        if (currentWord.Length > 2 && 
            currentWord.Substring(currentWord.Length - 2) == "es")
            numVowels--;
        // remove silent e
        else if (currentWord.Length > 1 &&
            currentWord.Substring(currentWord.Length - 1) == "e")
            numVowels--;

        return numVowels;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
