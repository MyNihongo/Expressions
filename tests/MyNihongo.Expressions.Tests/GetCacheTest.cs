namespace MyNihongo.Expressions.Tests
{
	public abstract record GetCacheTest
	{
		public string Prop { get; set; } = string.Empty;
	}

	public sealed record GetCacheTest1 : GetCacheTest;
	public sealed record GetCacheTest2 : GetCacheTest;
}
