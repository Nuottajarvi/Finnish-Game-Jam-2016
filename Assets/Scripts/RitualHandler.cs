using UnityEngine;
using System.Collections;

public class RitualHandler {
    static RitualHandler instance;
    public static RitualHandler Instance {
        get {
            if(instance == null) {
                instance = new RitualHandler();
            }
            return instance;
        }
    }


    System.Random random;

    static string[] currentRitual;
    public static string[] CurrentRitual {
        get { return currentRitual; }
    }

    private RitualHandler() {
        random = new System.Random();
    }


    public void NewRitual() {
        int wordCount = random.Next(4, 8);

        currentRitual = new string[wordCount];

        for(int i = 0; i < wordCount; i++) {
            currentRitual[i] = WordGenerator.GetWord();
        }

        GameUI.Instance.SetNewRitualText(currentRitual);
    }
}
