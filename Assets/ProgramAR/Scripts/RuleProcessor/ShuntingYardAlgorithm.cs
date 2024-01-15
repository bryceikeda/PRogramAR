 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cite: https://github.com/itdranik/coding-interview/tree/master/cs-solvers/ITDranik/CodingInterview/Solvers/MathExpressions
/// </summary>

namespace RuleProcessor
{
    public class ShuntingYardAlgorithm
    {
        public ShuntingYardAlgorithm()
        {
            _operatorsStack = new Stack<OperatorToken>();
            _postfixNotationTokens = new List<IToken>();
        }

        public IEnumerable<IToken> Apply(IEnumerable<IToken> infixNotationTokens)
        {
            Reset();
            foreach (var token in infixNotationTokens)
            {
                ProcessToken(token);
            }
            return GetResult();
        }

        private void Reset()
        {
            _operatorsStack.Clear();
            _postfixNotationTokens.Clear();
        }


        private void ProcessToken(IToken token)
        {
            switch (token)
            {
                case OperandToken operandToken:
                    StoreOperand(operandToken);
                    break;
                case OperatorToken operatorToken:
                    ProcessOperator(operatorToken);
                    break;
                default:
                    var exMessage = $"An unknown token type: {token.GetType()}.";
                    throw new SyntaxException(exMessage);
            }
        }

        private void StoreOperand(OperandToken operandToken)
        {
            _postfixNotationTokens.Add(operandToken);
        }

        private void ProcessOperator(OperatorToken operatorToken)
        {
            switch (operatorToken.OperatorType)
            {
                default:
                    PushOperator(operatorToken);
                    break;
            }
        }

        private void PushOperator(OperatorToken operatorToken)
        {
            var operatorPriority = GetOperatorPriority(operatorToken);

            while (_operatorsStack.Count > 0)
            {
                var stackOperatorToken = _operatorsStack.Peek();

                var stackOperatorPriority = GetOperatorPriority(stackOperatorToken);
                if (stackOperatorPriority < operatorPriority)
                {
                    break;
                }

                _postfixNotationTokens.Add(_operatorsStack.Pop());
            }

            _operatorsStack.Push(operatorToken);
        }

        private static int GetOperatorPriority(OperatorToken operatorToken)
        {
            switch (operatorToken.OperatorType)
            {
                case OperatorType.Or:
                    return 0;
                case OperatorType.And:
                    return 1;
                default:
                    var exMessage = "An unexpected action for the operator: " +
                        $"{operatorToken.OperatorType}.";
                    throw new SyntaxException(exMessage);
            }
        }

        private List<IToken> GetResult()
        {
            while (_operatorsStack.Count > 0)
            {
                var token = _operatorsStack.Pop();
                _postfixNotationTokens.Add(token);
            }
            return _postfixNotationTokens;
        }

        private readonly Stack<OperatorToken> _operatorsStack;
        private readonly List<IToken> _postfixNotationTokens;
    }
}