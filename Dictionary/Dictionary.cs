using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Nodes;
using levisthanDistance;

namespace DictionaryBook
{
    class Dictionary
    {


        // A node is just a segment of letters.It can or can't be a word
        // Note: This class can't be tested directly
        private class Node
        {
            //The string of letters passed
            private string Data;
            //Whether the segment of words is word that is in the list
            private bool isAWord;
            /*
            The most easiest way to explain this is with an example.
            If Data=accomplic , it's children would be accomplice,accomplices
            */
            private Dictionary<String, Node> Children;
            //This contains edit Distance as key and list of strings which have the same distance from Data
            private Dictionary<int, List<String>> SimilarWords;
            //This list contains all the possible matching words and the frequency they have been used  
            private Dictionary<String, int> Popularity;

            //letter is segment of words.status is whether the segment is an actual word.By default it is false
            public Node(string letter, bool status = false)
            {
                //Basic validation
                if (letter.Length != 0)
                {
                    this.Data = letter;
                    Children = new Dictionary<String, Node>();
                    SimilarWords = new Dictionary<int, List<String>>();
                    this.Popularity = new Dictionary<string, int>();
                }
                else
                {
                    this.Data = null;
                    this.Children = null;
                    this.SimilarWords = null;
                    this.Popularity = null;
                }

                this.isAWord = status;
            }

            //This function adds the current string in the Node
            public Boolean add(string current, string word)
            {
                //Basic validation
                if (current == null || current.Length == 0 || word == null || word.Length == 0)
                    return false;

                else
                {

                    //Gets the edit distance between the current segment and the complete word
                    int editDistance = LevisthanDistance.Compute(current, word);

                    //If editDistance already is in the Dictionary of SimilarWords
                    if (this.SimilarWords.ContainsKey(editDistance))
                    {
                        //Get the listOfWords for the editDistance and add the complete word in the list
                        List<String> listOfWords;
                        this.SimilarWords.TryGetValue(editDistance, out listOfWords);
                        listOfWords.Add(word);
                    }
                    else
                    {
                        //Create a new list, add the word and then add the distance and the list in the Dictionary
                        List<String> newList = new List<String>();
                        newList.Add(word);
                        this.SimilarWords.Add(editDistance, newList);
                    }

                    //Initially when added no word has been used yet
                    this.Popularity.Add(word, 0);

                    //if the current segment of letters is not already a child
                    if (!this.Children.ContainsKey(current))
                    {
                        //If the current segment has become equal to the current word, wordstatus = true otherwise false
                        if ((current).Equals(word))
                        {
                            this.Children.Add(current, new Node(current, true));
                        }
                        else
                            this.Children.Add(current, new Node(current));
                    }
                    //If the child exists.
                    else
                    {
                        //If it is word then change the wors status to true
                        /*
                        This if statement allows me to add words from file in any order as long as the first letter
                        remains the same
                        */
                        if (current.Equals(word))
                        {
                            Node resultNode;
                            this.Children.TryGetValue(current, out resultNode);
                            resultNode.changeWordStatus(true);
                        }
                    }

                    //If the word already exists as a child
                    //Note: This marks the end of the function. i.e base case
                    if (this.Children.ContainsKey(word))
                    {
                        return true;
                    }
                    else
                    {
                        /*
                        resultNode is the node of the current segment of letters
                        For e.g: current=accom word=accommodate then at this point the first parameter being passed
                        in this recursive call is accomm 
                        */
                        Node resultNode;
                        this.Children.TryGetValue(current, out resultNode);
                        return resultNode.add(current + word.Substring(current.Length, 1), word);
                    }

                }
            }

            //Returns list of words for a given editDistance from Data
            public List<String> getListOfSimilarWords(int editDistance)
            {
                if (this.SimilarWords.ContainsKey(editDistance))
                {
                    List<String> outcome;
                    this.SimilarWords.TryGetValue(editDistance, out outcome);
                    return outcome;
                }
                else
                {
                    return new List<String>();
                }
            }

            //Increase the frequency of the word being used by 1
            // Note: Make sure word exists
            public void increaseWordPopularity(string word)
            {
                int curValue;
                this.Popularity.TryGetValue(word, out curValue);

                this.Popularity[word] = ++curValue;
            }

            //Get the most popular word used by the user for a Node
            //For example if format has been used 2 times and form 3 times,for node "for" most popular word is form 
            public string getMostPopularWord()
            {
                return Popularity.Where(kvp => kvp.Value == Popularity.Values.Max()).Select(kvp => kvp.Key).FirstOrDefault();
            }

            //Checks whether the Dictionary has a certain edit distance
            public bool hasEditDistance(int key)
            {
                return this.SimilarWords.ContainsKey(key);
            }

            //Returns the dictionary of Similar Words
            public Dictionary<int, List<String>> getAllDistances()
            {
                return this.SimilarWords;
            }

            //Gets the maximum edit distance from Data to any number of given words
            public int maxDistance()
            {
                return this.SimilarWords.Keys.Max();
            }

            //Gets the minimum edit distance from Data to any number of given words
            public int minDistance()
            {
                return this.SimilarWords.Keys.Min();
            }

            //Checks whether Data is a word or not
            public bool wordStatus()
            {
                return this.isAWord;
            }

            //Changes the status of a word of whether it is an actual word
            private void changeWordStatus(bool status)
            {
                this.isAWord = status;
            }

            //Gets Data
            public string getWord()
            {
                return this.Data;
            }

            //Returns the Node of the current string from the data structure 
            public Node search(string current, string word)
            {
                //Basic validation
                if (current == null || current.Length == 0 || word == null || word.Length == 0)
                    return new Node(" ");
                else
                {
                    // If the child of the current segment contains the word
                    if (this.Children.ContainsKey(word))
                    {
                        Node resultNode;
                        this.Children.TryGetValue(word, out resultNode);

                        return resultNode;
                    }
                    else
                    {
                        /*
                        For example if accommodate doesn't exists as a child node of accom (which it won't)
                        than do a recursive call where current now will be accomm and word=accommodate
                        */
                        if (this.Children.ContainsKey(current + word.Substring(current.Length, 1)))
                        {
                            Node resultNode;
                            this.Children.TryGetValue(current + word.Substring(current.Length, 1), out resultNode);

                            return resultNode.search(current + word.Substring(current.Length, 1), word);
                        }
                        else
                        {
                            //Returns an empty node let's say if the word was equal to accomodate and current=accom
                            return new Node(" ");
                        }
                    }

                }

            }

        }


        //Dictionary class starts from here 

        // The dictionary holds alphabetical letters as keys and all the words starting from that letter in the node as values
        private Dictionary<String, Node> Alphabets;

        // Constructor just takes the filename
        public Dictionary(String filename)
        {

            if (filename!=null && filename.Length!=0) {

                string line;
                Node currentNode=null;
                string currentLetter="";
                Alphabets = new Dictionary<string, Node>();

                // Read the file and display it line by line.
                try
                {
                    //Reading the file
                    System.IO.StreamReader file = new System.IO.StreamReader(filename);
                    while ((line = file.ReadLine()) != null)
                    {
                        //Removing leading and trailing whitespaces
                        line = line.Trim();

                        //For adding all the words starting with currentLetter. 
                        //Note: Will never go in the first time
                        if (line.Substring(0, 1).Equals(currentLetter))
                        {
                            //Adding the word in the alphabetical node
                            currentNode.add(line.Substring(0, 2), line);
                        }
                        // Program enters else only when currentLetter needs to change .
                        //For e.g: a needs to become b
                        else
                        {
                            // Condition only required the first time program enters the while loop
                            if (currentNode != null)
                                this.Alphabets.Add(currentLetter, currentNode);

                            //Getting the current alphabet and reinitialzing the currentNode for the alphabet
                            currentLetter = line.Substring(0, 1);
                            currentNode = new Node(currentLetter);

                            // Adding the first word for the new selected alphabet in the file
                            currentNode.add(line.Substring(0, 2), line);
                        }

                    }

                    //This condition is required for the very last alphabet in the file e.g: z
                    if (currentNode != null)
                    {
                        this.Alphabets.Add(currentLetter, currentNode);
                    }

                    file.Close();
                }
                // In case file is not found
                catch (System.IO.FileNotFoundException) {
                    this.Alphabets = new Dictionary<string, Node>();
                };

            }
        }


        public Boolean LookUp(string query)
        {
            Node node;
            return this.LookUp(query, out node);
        }

        //This function checks whether a word segment exists or not. For eg aah = true but aaa=false
        private Boolean LookUp(string query,out Node finalNode)
        {
            //basic validation
            if(query==null || query.Length==0 || query.Length==1)
            {
                finalNode = new Node(" ");
                return false;
            }

            //Gets the letter the segment starts from
            string letter = query.Substring(0, 1).ToLower();

            //Gets the node of the alphabet
            Node currentnode;
            this.Alphabets.TryGetValue(letter,out currentnode);

            //Search function gets the node of the word segment 
            finalNode = currentnode.search(letter, query);

            // Data of finalNode needs to be equal to the passed segment to be true
            return finalNode.getWord().Equals(query);

        }

        /*
            Note: Unit testing next three function is not feasible since the returned lists are different 
                in size and content for every incorrect word
        */

        //Function returns a list of suggested replacements word for an incorrect word passed as parameter
        public HashSet<String> suggestions(string incorrectWord)
        {
            //Past needs to be initialized to be used.There are safeguards in place for protection against null
            Node past=null;
            Node present;

            //Starts from the first two letters of the incorrect word.For e.g: ac for word accomodate
            for(int run=2;run<incorrectWord.Length+1;run++)
            {
                //Only when the segment doesn't exists anywhere in the list
                //In case of accomodate, program goes in when loop reaches accomo
                if(!this.LookUp(incorrectWord.Substring(0, run),out present))
                {
                    return this.suggestions(incorrectWord, past);
                }

                //In case the segment exists present becomes past
                /*
                For eg: until accom segment exists so here past=Node of accom.
                On the next iteration present=accomo which doesn't exists so past is passed
                */ 
                past = present;
            }

            //In case program never enters the if loop above
            return new HashSet<string>();

        } 
       
        //Second function for the same purpose above but with different parameters
        //Note: correctInitialSegment node has to exist in the data structure
        private HashSet<String> suggestions(string incorrectWord,Node correctInitialSegment)
        {
            //Basic validation
            if (correctInitialSegment == null || incorrectWord == null || incorrectWord.Length == 0)
                return new HashSet<String>();

            //List that will be returned
            HashSet<String> list = new HashSet<string>();

            //Starts iterating from the second char of the correct segment to the last of it
            /*
            For e.g: if accomodate = incorrectWord, so the correctInitialSegment.getWord() has to be = accom
            because up until accom there are number of possible correct words for instance accommodate,
            but accomo has no possible matching words
            */
            for (int externalRun = 2; externalRun < correctInitialSegment.getWord().Length+1; externalRun++)
            {

 
                //Getting the node of the correct segment
                Node node;
                this.LookUp(correctInitialSegment.getWord().Substring(0,externalRun), out node);

                //Getting the list of possible replacement words for that one segment
                HashSet<String> tempList = this.generateSuggestionList(incorrectWord, node);

                //Adding it in final list that will be returned
                for (int run = 0; run < tempList.Count; run++)
                    list.Add(tempList.ElementAt(run));
            }

            /*
            When the program exits this for loop,it will have added all the possible replacement words.
            For example in case of the word accomodate, the list will contain words with correct for 
            ac , acc , acco , accom
            */

            /*
            This will add the most popular word used for the segment in the list.
            For e.g: in case of accom perhaps the most popular word could be accomodate depending upon the user.
            */        
            //Note:The word can change depending on how many times the word is used
            list.Add(this.getMostPopularWordForSegment(correctInitialSegment.getWord()));

            return list;

        }

        //This function generates the list of possible replacment words for the incorrect segment.
        //User doesn't needs access to this function
        //sensitvity determines whether or not to add a word in the list
        //Note: correctInitialSegment node has to exist in the data structure
        private HashSet<String> generateSuggestionList(string incorrectWord,Node correctInitialSegment)
        {
            //In case the segment doesn't exists
            Node temp;
            if (!this.LookUp(correctInitialSegment.getWord(), out temp))
                return new HashSet<String>();

            //The list that will be returned
            HashSet<String> finalListOfWords = new HashSet<String>();

            //Edit distance for the incorrect word and the correct initial segment
            int dis = LevisthanDistance.Compute(incorrectWord,correctInitialSegment.getWord());
            
            //In case the dis is less or the more than the value of all editDistances available in the word segment
            if (dis < correctInitialSegment.minDistance())
                dis = correctInitialSegment.minDistance();
            else
            {
                if (dis > correctInitialSegment.maxDistance())
                    dis = correctInitialSegment.maxDistance();
            }

            //Gets the closest editDistance available if the dis calculated above is not in the SimilarWords
            //This while loop while iterate until a closest editDistance is not found
            //maxChecker checkes whether dis has reached max value in the list or not
            bool maxChecker = false;
            while (!correctInitialSegment.hasEditDistance(dis))
            {
                
                //First tries to get the closest greater editDistance
                if (!maxChecker)
                    ++dis;

                //In case dis reaches the max value in list of the SimilarWords Dictionary
                if (dis == correctInitialSegment.maxDistance())
                    maxChecker = true;

                //Tries to get the lowest and the closest value 
                if (maxChecker)
                    --dis;
                 
            }

            //This value determines whether or not to add a word in the list
            int sensitivity = 3;

            //Gets all the list of words which have the same edit distance from the correct segment
            List<String> choices = correctInitialSegment.getListOfSimilarWords(dis);

            //Will only iterate until there is atleast one word in the list
            while (finalListOfWords.Count==0)
            {
                //Iterating over the list of the words retrived above
                for (int run = 0; run < choices.Count; run++)
                {
                    //Current word at index run
                    string curWord = choices.ElementAt(run);

                    //If the edit distance between the incorrect word and the word above is less than sensitivty 
                    if (LevisthanDistance.Compute(incorrectWord, curWord) < sensitivity)
                    {
                        finalListOfWords.Add(curWord);
                    }

                    //Prevents list from becoming too big
                    if (finalListOfWords.Count == 10)
                        break;

                }

                ++sensitivity;

                //Prevents list from becoming too big
                if (finalListOfWords.Count == 10)
                    break;

            }

            
            return finalListOfWords;
        }
            
        //Checks whether a segment is an actual word not
        //e.g: aah=true aahe=false
        public bool isAWord(string word)
        {
            Node node;
            if (this.LookUp(word, out node))
                return node.wordStatus();
            else
                return false; 

        }

        //Changes the popularity of a word
        public void changeWordPopularity(string word)
        {
            //Checks whether the word actually exists or not
            if(this.isAWord(word))
            {
                for(int run=2;run<word.Length+1;run++)
                {
                    Node node;
                    this.LookUp(word.Substring(0, run), out node);
                    node.increaseWordPopularity(word);
                }
            }
        }

        //Gets the most popular for a given segment.
        //For e.g: if segment=accom,possible popular word could be accomodate depending upon the usage of the word 
        public string getMostPopularWordForSegment(string segment)
        {
            Node node;
            //Checks whether segment exists or not
            if (this.LookUp(segment, out node))
                return node.getMostPopularWord();

            //In case segment doesn't exists
            return "";
        }

    }
}
