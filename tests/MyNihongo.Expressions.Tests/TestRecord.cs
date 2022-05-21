namespace MyNihongo.Expressions.Tests;

public abstract record TestRecordBase;

public sealed record TestRecord : TestRecordBase
{
	public string Text { get; set; } = string.Empty;

	public int GetResult() =>
		123;

	public int GetAnotherResult(int param) =>
		123 + param;
}
