 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProgramAR.Pages;


/// <summary>
/// Cite: https://github.com/itdranik/coding-interview/blob/master/cs-solvers/ITDranik/CodingInterview/Solvers/MathExpressions/PostfixNotationCalculator.cs
/// </summary>

namespace RuleProcessor
{
    public class Tokenizer : MonoBehaviour
    {
        [SerializeField] TriggerActionRules rules;
        [SerializeField] ZoneObjectRuntimeDictionary zoneObjects;
        [SerializeField] BoxObjectRuntimeDictionary boxObjects;
        [SerializeField] GameObject tapListFromYourRules;
        public Tokenizer()
        {
            _infixNotationTokens = new List<IToken>();
        }

/*        public IEnumerable<IToken> Parse(string expression)
        {
            Reset();
            foreach (string next in expression)
            {
                FeedCharacter(next);
            }
            return _infixNotationTokens();
        }*/
        
        private void Reset()
        {
            _infixNotationTokens.Clear();
        }

        private void FeedCharacter(string next)
        {

            _infixNotationTokens.Add(CreateOperandToken(true));
        }

        private static IToken CreateOperandToken(bool value)
        {
            return new OperandToken(value);
        }

        private static OperatorToken CreateOperatorToken(string c)
        {
            return c switch
            {
                "And" => new OperatorToken(OperatorType.And),
                "Or" => new OperatorToken(OperatorType.Or),
                _ => throw new SyntaxException($"There's no a suitable operator for the string {c}"),
            };
        }


        private readonly List<IToken> _infixNotationTokens;
    }
}