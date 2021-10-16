/*
* Copyright © 2021 MyNihongo
*/

using System;
using System.Linq.Expressions;

namespace MyNihongo.Expressions
{
	/// <summary>
	/// Utility class for getting and settings properties
	/// </summary>
	public static class PropertyOf
	{
		public static object? Get(object source, string propertyName) =>
			Get<object?>(source, propertyName);

		public static T? Get<T>(object source, string propertyName)
		{
			var key = new Tuple<Type, string>(source.GetType(), propertyName);
			var @delegate = ExpressionCache.PropertyGetters.GetOrAdd(key, static x =>
			{
				return new Lazy<Delegate>(() =>
				{
					var param = Expression.Parameter(x.Item1);
					var prop = Expression.Property(param, x.Item2);

					return Expression.Lambda(prop, param).Compile();
				});
			}).Value;

			return (T?)@delegate.DynamicInvoke(source);
		}

		public static void Set<T>(object source, string propertyName, T value)
		{
			var key = new Tuple<Type, string>(source.GetType(), propertyName);
			var @delegate = ExpressionCache.PropertySetters.GetOrAdd(key, static x =>
			{
				return new Lazy<Delegate>(() =>
				{
					var param = Expression.Parameter(x.Item1);
					var prop = Expression.Property(param, x.Item2);
					var valueParam = Expression.Parameter(typeof(T));
					var assign = Expression.Assign(prop, valueParam);

					return Expression.Lambda(assign, param, valueParam).Compile();
				});
			}).Value;

			@delegate.DynamicInvoke(source, value);
		}
	}

	/// <summary>
	/// Utility class for getting and settings properties
	/// </summary>
	public static class PropertyOf<T>
	{
		public static TDestination Get<TDestination>(T obj, string propertyName) =>
			Cache<TDestination>.Get(propertyName)(obj);

		private static class Cache<TDestination>
		{
			public static Func<T, TDestination> Get(string propertyName)
			{
				var key = new Tuple<Type, string>(typeof(T), propertyName);

				return (Func<T, TDestination>)ExpressionCache.PropertyGetters.GetOrAdd(key, static x =>
				{
					return new Lazy<Delegate>(() =>
					{
						var param = Expression.Parameter(x.Item1);
						var prop = Expression.Property(param, x.Item2);

						return Expression.Lambda<Func<T, TDestination>>(prop, param).Compile();
					});
				}).Value;
			}
		}
	}
}
