namespace MyNihongo.Expressions;

internal static class ExpressionCache
{
	internal static readonly ConcurrentDictionary<Tuple<Type, string>, Lazy<Delegate>>
		PropertyGetters = new(),
		PropertySetters = new();

	internal static readonly ConcurrentDictionary<Tuple<Type, string>, Lazy<Delegate>>
		Invoke = new();
}
