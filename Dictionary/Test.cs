using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DictionaryBook;
//using Nodes;


namespace ClassTester
{
    class Test
    {

        Dictionary d;

        //For testing lookUp and isAWord
        /******************/
        String[] correctSegments;
        String[] InCorrectSegments;
        String[] CorrectWords;
        String[] inCorrectWords;
        Dictionary<String, bool> whethersegmentisAlsoAWord;
        /******************/

        //For testing mostpopularword
        /*****************/
        String[] frequentWords;
        String[] listOfSegments;
        Dictionary<String,String> frequencyAnswers; 
        /*****************/

        public Test()
        {

            //The following four lists should have matching segments and words at the same index ideally

            /***************************************/
            this.correctSegments = new String[20]
                {"foo", "garb","accom","acknowle","argue","comit","deduct","depi","exist","forwor",
                    "herr","inadv","judgem","ocu","paro","superi","abla","brav","hub","cooi"};
            /***************************************/

            /***************************************/
            this.InCorrectSegments = new String[20]
                {"foob","garbga","accomood","acknowleg","arguem","comitm","deducta","depinda","existan","forword",
                    "herras","inadvarte","judgeman","ocurr","parogat","supar","ablabc","brafcd","hubz","cooibn" };
            /***************************************/

            /***************************************/
            this.CorrectWords = new String[20]
                {"food", "garbage","accommodate","acknowledge","arguers","comitia","deductible","depict","existed","forworn",
                    "herring","inadvertent","judgement","ocular","parody","super","ablaze","bravo","hubby","cooing"};
            /***************************************/

            /***************************************/
            this.inCorrectWords = new String[20]
                {"foobar","garbgae","accomodate","acknowlegement","arguemint","comitmment","deductabel","depindant","existanse","forworde",
                     "herrass","inadvartent","judgemant","ocurrance","parady","superas","ablize","bravfi","hubay","cooibng"};
            /***************************************/

            //Sometimes a segment is also a word

            /***************************************/
            this.whethersegmentisAlsoAWord = new Dictionary<string, bool>();

            for (int run = 0; run < this.correctSegments.Count(); run++)
                this.whethersegmentisAlsoAWord.Add(this.correctSegments[run], false);

            this.whethersegmentisAlsoAWord["argue"] = true;
            this.whethersegmentisAlsoAWord["deduct"] = true;
            this.whethersegmentisAlsoAWord["exist"] = true;
            this.whethersegmentisAlsoAWord["garb"] = true;
            this.whethersegmentisAlsoAWord["hub"] = true;
            /***************************************/

            // Run this list in changePopularity function

            /***************************************/
            this.frequentWords = new String[]
                {"for","for","form","form","form","format","format","formatting","axes","axile","axilla",
                    "axilla","bawd","bawd","bawd","bawd","bawdier","bawdier","bawdier","bawdiest"};
            /***************************************/

            //Strings in listOfSegments have to exist as key in frequencyAnswers

            /***************************************/
            this.listOfSegments = new String[]
            {"fo","for","form","forma","formatt","ax","axe","axi","axil","axile","baw","bawd","bawdie","bawdies","bawdiest" };
            /***************************************/

            // Values in frequency have to determined from frequencyWords array
            //Note: Remember that popularity of a word depends on how many times it has been used

            /***************************************/
            this.frequencyAnswers = new Dictionary<string, string>();

            this.frequencyAnswers.Add("fo","form");
            this.frequencyAnswers.Add("for", "form");
            this.frequencyAnswers.Add("form", "form");
            this.frequencyAnswers.Add("forma", "format");
            this.frequencyAnswers.Add("formatt", "formatting");
            this.frequencyAnswers.Add("ax", "axilla");
            this.frequencyAnswers.Add("axe", "axes");
            this.frequencyAnswers.Add("axi", "axilla");
            this.frequencyAnswers.Add("axil","axilla");
            this.frequencyAnswers.Add("axile", "axile");
            this.frequencyAnswers.Add("baw", "bawd");
            this.frequencyAnswers.Add("bawd", "bawd");
            this.frequencyAnswers.Add("bawdie", "bawdier");
            this.frequencyAnswers.Add("bawdies","bawdiest");
            this.frequencyAnswers.Add("bawdiest", "bawdiest");
            /***************************************/


            Console.Write("Type in file name:");
            string file=Console.ReadLine();

            this.d = new Dictionary(file);

            // Changing popularity of words
            for (int run = 0; run < this.frequentWords.Count(); run++)
                this.d.changeWordPopularity(this.frequentWords[run]);


            Console.WriteLine("++++++++++++++++++++++++++++++++");

        }

        public void testPopularity()
        {
            int testFailed = 0;
            int totalTests = 0;

            for (int run=0;run<this.listOfSegments.Count();run++)
            {
                ++totalTests;

                try
                {
                    string segment = this.listOfSegments[run];
                    Assert.AreEqual(this.frequencyAnswers[segment], this.d.getMostPopularWordForSegment(segment), "Failed for segment: "+segment);
                }
                catch(AssertionException e)
                {
                    ++testFailed;
                    Console.WriteLine(e.Message);
                }
            }

            if (testFailed == 0)
                Console.WriteLine("CONGRATULATIONS!... getMostWordPopularForSegment function in dictionary passed all "+totalTests+" tests!");
            else
                Console.WriteLine("Total number of tests failed: " + testFailed + " out of: " + totalTests);
        }


        public void LookUpFunctionTest()
        {
            int testFailed = 0;
            int totalTests = 0;

            for(int run=0;run<correctSegments.Count();run++)
            {
                ++totalTests;

                try
                {
                    Assert.IsTrue(d.LookUp(correctSegments[run]),"Failed at input: "+correctSegments[run]);
                }
                catch(AssertionException e)
                {
                    ++testFailed;

                    Console.WriteLine(e.Message);
                }
            }

            for (int run = 0; run < InCorrectSegments.Count(); run++)
            {
                ++totalTests;

                try
                {
                    Assert.IsFalse(d.LookUp(InCorrectSegments[run]), "Failed at input: " + InCorrectSegments[run]);
                }
                catch (AssertionException e)
                {
                    ++testFailed;

                    Console.WriteLine(e.Message);
                }
            }

            for (int run = 0; run < CorrectWords.Count(); run++)
            {
                ++totalTests;

                try
                {
                    Assert.IsTrue(d.LookUp(CorrectWords[run]), "Failed at input: " + CorrectWords[run]);
                }
                catch (AssertionException e)
                {
                    ++testFailed;

                    Console.WriteLine(e.Message);
                }
            }

            
            for (int run = 0; run < inCorrectWords.Count(); run++)
            {
                ++totalTests;

                try
                {
                    Assert.IsFalse(d.LookUp(inCorrectWords[run]), "Failed at input: " + inCorrectWords[run]);
                }
                catch (AssertionException e)
                {
                    ++testFailed;

                    Console.WriteLine(e.Message);
                }
            }
            

            if (testFailed==0)
                Console.WriteLine("CONGRATULATIONS!... LookUp function in dictionary passed all " + totalTests + " tests!");
            else
                Console.WriteLine("Total number of tests failed: "+testFailed+" out of: "+totalTests);
        }

        public void testisAWordFunction()
        {
            int testFailed = 0;
            int totalTests = 0;

            for (int run = 0; run < inCorrectWords.Count(); run++)
            {
                ++totalTests;

                try
                {
                    string currentWord = inCorrectWords[run];

                    Assert.IsFalse(d.isAWord(currentWord), "Failed with parameter STRING at input: " + currentWord);
                }
                catch (AssertionException e)
                {
                    ++testFailed;
                    Console.WriteLine(e.Message);
                }
            }


            for (int run = 0; run < CorrectWords.Count(); run++)
            {
                ++totalTests;

                try
                {
                    string currentWord = CorrectWords[run];

                    Assert.IsTrue(d.isAWord(currentWord), "Failed with parameter STRING at input: " + currentWord);
                }
                catch (AssertionException e)
                {
                    ++testFailed;
                    Console.WriteLine(e.Message);
                }
            }

            for (int run = 0; run < InCorrectSegments.Count(); run++)
            {
                ++totalTests;

                try
                {
                    string currentWord = InCorrectSegments[run];

                    Assert.IsFalse(d.isAWord(currentWord), "Failed with parameter STRING at input: " + currentWord);
                }
                catch (AssertionException e)
                {
                    ++testFailed;
                    Console.WriteLine(e.Message);
                }
            }

            for (int run = 0; run < correctSegments.Count(); run++)
            {
                ++totalTests;
                string currentWord="";

                try
                {
                    currentWord = correctSegments[run];

                    Assert.IsFalse(d.isAWord(currentWord), "Failed with parameter STRING at input: " + currentWord);
                }
                catch (AssertionException e)
                {
                    bool value;
                    this.whethersegmentisAlsoAWord.TryGetValue(currentWord,out value);

                    if (!value)
                    {
                        ++testFailed;
                        Console.WriteLine(e.Message);
                    }
                }
            }

            if (testFailed == 0)
                Console.WriteLine("CONGRATULATIONS!... LookUp function in dictionary passed all " + totalTests + " tests!");
            else
                Console.WriteLine("Total number of tests failed: " + testFailed + " out of: " + totalTests);

        }

        public void startTest()
        {
            Console.WriteLine("\n-------------------Testing lookUp function----------------------\n");
            this.LookUpFunctionTest();
            Console.WriteLine("\n-------------------Testing isAWord function----------------------\n");
            this.testisAWordFunction();
            Console.WriteLine("\n-------------------Testing wordPopularity function----------------------\n");
            this.testPopularity();
        }

    }


}
