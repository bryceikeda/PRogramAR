 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProgramAR.Pages;
using System;
using System.Text.RegularExpressions;
using ProgramAR.Sets; 

/// <summary>
///
/// </summary>

namespace RuleProcessor
{
    public class TriggerProcessor : MonoBehaviour
    {
        [SerializeField] TriggerActionRules rules;
        [SerializeField] ZoneObjectRuntimeDictionary zoneObjects;
        [SerializeField] BoxObjectRuntimeDictionary boxObjects;
        private PostfixNotationCalculator postFixCalculator;
        private ShuntingYardAlgorithm shuntingYard;

        private List<IToken> _infixNotationTokens;

        private void Start()
        {
            postFixCalculator = new PostfixNotationCalculator();
            shuntingYard = new ShuntingYardAlgorithm();

        }

        public string GetOperator(int index)
        {
            return rules.pairs[index].trigger[0].expressions[0].expressionName; 
        }

        public bool EvaluateTrigger(int index)
        {
            _infixNotationTokens = new List<IToken>();
            if (rules.pairs.Count > index)
            {
                _infixNotationTokens.Clear();

                // Check the triggers
                for (int k = 0; k < rules.pairs[index].trigger.Count; k++)
                {
                    

                    var clause = rules.pairs[index].trigger[k];
              
                    if (k != 0)
                    {
                        _infixNotationTokens.Add(CreateOperatorToken(clause.expressions[0].expressionName));
                    }
                    // last word is always "Zone #" 
                    if (zoneObjects.Items.TryGetValue(clause.expressions[3].expressionName[5] - '0', out ZoneObject zone))
                    {
                       
                        // Third word is "
                        string obj = clause.expressions[1].expressionName;
                        string condition = clause.expressions[2].expressionName;

                        if (obj.Equals("any box"))
                        {
                            if (condition.Equals("is in"))
                            {
                                _infixNotationTokens.Add(CreateOperandToken(zone.HasObjectsInside()));
                            }
                            else if (condition.Equals("is not in"))
                            {
                                _infixNotationTokens.Add(CreateOperandToken(!zone.HasObjectsInside()));
                            }
                        }
                        else if (obj.Equals("no boxes"))
                        {
                            _infixNotationTokens.Add(CreateOperandToken(!zone.HasObjectsInside()));
                        }
                        else 
                        {
                            if (condition.Equals("is in"))
                            {
                                _infixNotationTokens.Add(CreateOperandToken(zone.HasObjectInside(obj)));
                            }
                            else if (condition.Equals("is not in"))
                            {
                                _infixNotationTokens.Add(CreateOperandToken(!zone.HasObjectInside(obj)));
                            }
                        }
                    }
                    else
                    {
                        _infixNotationTokens.Add(CreateOperandToken(false));
                    }
                }

                if (_infixNotationTokens.Count > 0)
                {
                    var postFix = shuntingYard.Apply(_infixNotationTokens);
                    OperandToken trigger = postFixCalculator.Calculate(postFix);
                    return trigger.Value;
                }
                else
                {
                    Debug.LogWarning("Invalid Rule");
                    return false; 
                }
            }
            else
            {
                Debug.LogWarning("Index " + index + " does not exist " + rules.pairs.Count);
                return false;
            }
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

    }
}