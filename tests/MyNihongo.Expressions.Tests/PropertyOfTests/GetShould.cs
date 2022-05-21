namespace MyNihongo.Expressions.Tests.PropertyOfTests;

public sealed class GetShould
{
	[Fact]
	public void GetPropertyValueNonGeneric()
	{
		const string text = nameof(text);
		var input = new TestRecord { Text = text };

		var result = PropertyOf.Get<string>(input, nameof(TestRecord.Text));

		result
			.Should()
			.Be(text);
	}

	[Fact]
	public void GetPropertyValueGeneric()
	{
		const string text = nameof(text);
		var input = new TestRecord { Text = text };

		var result = PropertyOf<TestRecord>.Get<string>(input, nameof(TestRecord.Text));

		result
			.Should()
			.Be(text);
	}

	[Fact]
	public void SaveExpressionsInCacheFromNonGeneric()
	{
		var item = new GetCacheTest1
		{
			Prop = "text123"
		};

		const string prop = nameof(item.Prop);
		var key = new Tuple<Type, string>(item.GetType(), prop);

		var dictionary = (ConcurrentDictionary<Tuple<Type, string>, Lazy<Delegate>>)typeof(ExpressionCache)
			.GetField(nameof(ExpressionCache.PropertyGetters), BindingFlags.Static | BindingFlags.NonPublic)
			!.GetValue(null);

		dictionary
			.Should()
			.NotContainKey(key);

		PropertyOf.Get(item, prop);

		dictionary
			.Should()
			.ContainKey(key);
	}

	[Fact]
	public void SaveExpressionsInCacheFromGeneric()
	{
		var item = new GetCacheTest2
		{
			Prop = "text123"
		};

		const string prop = nameof(item.Prop);
		var key = new Tuple<Type, string>(item.GetType(), prop);

		var dictionary = (ConcurrentDictionary<Tuple<Type, string>, Lazy<Delegate>>)typeof(ExpressionCache)
			.GetField(nameof(ExpressionCache.PropertyGetters), BindingFlags.Static | BindingFlags.NonPublic)
			!.GetValue(null);

		dictionary
			.Should()
			.NotContainKey(key);

		PropertyOf<GetCacheTest2>.Get<string>(item, prop);

		dictionary
			.Should()
			.ContainKey(key);
	}

	[Fact]
	public void ThrowExceptionIfPropertyNotFoundNonGeneric()
	{
		var input = new TestRecord();

		Action action = () => PropertyOf.Get(input, "NotPresent");

		action
			.Should()
			.ThrowExactly<ArgumentException>();
	}

	[Fact]
	public void ThrowExceptionIfPropertyNotFoundGeneric()
	{
		var input = new TestRecord();

		Action action = () => PropertyOf<TestRecord>.Get<string>(input, "NotPresent");

		action
			.Should()
			.ThrowExactly<ArgumentException>();
	}

	[Fact]
	public void ThrowExceptionIfInvalidTypeNonGeneric()
	{
		var input = new TestRecord();

		Action action = () => PropertyOf.Get<int>(input, nameof(TestRecord.Text));

		action
			.Should()
			.ThrowExactly<InvalidCastException>();
	}

	[Fact]
	public void ThrowExceptionIfInvalidTypeGeneric()
	{
		var input = new TestRecord();

		Action action = () => PropertyOf<TestRecord>.Get<int>(input, nameof(TestRecord.Text));

		action
			.Should()
			.ThrowExactly<InvalidCastException>();
	}
}
