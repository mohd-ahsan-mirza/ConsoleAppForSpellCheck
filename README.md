# ConsoleAppForSpellCheck


This is a console application used for real time spell checking. Functionalites the application provide are detecting spelling mistakes, providing a list of matching replacements for incorrect words and auto complete suggestions.

The Dictionary uses the Node class as private because exposing the nodes in the Dictionary class can break the application.

A picture is worth a thousand words.That is why I drew a simple diagram to illustrate how Node class works for this application.
https://github.com/mohd-ahsan-mirza/ConsoleAppForSpellCheck/blob/master/image.png

The application simply uses a non-binary search tree data structure for detecting spelling mistakes and coupled with edit distances between words and segments, the application provides list of suggestions for incorrect words

After a dictionary class is initialized, passing a file name as arguement, it takes couple of seconds to finish initialization because of
the levisthan distance being added in every segment for every of it's potentional word.

There is a test class also in the project. You can use the test class for unit testing.

There are detailed comments in the solution, explaining how everything works.

Enjoy!!


