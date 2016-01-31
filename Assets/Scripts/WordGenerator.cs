using UnityEngine;
using System.Collections.Generic;

public class WordGenerator {
	//List of individual words
	private static List<string> wordList = new List<string>() { "molestie", "pulvinar", "dis", "fdiam", "sagittis", "accumsan", "dictumst", "gravida", "risus", "elit", "condimentum", "dignissim", "metus", "corper", "commodo", "iaculis", "velit", "consectetur", "egestas", "sollicitudin", "faucibus", "eros", "rutrum", "adipiscing", "venenatis", "vel", "vulputate", "libero", "mollis", "maecenas", "ante", "curae", "eget", "tellus", "placerat", "malesuada", "amet", "montes", "turpis", "felis", "lorem", "eleifend", "aliquam", "justo", "blandit", "proin", "viverra", "fermentum", "sodales", "nunc", "vitae", "ipsum", "aenean", "auctor", "natoque", "magna", "cubilia", "facilisis", "luctus", "vivamus", "penatibus", "porttitor", "muscurabitur", "sed", "habitasse", "sem", "arcu", "quis", "volutpat", "nec", "quam", "leo", "tristique", "congue", "maximus", "cras", "parturient", "ultricies", "pharetra", "Fusce", "magnis", "ultrices", "imperdiet", "odio", "enim", "erat", "etiam", "bibendum", "nisl", "integra", "nisi", "dui", "tortor", "nibh", "ornare", "vehicula", "rhoncus", "sociis", "dolor", "pretium", "laoreet", "porta", "accumsane", "elementum", "augue", "mauris", "primis", "ridiculus", "efficitur", "posuere", "dapibus", "tempus", "pellentesque", "massa", "cursus", "sapien", "feugiat", "est", "nascetur", "nulla", "dolora", "dictum", "vestibulum", "cumos", "fringilla", "hac", "lectus", "aliquet", "platea", "interdum", "non", "donec", "tempor", "phasellus", "varius", "nullam", "praesent", "tincidunt", "orci", "lobortis", "mattis", "consequat", "curabitur", "convallis", "sit", "suscipit", "ullam", "neque", "lacinia" };

	private static string[] currentWordPool;
	public static string[] CurrentWordPool {
		get {
			return currentWordPool;
		}
	}

    //Get random word from the list
    public static string GetWord() {
        //Get random word
        string wordToReturn = currentWordPool[GameController.jamRandomer.Next(0, currentWordPool.Length)];

        //Capitalize and return
        return FirstCharToUpper(wordToReturn);
    }

    //Function to capitalize the first letter of given word
    public static string FirstCharToUpper(string s)
    {
        return char.ToUpper(s[0]) + s.Substring(1);
    }

	public static void CreateWordPool() {
		List<string> tempWords = new List<string>(wordList);
		int wordCount = 0;

		if(ServerNetworker.Instance != null) {
			wordCount = ServerNetworker.Instance.connectedPlayers.Count * 4;
		} else {
			wordCount = 7;
		}

		wordCount = Mathf.Min(wordCount, wordList.Count);

		currentWordPool = new string[wordCount];

		for(int i = 0; i < wordCount; i++) {
			string word = wordList[GameController.jamRandomer.Next(wordList.Count)];

			currentWordPool[i] = word;

			tempWords.Remove(word);
		}
	}
}
