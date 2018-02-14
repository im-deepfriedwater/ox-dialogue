using System;

public class InvalidDialogueFormatException: Exception {
    public InvalidDialogueFormatException () {}

    public InvalidDialogueFormatException(string message) 
 	   : base(message)
 	{}

    public InvalidDialogueFormatException(string message, Exception inner)
    	: base(message, inner)
    {}
}