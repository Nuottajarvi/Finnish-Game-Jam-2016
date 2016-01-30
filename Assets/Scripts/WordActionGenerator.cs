using UnityEngine;
using System.Collections.Generic;

public class WordActionGenerator : MonoBehaviour {

    public enum WordAction { Swipe, Shake, Tap }

    //Get random action for a word
    public static WordAction GetWordAction() {
		WordAction[] actions = System.Enum.GetValues(typeof(WordAction)) as WordAction[];

		return actions[GameController.jamRandomer.Next(0, actions.Length)];
    }
}
