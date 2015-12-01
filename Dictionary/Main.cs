using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryBook;
//using Nodes;
using levisthanDistance;
using ClassTester;

namespace Main
{
    class main
    {
        static void Main(string[] args)
        {

            //Unit Testing
            Test t = new Test();
            t.startTest();

            // Manual Testing
            /*

            //Console.WriteLine("---------------------------------");
            //Console.WriteLine("Distance: "+LevisthanDistance.Compute("dependant", "depindant"));

            Dictionary d = new Dictionary("file.txt");

            string choice = "";

            while (choice != "QUIT")
            {
                /*
                Console.WriteLine("---------------------------------");

                Console.Write("Enter word to look up (Type QUIT to exit):");
                choice = Console.ReadLine();

                Node resultant;
                choice = choice.Trim().ToLower();

                Console.WriteLine("Is the word segment there?:"+d.LookUp(choice,out resultant));
                Console.WriteLine("The segment that was looked up is: " + resultant.getWord());
                Console.WriteLine("Is it an actual Word?: "+ d.isAWord(choice));
                */

            /*
            Console.WriteLine("---------------------------------");

            Console.Write("Enter a similar and longer incorrect word: ");
            string inCorrect = Console.ReadLine();

            HashSet<String> suggestedWords = d.suggestions(inCorrect, resultant);

            Console.WriteLine("List of suggested words:");

            for (int run = 0; run < suggestedWords.Count; run++)
                Console.WriteLine(suggestedWords.ElementAt(run));
            */

            /*
            Console.WriteLine("---------------------------------");

            Console.Write("Enter an incorrect word to see suggestions:");
            choice = Console.ReadLine();

            Console.WriteLine("List of suggested words:");

            HashSet<String> wordSuggestion = d.suggestions(choice);

            for (int run = 0; run < wordSuggestion.Count; run++)
                Console.WriteLine(wordSuggestion.ElementAt(run));
            */

            /*
            Console.WriteLine("---------------------------------");

            Console.Write("Enter a correct word to change popularity:");
            choice = Console.ReadLine();
            d.changeWordPopularity(choice);

            Console.Write("Enter a segment of the previous word to see the most popular word:");
            choice = Console.ReadLine();

            Console.WriteLine("Most popular word for segment " + choice + " is:" + d.getMostPopularWordForSegment(choice));



        }
    */
        }
    
    }

}
