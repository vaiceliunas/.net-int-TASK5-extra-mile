using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OracleCallTranslator
{
    public class ExpressionToOracleTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;
        public ExpressionToOracleTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable))
            {
                switch (node.Method.Name)
                {
                    case "Where":
                        var queryNode = node.Arguments[0];
                        var predicate = node.Arguments[1];
                        InitializeQuery(queryNode);
                        Visit(predicate);
                        return node;
                    default:
                        throw new NotSupportedException($"Method '{node.Method.Name}' not supported");
                }
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:

                    Visit(node.Left);
                    _resultStringBuilder.Append(" = ");
                    Visit(node.Right);

                    break;

                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append(" AND ");
                    Visit(node.Right);
                    break;

                case ExpressionType.GreaterThan:
                    Visit(node.Left);
                    _resultStringBuilder.Append(" > ");
                    Visit(node.Right);
                    break;

                case ExpressionType.LessThan:
                    Visit(node.Left);
                    _resultStringBuilder.Append(" < ");
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name);

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == typeof(string))
            {
                _resultStringBuilder.Append("'" + node.Value + "'");
            }

            if (node.Type == typeof(int))
            {
                _resultStringBuilder.Append(node.Value);
            }

            return node;
        }


        public Expression InitializeQuery(Expression node)
        {
            _resultStringBuilder.Append("SELECT * FROM " + node.Type.GenericTypeArguments[0].Name + " WHERE ");
            return node;
        }
    }
}
