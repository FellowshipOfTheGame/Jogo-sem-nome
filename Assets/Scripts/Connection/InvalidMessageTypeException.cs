using System;

public class InvalidMessageTypeException : Exception {
    
    public InvalidMessageTypeException(){}
    public InvalidMessageTypeException(string message) : base(message){}
    public InvalidMessageTypeException(string message, Exception inner) 
        : base(message, inner){}
}