namespace MyNihongo.Expressions;

/// <summary>
/// Utility class for casting
/// </summary>
public static class CastTo<T>
{
	public static T From<TSource>(TSource from) =>
		Cache<TSource>.Caster(from);

	private static class Cache<TSource>
	{
		public static readonly Func<TSource, T> Caster = GetFunc();

		private static Func<TSource, T> GetFunc()
		{
			var parameter = Expression.Parameter(typeof(TSource));
			var check = Expression.ConvertChecked(parameter, typeof(T));

			return Expression.Lambda<Func<TSource, T>>(check, parameter).Compile();
		}
	}
}
