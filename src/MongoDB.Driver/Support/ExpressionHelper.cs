using System.Linq.Expressions;

namespace MongoDB.Driver.Support
{
    internal static class ExpressionHelper
    {
        /// <summary>
        ///  Creates short path for `if x == null then null else foo(x)` => just `foo(x)` since mongo get all nulls propagated automatically
        /// </summary>
        public static Expression TryFlattenIfEqNull(this ConditionalExpression node)
        {
            if ((node.Test.NodeType == ExpressionType.Equal || node.Test.NodeType == ExpressionType.NotEqual)
                && (node.Test as BinaryExpression)?.Right is ConstantExpression constantExpression
                && constantExpression.Value is null)
            {
                if (node.IfTrue is ConstantExpression trueConstantExpression && trueConstantExpression.Value is null)
                {
                    return node.IfFalse;
                }

                if (node.IfFalse is ConstantExpression falseConstantExpression && falseConstantExpression.Value is null)
                {
                    return node.IfTrue;
                }
            }

            return null;
        }
    }
}
