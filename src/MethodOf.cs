using System;
using System.Linq.Expressions;

namespace MyNihongo.Expressions
{
	/// <summary>
	/// Utility class for invoking methods
	/// </summary>
	public static class MethodOf
	{
		public static T? Invoke<T>(object @this, string methodName, params object[] args)
		{
			var type = @this.GetType();
			var key = new Tuple<Type, string>(type, methodName);

			if (!ExpressionCache.Invoke.TryGetValue(key, out var lambda))
			{
				var param = Expression.Parameter(type);

				if (args.Length == 0)
				{
					var call = Expression.Call(param, methodName, null);
					lambda = Expression.Lambda(call, param).Compile();
				}
				else
				{
					var @params = new Expression[args.Length];

					for (var i = 0; i < args.Length; i++)
						@params[i] = Expression.Parameter(args[i].GetType());

					var lambdaParams = new ParameterExpression[args.Length + 1];
					lambdaParams[0] = param;
					Array.Copy(@params, 0, lambdaParams, 1, @params.Length);

					var call = Expression.Call(param, methodName, null, @params);
					lambda = Expression.Lambda(call, lambdaParams).Compile();
				}

				ExpressionCache.Invoke.TryAdd(key, lambda);
			}

			object?[] invokeParams;

			if (args.Length == 0)
			{
				invokeParams = new[] { @this };
			}
			else
			{
				invokeParams = new object[args.Length + 1];
				invokeParams[0] = @this;
				Array.Copy(args, 0, invokeParams, 1, args.Length);
			}

			return (T?)lambda.DynamicInvoke(invokeParams);
		}
	}
}
