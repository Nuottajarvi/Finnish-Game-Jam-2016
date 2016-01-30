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

    static System.Random rand = new System.Random();

    //Get random word from the list
    public static string GetWord() {
        //Get random word
        string wordToReturn = currentWordPool[rand.Next(0, currentWordPool.Length)];

        //Capitalize and return
        return FirstCharToUpper(wordToReturn);
    }

    //Function to capitalize the first letter of given word
    public static string FirstCharToUpper(string s)
    {
        return char.ToUpper(s[0]) + s.Substring(1);
    }

	public static void CreateWordPool() {
		int wordCount = Mathf.Max(7, ServerNetworker.Instance.connectedPlayers.Count * 4);

		wordCount = Mathf.Min(wordCount, wordList.Count);

		currentWordPool = new string[wordCount];
		
		for(int i = 0; i < wordCount; i++) {
			currentWordPool[i] = wordList[i];
		}
	}

    //Python parsing spell on how to form the wordList if longer is required

    /*test = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean pretium, nulla at pharetra varius, ipsum eros ullamcorper sem, at tempor nulla ante vel lorem. Phasellus porttitor metus orci, venenatis mattis erat placerat quis. Maecenas sapien mi, imperdiet sed ullamcorper vel, fermentum a lectus. Ut mi enim, tempus non placerat a, auctor in nunc. Pellentesque tempus magna nec viverra iaculis. Cras fermentum arcu libero. Sed maximus auctor sagittis. Vivamus accumsan erat eu sollicitudin bibendum. Donec in tristique diam. Integer varius, metus sit amet varius interdum, massa est bibendum nibh, varius dignissim lectus odio et ipsum. Nunc congue felis eget est porttitor consequat. Mauris quis dignissim dolor.Proin massa lectus, placerat et blandit ut, dapibus eu odio.Curabitur aliquet faucibus est id cursus. Nullam sit amet porta ante, non vehicula neque.Ut luctus lorem odio, id maximus leo varius in. Nunc consectetur eu arcu in facilisis.Aenean non diam sed lectus dignissim viverra.In vulputate turpis sapien. Cras sed orci ornare, porta lectus ac, feugiat eros. In porta vehicula augue vel gravida. Etiam lacinia justo eu velit molestie aliquam.Sed pharetra dapibus lorem ac condimentum. Proin venenatis ac ex et feugiat. Phasellus dignissim, diam quis viverra sagittis, libero dolor vestibulum felis, at pellentesque quam orci ut enim. Nunc pretium, odio vitae ultrices dignissim, ex nisi auctor nunc, nec interdum sapien ex vel dui. In rhoncus, ex a dapibus volutpat, orci eros aliquet sapien, vitae ultricies tortor turpis eu justo. Proin sed fermentum nulla. In hac habitasse platea dictumst.Aenean lobortis rhoncus bibendum. Ut facilisis erat eu risus volutpat, sit amet efficitur libero pellentesque.Proin velit tellus, vulputate id elementum vel, molestie at risus.Fusce mollis enim nunc, nec ornare dolor dapibus a.Proin tristique metus et eros venenatis, nec sagittis metus tempus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Etiam porta vestibulum ex ac dapibus. Integer vestibulum nec massa sed fermentum.Nullam ac lorem at ipsum tincidunt tempor ac nec sem. Vivamus faucibus est justo, vitae sodales nulla venenatis quis.Proin porta tortor vehicula erat faucibus, vitae tempor elit aliquam. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Curabitur suscipit, sem quis pulvinar gravida, odio magna iaculis sapien, et tincidunt sapien enim eu arcu. Pellentesque tincidunt mauris id metus elementum, ut fringilla enim consectetur. Donec placerat nibh vel eros vehicula, sed aliquam massa consequat. Sed eleifend nulla eget augue vestibulum, sit amet luctus turpis ultricies.Duis luctus convallis velit, nec varius turpis eleifend id.Praesent ut mi non risus imperdiet accumsan.Proin egestas elit sed mauris malesuada, efficitur dictum ante consectetur. Maecenas vitae ante condimentum, egestas magna sed, tincidunt elit. Vivamus ac pretium diam. Pellentesque molestie posuere mauris a auctor. Sed feugiat eros diam, vitae laoreet felis faucibus ac.Vestibulum non accumsan tellus. Nulla sodales dignissim diam, ac egestas sem sollicitudin in. Fusce a dolor vel leo congue commodo vitae non sapien. Integer malesuada ante non dolor lacinia, id viverra sapien tempor. Vivamus lacinia condimentum velit quis iaculis. In in lorem ut nisi pellentesque dictum et faucibus augue. Praesent id facilisis dolor, sed facilisis nunc.Maecenas pretium nisl eget rutrum maximus. In suscipit, sapien eu ornare consectetur, leo dui ullamcorper sem, vel interdum metus nisi eget orci. Nunc sit amet egestas mi."
    test = test.replace(",", "")
    test = test.replace(".", "")
    testList = test.split(" ")
    testList = list(set(testList))
    testList = [word for word in testList if len(word) >= 3 ]*/


}
