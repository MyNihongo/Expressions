using System;
using System.Collections.Concurrent;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace MyNihongo.Expressions.Tests.PropertyOfTests
{
	public sealed class SetShould
	{
		[Fact]
		public void SetProperty()
		{
			const string text = nameof(text);
			var input = new TestRecord();

			input.Text
				.Should()
				.BeEmpty();

			PropertyOf.Set(input, nameof(TestRecord.Text), text);

			input.Text
				.Should()
				.Be(text);
		}

		[Fact]
		public void SaveExpressionsInCache()
		{
			var item = new SetCacheTest
			{
				Prop = "text123"
			};

			const string prop = nameof(item.Prop);
			var key = new Tuple<Type, string>(item.GetType(), prop);

			var dictionary = (ConcurrentDictionary<Tuple<Type, string>, Lazy<Delegate>>)typeof(ExpressionCache)
				.GetField(nameof(ExpressionCache.PropertySetters2), BindingFlags.Static | BindingFlags.NonPublic)
				!.GetValue(null);

			dictionary
				.Should()
				.NotContainKey(key);

			PropertyOf.Set(item, prop, "another text");

			dictionary
				.Should()
				.ContainKey(key);
		}

		[Fact]
		public void ThrowExceptionIfPropertyNotFound()
		{
			var input = new TestRecord();

			Action action = () => PropertyOf.Set(input, "NotPresent", "any");

			action
				.Should()
				.ThrowExactly<ArgumentException>();
		}
	}
}
