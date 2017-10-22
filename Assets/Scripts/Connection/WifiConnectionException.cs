using System;

public class WifiConnectionException : ConnectionException {
    
    public WifiConnectionException(){}
    public WifiConnectionException(string message) : base(message){}
    public WifiConnectionException(string message, Exception inner) 
        : base(message, inner){}
}