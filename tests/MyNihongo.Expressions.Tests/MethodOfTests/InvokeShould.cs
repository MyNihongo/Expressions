namespace MyNihongo.Expressions.Tests.MethodOfTests;

public sealed class InvokeShould
{
	[Fact]
	public void ReturnResultWithoutParameters()
	{
		var result = MethodOf.Invoke<int>(new TestRecord(), nameof(TestRecord.GetResult));

		result
			.Should()
			.Be(123);
	}

	[Fact]
	public void ReturnResultWithParameters()
	{
		var result = MethodOf.Invoke<int>(new TestRecord(), nameof(TestRecord.GetAnotherResult), 12);

		result
			.Should()
			.Be(135);
	}

	[Fact]
	public void SaveExpressionInCache()
	{
		const string method = nameof(InvokeCacheTest.GetResult);
		var key = new Tuple<Type, string>(typeof(InvokeCacheTest), method);

		var dictionary = (ConcurrentDictionary<Tuple<Type, string>, Lazy<Delegate>>)typeof(ExpressionCache)
			.GetField(nameof(ExpressionCache.Invoke), BindingFlags.Static | BindingFlags.NonPublic)
			!.GetValue(null)!;

		dictionary
			.Should()
			.NotContainKey(key);

		MethodOf.Invoke<DateTime>(new InvokeCacheTest(), method);

		dictionary
			.Should()
			.ContainKey(key);
	}

	[Fact]
	public void ThrowExceptionIfMethodNotExists()
	{
		Action action = () => MethodOf.Invoke<int>(new TestRecord(), "NotExists");

		action
			.Should()
			.ThrowExactly<InvalidOperationException>();
	}

	[Fact]
	public void ThrowExceptionIfInvalidReturnType()
	{
		Action action = () => MethodOf.Invoke<DateTime>(new TestRecord(), nameof(TestRecord.GetResult));

		action
			.Should()
			.ThrowExactly<InvalidCastException>();
	}
}
