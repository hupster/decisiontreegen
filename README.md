# decisiontreegen
Generate JSON file for use with Sidecar Decision-tree plugin for WordPress

The Sidecar decision tree plugin uses a single JSON entry in the database to store decision trees.
This tool can generate the entry, using a text file as input instead of the WordPress interface.

http://sidecar.tv/contact-us/
https://github.com/wp-plugins/sidecar-decision-tree/

## Usage 

`
decisiontreegen.exe c:\example\mydecisiontree.txt
`

1. Create input .txt file
2. Run command to create .json
3. Create new decision tree the normal way
4. Copy .json text to the database field (table xxxx_postmeta, newest entry with meta_key=dtree_blob)

## Input

On the command line provide a link to a plain text file with the following format:

```
Do you like the decision tree plugin?
  Yes -> Did you know that you can now also generate them using text files?
    OK -> Cheers! *It also supports subtext.
    I dont want too -> Then just use the Wordpress interface.
  No -> No problemo.
```

The format uses these special characters:
- To add choices to a question, indent them using a single tab (no spaces). If a line has no choices, it becomes an answer. There is no practical limit to the amount of choices, just keep increasing the amount of tabs.
- To split the choice from the next question (or anwer), use the string " -> ".
- To optionally split subtext from the question text, use the character "*".
- Note that the " character itself seems not supported by the plugin. So the tool checks for that.

## Output

The tool creates a .json file at the same location as the .txt file, with the following format:

```
{"data":{"1":{"question":"Do you like the decision tree plugin?","type":"question","subtext":"","textlink":"","info":"","choices":[{"choice":"Yes","next":"2"},{"choice":"No","next":"5"}]},"2":{"question":"Did you know that you can now also generate them using text files?","type":"question","subtext":"","textlink":"","info":"","choices":[{"choice":"OK","next":"3"},{"choice":"I dont want too","next":"4"}]},"3":{"question":"Cheers! ","type":"answer","subtext":"It also supports subtext.","textlink":"","info":"","choices":[]},"4":{"question":"Then just use the Wordpress interface.","type":"answer","subtext":"","textlink":"","info":"","choices":[]},"5":{"question":"No problemo.","type":"answer","subtext":"","textlink":"","info":"","choices":[]}},"index":{"0":"1","1":"2","2":"3","3":"4","4":"5"},"start_ID":"1","version":"1.1.0"}
```

## Why?
Because I made a long decision tree, pressed save and publish, and then the whole thing disappeared. So I traced it back to the database field and decided I could enter it there directly. Also, it allows others to review and modify the text without having to recreate the tree, which is quite time consuming.
