namespace BLL.Exceptions;

public class TooFrequentFlightsException : Exception
{
    public TooFrequentFlightsException(string s) : base(s)
    {
        
    }
}