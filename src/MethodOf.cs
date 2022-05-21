namespace MyNihongo.Expressions;

/// <summary>
/// Utility class for invoking methods
/// </summary>
public static class MethodOf
{
	public static T? Invoke<T>(object @this, string methodName, params object[] args)
	{
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

		var key = new Tuple<Type, string>(@this.GetType(), methodName);
		var @delegate = ExpressionCache.Invoke.GetOrAdd(key, x =>
		{
			return new Lazy<Delegate>(() =>
			{
				var param = Expression.Parameter(x.Item1);

				if (args.Length == 0)
				{
					var call = Expression.Call(param, x.Item2, null);
					return Expression.Lambda(call, param).Compile();
				}
				else
				{
					var @params = new Expression[args.Length];

					for (var i = 0; i < args.Length; i++)
						@params[i] = Expression.Parameter(args[i].GetType());

					var lambdaParams = new ParameterExpression[args.Length + 1];
					lambdaParams[0] = param;
					Array.Copy(@params, 0, lambdaParams, 1, @params.Length);

					var call = Expression.Call(param, x.Item2, null, @params);
					return Expression.Lambda(call, lambdaParams).Compile();
				}
			});
		}).Value;

		return (T?)@delegate.DynamicInvoke(invokeParams);
	}
}
