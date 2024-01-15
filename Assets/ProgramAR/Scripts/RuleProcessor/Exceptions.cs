 
using System;
using System.Runtime.Serialization;

/// <summary>
///  Cite: https://github.com/itdranik/coding-interview/tree/master/cs-solvers/ITDranik/CodingInterview/Solvers/MathExpressions
/// </summary>

[Serializable]
public class MathExpressionException : Exception
{
    public MathExpressionException()
    {
    }

    public MathExpressionException(string message) : base(message)
    {
    }

    public MathExpressionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected MathExpressionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}

public class SyntaxException : MathExpressionException
{
    public SyntaxException()
    {
    }

    public SyntaxException(string message) : base(message)
    {
    }

    public SyntaxException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected SyntaxException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}