using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [System.Serializable]
    public class Clause
    {
        public List<Expression> expressions;
        public Clause()
        {
            expressions = new List<Expression>();
        }

        public Clause(List<Expression> expressions)
        {
            this.expressions = expressions; 
        }

        public List<Expression> GetExpressions()
        {
            return expressions;
        }

        public bool IsDefault()
        {
            if (expressions[0].GetExpressionNodeName().Equals("Default"))
            {
                return true;
            }
            return false;
        }

        public void AddExpression(int index, Expression expression)
        {
            if ((expressions.Count - 1) >= index)
            {
                expressions[index].SetExpression(expression.GetExpressionNodeName(), expression.GetExpressionName(), expression.GetNextExpressionNode());
            }
            else
            {
                expressions.Add(new Expression(expression.GetExpressionNodeName(), expression.GetExpressionName(), expression.GetNextExpressionNode()));
            }
        }

        public void AddExpressions(List<Expression> expressions)
        {
            foreach (Expression expression in expressions)
            {
                Debug.Log("[Clause]: Adding expression to clause " + expression.GetExpressionName());
                this.expressions.Add(new Expression(expression.GetExpressionNodeName(), expression.GetExpressionName(), expression.GetNextExpressionNode()));
            }
        }

        public string GetClauseName()
        {
            string name = "";
            foreach (Expression expression in expressions)
            {
                name += expression.GetExpressionName() + " ";
            }
            return name;
        }
    }
}