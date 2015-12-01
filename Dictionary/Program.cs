

                 /*********This class is not being used as public in the spell check application***************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using levisthanDistance;

namespace Nodes
{
   
    // A node is just a segment of letters.It can or can't be a word
    // Note: This class can't be tested directly
    public class Node
    {
        //The string of letters passed
        private string Data;
        //Whether the segment of words is word that is in the list
        private bool isAWord;
        /*
        The most easiest way to explain this is with an example.
        If Data=accomplic , it's children would be accomplice,accomplices
        */
        private Dictionary<String,Node> Children;
        //This contains edit Distance as key and list of strings which have the same distance from Data
        private Dictionary<int,List<String>> SimilarWords;
        //This list contains all the possible matching words and the frequency they have been used  
        private Dictionary<String, int> Popularity;

        //letter is segment of words.status is whether the segment is an actual word.By default it is false
        public Node(string letter,bool status=false)
        {
            //Basic validation
            if (letter.Length != 0)
            {
                this.Data = letter;
                Children = new Dictionary<String,Node>();
                SimilarWords = new Dictionary<int,List<String>>();
                this.Popularity = new Dictionary<string,int>();
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
        public Boolean add(string current,string word)
        {
            //Basic validation
            if (current==null || current.Length==0 || word==null || word.Length==0)
                return false;

            else {

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
                    if(current.Equals(word))
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
            if(this.SimilarWords.ContainsKey(editDistance))
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
        public Node search(string current,string word)
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
    
}
