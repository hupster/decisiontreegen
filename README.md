# decisiontreegen
Generate JSON file for use with Sidecar Decision-tree plugin for WordPress

The Sidecar decision tree plugin uses a single JSON entry in the database to store decision trees.
This tool can generate the entry, using a text file as input instead of the WordPress interface.

http://sidecar.tv/contact-us/
https://github.com/wp-plugins/sidecar-decision-tree/

Usage 
decisiontreegen.exe c:\example\mydecisiontree.txt

Example input

Do you like decision tree plugin?
  Yes -> Did you know that you can now also generate them using text files?
    OK -> Cheers!
    I dont want too -> Then just use the Wordpress interface
  No -> No problemo.

Why?
Because I made a long decision tree, pressed save and publish, and then the whole thing disappeared. So I traced it back to the database field and decided I could enter it there directly.
