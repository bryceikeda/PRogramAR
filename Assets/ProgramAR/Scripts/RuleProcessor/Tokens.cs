 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cite: https://github.com/itdranik/coding-interview/tree/master/cs-solvers/ITDranik/CodingInterview/Solvers/MathExpressions
/// </summary>

namespace RuleProcessor
{
    public interface IToken { }

    public class OperandToken : IToken
    {
        public bool Value { get; }

        public OperandToken(bool value)
        {
            Value = value;
        }
    }

    public enum OperatorType
    {
        And,
        Or
    }

    public class OperatorToken : IToken
    {
        public OperatorType OperatorType { get; }

        public OperatorToken(OperatorType operatorType)
        {
            OperatorType = operatorType;
        }
    }
}