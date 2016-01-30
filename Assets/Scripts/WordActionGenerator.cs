using UnityEngine;
using System.Collections.Generic;

public class WordActionGenerator : MonoBehaviour {

    public enum WordAction { Swipe, Shake, Tap }

    //Get random action for a word
    public static WordAction GetWordAction() {
        var v = WordAction.GetValues(typeof(WordAction));
        return (WordAction)v.GetValue(new System.Random().Next(v.Length));
    }
}
