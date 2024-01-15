namespace ProgramAR.Pages
{
    [System.Serializable]
    public class Expression
    {
        public string expressionNodeName;
        public string expressionName;
        public string nextExpressionNode;

        public Expression(string expressionNodeName, string expressionName, string nextExpressionNode)
        {
            this.expressionNodeName = expressionNodeName;
            this.expressionName = expressionName;
            this.nextExpressionNode = nextExpressionNode;
        }

        public void SetExpression(string expressionNodeName, string expressionName, string nextExpressionNode)
        {
            this.expressionNodeName = expressionNodeName;
            this.expressionName = expressionName;
            this.nextExpressionNode = nextExpressionNode;
        }

        public string GetExpressionNodeName()
        {
            return expressionNodeName;
        }

        public string GetExpressionName()
        {
            return expressionName;
        }

        public string GetNextExpressionNode()
        {
            return nextExpressionNode;
        }

        public Expression Copy()
        {
            return new Expression(expressionNodeName, expressionName, nextExpressionNode);
        }
    }
}