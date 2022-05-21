namespace MyNihongo.Expressions.Tests.CastToTests;

public sealed class FromShould
{
	[Fact]
	public void CastCorrectly()
	{
		var item = new TestRecord();

		var result = CastTo<TestRecordBase>.From(item);

		result
			.Should()
			.Be(item);
	}

	[Fact]
	public void ThrowExceptionIfCastNotValid()
	{
		var item = new GetCacheTest2();

		Action action = () => CastTo<GetCacheTest1>.From(item);

		action
			.Should()
			.ThrowExactly<TypeInitializationException>();
	}
}
