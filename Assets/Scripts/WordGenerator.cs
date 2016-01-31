using UnityEngine;
using System.Collections.Generic;

public class WordGenerator {
    //List of individual words
    private static List<string> wordList = new List<string>() { "vitae", "sit", "suscipit", "nibh", "penatibus", "porttitor", "bibendum", "est", "Etiam", "tortor", "cubilia", "viverra", "nisi", "pulvinar", "nisl", "dictumstAenean", "ullamcorper", "luctus", "condimentum", "pharetra", "odioCurabitur", "odio", "habitasse", "consectetur", "montes", "dapibus", "enim", "diam", "commodo", "malesuada", "felis", "justo", "Nulla", "Aenean", "nunc", "sollicitudin", "primis", "iaculis", "placerat", "tristique", "eros", "lobortis", "ridiculus", "eleifend", "velit", "dolorProin", "Fusce", "lorem", "turpis", "faucibus", "Nullam", "Vestibulum", "Cras", "ipsum", "Phasellus", "dolor", "amet", "accumsan", "Integer", "Cum", "rhoncus", "accumsanProin", "fringilla", "aliquam", "feugiat", "Pellentesque", "augue", "musCurabitur", "orci", "nuncMaecenas", "Donec", "erat", "ante", "Mauris", "lectus", "molestie", "massa", "gravida", "vestibulum", "nec", "congue", "natoque", "quis", "leo", "quam", "mattis", "magna", "nequeUt", "Sed", "tempus", "maximus", "laoreet", "cursus", "acVestibulum", "pellentesque", "magnis", "parturient", "sapien", "dignissim", "mollis", "eget", "sodales", "Maecenas", "dui", "ultricies", "fermentumNullam", "interdum", "quisProin", "aliquamSed", "mauris", "blandit", "imperdiet", "tellus", "vehicula", "elementum", "dictum", "elit", "sem", "efficitur", "posuere", "ornare", "tincidunt", "sed", "sagittis", "vel", "Curae;", "hac", "libero", "pretium", "dis", "arcu", "auctor", "Vivamus", "facilisis", "pellentesqueProin", "non", "fermentum", "risus", "nulla", "aliquet", "Lorem", "aProin", "Proin", "lacinia", "sociis", "varius", "metus", "ultrices", "Praesent", "volutpat", "platea", "viverraIn", "ultriciesDuis", "tempor", "risusFusce", "idPraesent", "adipiscing", "vulputate", "nascetur", "porta", "consequat", "convallis", "facilisisAenean", "egestas", "rutrum", "Nunc", "venenatis" };

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
			wordCount = Mathf.Max(7, ServerNetworker.Instance.connectedPlayers.Count * 4);
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
