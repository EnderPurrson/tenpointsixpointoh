using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
	public abstract class ExpressionVisitor
	{
		public virtual Expression Visit(Expression exp)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected I4, but got Unknown
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Expected O, but got Unknown
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Expected O, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Expected O, but got Unknown
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Expected O, but got Unknown
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Expected O, but got Unknown
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Expected O, but got Unknown
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Expected O, but got Unknown
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			if (exp == null)
			{
				return exp;
			}
			ExpressionType nodeType = exp.get_NodeType();
			switch ((int)nodeType)
			{
			case 4:
			case 10:
			case 11:
			case 28:
			case 30:
			case 34:
			case 40:
			case 44:
				return VisitUnary((UnaryExpression)exp);
			case 0:
			case 1:
			case 2:
			case 3:
			case 5:
			case 7:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 19:
			case 20:
			case 21:
			case 25:
			case 26:
			case 27:
			case 35:
			case 36:
			case 37:
			case 41:
			case 42:
			case 43:
				return VisitBinary((BinaryExpression)exp);
			case 45:
				return VisitTypeIs((TypeBinaryExpression)exp);
			case 8:
				return VisitConditional((ConditionalExpression)exp);
			case 9:
				return VisitConstant((ConstantExpression)exp);
			case 38:
				return VisitParameter((ParameterExpression)exp);
			case 23:
				return VisitMember((MemberExpression)exp);
			case 6:
				return VisitMethodCall((MethodCallExpression)exp);
			case 18:
				return VisitLambda((LambdaExpression)exp);
			case 31:
				return (Expression)(object)VisitNew((NewExpression)exp);
			case 32:
			case 33:
				return VisitNewArray((NewArrayExpression)exp);
			case 17:
				return VisitInvocation((InvocationExpression)exp);
			case 24:
				return VisitMemberInit((MemberInitExpression)exp);
			case 22:
				return VisitListInit((ListInitExpression)exp);
			default:
				throw new System.Exception(string.Format("Unhandled expression type: '{0}'", (object)exp.get_NodeType()));
			}
		}

		protected virtual MemberBinding VisitBinding(MemberBinding binding)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected I4, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			MemberBindingType bindingType = binding.get_BindingType();
			switch ((int)bindingType)
			{
			case 0:
				return (MemberBinding)(object)VisitMemberAssignment((MemberAssignment)binding);
			case 1:
				return (MemberBinding)(object)VisitMemberMemberBinding((MemberMemberBinding)binding);
			case 2:
				return (MemberBinding)(object)VisitMemberListBinding((MemberListBinding)binding);
			default:
				throw new System.Exception(string.Format("Unhandled binding type '{0}'", (object)binding.get_BindingType()));
			}
		}

		protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
		{
			ReadOnlyCollection<Expression> val = VisitExpressionList(initializer.get_Arguments());
			if (val != initializer.get_Arguments())
			{
				return Expression.ElementInit(initializer.get_AddMethod(), (System.Collections.Generic.IEnumerable<Expression>)val);
			}
			return initializer;
		}

		protected virtual Expression VisitUnary(UnaryExpression u)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Expression val = Visit(u.get_Operand());
			if (val != u.get_Operand())
			{
				return (Expression)(object)Expression.MakeUnary(((Expression)u).get_NodeType(), val, ((Expression)u).get_Type(), u.get_Method());
			}
			return (Expression)(object)u;
		}

		protected virtual Expression VisitBinary(BinaryExpression b)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Invalid comparison between Unknown and I4
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			Expression val = Visit(b.get_Left());
			Expression val2 = Visit(b.get_Right());
			Expression val3 = Visit((Expression)(object)b.get_Conversion());
			if (val != b.get_Left() || val2 != b.get_Right() || val3 != b.get_Conversion())
			{
				if ((int)((Expression)b).get_NodeType() == 7 && b.get_Conversion() != null)
				{
					return (Expression)(object)Expression.Coalesce(val, val2, (LambdaExpression)(object)((val3 is LambdaExpression) ? val3 : null));
				}
				return (Expression)(object)Expression.MakeBinary(((Expression)b).get_NodeType(), val, val2, b.get_IsLiftedToNull(), b.get_Method());
			}
			return (Expression)(object)b;
		}

		protected virtual Expression VisitIndex(IndexExpression e)
		{
			return (Expression)(object)e;
		}

		protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
		{
			Expression val = Visit(b.get_Expression());
			if (val != b.get_Expression())
			{
				return (Expression)(object)Expression.TypeIs(val, b.get_TypeOperand());
			}
			return (Expression)(object)b;
		}

		protected virtual Expression VisitConstant(ConstantExpression c)
		{
			return (Expression)(object)c;
		}

		protected virtual Expression VisitConditional(ConditionalExpression c)
		{
			Expression val = Visit(c.get_Test());
			Expression val2 = Visit(c.get_IfTrue());
			Expression val3 = Visit(c.get_IfFalse());
			if (val != c.get_Test() || val2 != c.get_IfTrue() || val3 != c.get_IfFalse())
			{
				return (Expression)(object)Expression.Condition(val, val2, val3);
			}
			return (Expression)(object)c;
		}

		protected virtual Expression VisitParameter(ParameterExpression p)
		{
			return (Expression)(object)p;
		}

		protected virtual Expression VisitMember(MemberExpression m)
		{
			Expression val = Visit(m.get_Expression());
			if (val != m.get_Expression())
			{
				return (Expression)(object)Expression.MakeMemberAccess(val, m.get_Member());
			}
			return (Expression)(object)m;
		}

		protected virtual Expression VisitMethodCall(MethodCallExpression m)
		{
			Expression val = Visit(m.get_Object());
			System.Collections.Generic.IEnumerable<Expression> enumerable = (System.Collections.Generic.IEnumerable<Expression>)VisitExpressionList(m.get_Arguments());
			if (val != m.get_Object() || enumerable != m.get_Arguments())
			{
				return (Expression)(object)Expression.Call(val, m.get_Method(), enumerable);
			}
			return (Expression)(object)m;
		}

		protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
		{
			List<Expression> val = null;
			int i = 0;
			for (int count = original.get_Count(); i < count; i++)
			{
				Expression val2 = Visit(original.get_Item(i));
				if (val != null)
				{
					val.Add(val2);
				}
				else if (val2 != original.get_Item(i))
				{
					val = new List<Expression>(count);
					for (int j = 0; j < i; j++)
					{
						val.Add(original.get_Item(j));
					}
					val.Add(val2);
				}
			}
			if (val != null)
			{
				return val.AsReadOnly();
			}
			return original;
		}

		protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
		{
			Expression val = Visit(assignment.get_Expression());
			if (val != assignment.get_Expression())
			{
				return Expression.Bind(((MemberBinding)assignment).get_Member(), val);
			}
			return assignment;
		}

		protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
		{
			System.Collections.Generic.IEnumerable<MemberBinding> enumerable = VisitBindingList(binding.get_Bindings());
			if (enumerable != binding.get_Bindings())
			{
				return Expression.MemberBind(((MemberBinding)binding).get_Member(), enumerable);
			}
			return binding;
		}

		protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
		{
			System.Collections.Generic.IEnumerable<ElementInit> enumerable = VisitElementInitializerList(binding.get_Initializers());
			if (enumerable != binding.get_Initializers())
			{
				return Expression.ListBind(((MemberBinding)binding).get_Member(), enumerable);
			}
			return binding;
		}

		protected virtual System.Collections.Generic.IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
		{
			List<MemberBinding> val = null;
			int i = 0;
			for (int count = original.get_Count(); i < count; i++)
			{
				MemberBinding val2 = VisitBinding(original.get_Item(i));
				if (val != null)
				{
					val.Add(val2);
				}
				else if (val2 != original.get_Item(i))
				{
					val = new List<MemberBinding>(count);
					for (int j = 0; j < i; j++)
					{
						val.Add(original.get_Item(j));
					}
					val.Add(val2);
				}
			}
			if (val != null)
			{
				return (System.Collections.Generic.IEnumerable<MemberBinding>)val;
			}
			return (System.Collections.Generic.IEnumerable<MemberBinding>)original;
		}

		protected virtual System.Collections.Generic.IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
		{
			List<ElementInit> val = null;
			int i = 0;
			for (int count = original.get_Count(); i < count; i++)
			{
				ElementInit val2 = VisitElementInitializer(original.get_Item(i));
				if (val != null)
				{
					val.Add(val2);
				}
				else if (val2 != original.get_Item(i))
				{
					val = new List<ElementInit>(count);
					for (int j = 0; j < i; j++)
					{
						val.Add(original.get_Item(j));
					}
					val.Add(val2);
				}
			}
			if (val != null)
			{
				return (System.Collections.Generic.IEnumerable<ElementInit>)val;
			}
			return (System.Collections.Generic.IEnumerable<ElementInit>)original;
		}

		protected virtual Expression VisitLambda(LambdaExpression lambda)
		{
			Expression val = Visit(lambda.get_Body());
			if (val != lambda.get_Body())
			{
				return (Expression)(object)Expression.Lambda(((Expression)lambda).get_Type(), val, (System.Collections.Generic.IEnumerable<ParameterExpression>)lambda.get_Parameters());
			}
			return (Expression)(object)lambda;
		}

		protected virtual NewExpression VisitNew(NewExpression nex)
		{
			System.Collections.Generic.IEnumerable<Expression> enumerable = (System.Collections.Generic.IEnumerable<Expression>)VisitExpressionList(nex.get_Arguments());
			if (enumerable != nex.get_Arguments())
			{
				if (nex.get_Members() != null)
				{
					return Expression.New(nex.get_Constructor(), enumerable, (System.Collections.Generic.IEnumerable<MemberInfo>)nex.get_Members());
				}
				return Expression.New(nex.get_Constructor(), enumerable);
			}
			return nex;
		}

		protected virtual Expression VisitMemberInit(MemberInitExpression init)
		{
			NewExpression val = VisitNew(init.get_NewExpression());
			System.Collections.Generic.IEnumerable<MemberBinding> enumerable = VisitBindingList(init.get_Bindings());
			if (val != init.get_NewExpression() || enumerable != init.get_Bindings())
			{
				return (Expression)(object)Expression.MemberInit(val, enumerable);
			}
			return (Expression)(object)init;
		}

		protected virtual Expression VisitListInit(ListInitExpression init)
		{
			NewExpression val = VisitNew(init.get_NewExpression());
			System.Collections.Generic.IEnumerable<ElementInit> enumerable = VisitElementInitializerList(init.get_Initializers());
			if (val != init.get_NewExpression() || enumerable != init.get_Initializers())
			{
				return (Expression)(object)Expression.ListInit(val, enumerable);
			}
			return (Expression)(object)init;
		}

		protected virtual Expression VisitNewArray(NewArrayExpression na)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Invalid comparison between Unknown and I4
			System.Collections.Generic.IEnumerable<Expression> enumerable = (System.Collections.Generic.IEnumerable<Expression>)VisitExpressionList(na.get_Expressions());
			if (enumerable != na.get_Expressions())
			{
				if ((int)((Expression)na).get_NodeType() == 32)
				{
					return (Expression)(object)Expression.NewArrayInit(((Expression)na).get_Type().GetElementType(), enumerable);
				}
				return (Expression)(object)Expression.NewArrayBounds(((Expression)na).get_Type().GetElementType(), enumerable);
			}
			return (Expression)(object)na;
		}

		protected virtual Expression VisitInvocation(InvocationExpression iv)
		{
			System.Collections.Generic.IEnumerable<Expression> enumerable = (System.Collections.Generic.IEnumerable<Expression>)VisitExpressionList(iv.get_Arguments());
			Expression val = Visit(iv.get_Expression());
			if (enumerable != iv.get_Arguments() || val != iv.get_Expression())
			{
				return (Expression)(object)Expression.Invoke(val, enumerable);
			}
			return (Expression)(object)iv;
		}
	}
}
