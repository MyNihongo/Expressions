/*
* Copyright © 2021 MyNihongo
*/

using System;
using System.Collections.Concurrent;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MyNihongo.Expressions.Tests")]
namespace MyNihongo.Expressions
{
	internal static class ExpressionCache
	{
		internal static readonly ConcurrentDictionary<Tuple<Type, string>, Lazy<Delegate>>
			PropertyGetters = new(),
			PropertySetters = new();

		internal static readonly ConcurrentDictionary<Tuple<Type, string>, Lazy<Delegate>>
			Invoke = new();
	}
}
