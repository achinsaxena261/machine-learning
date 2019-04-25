using Microsoft.ML.Data;

namespace FakeNewsAnalysis
{
	public class NewsType
	{
		[LoadColumn(0)]
		public string Id { get; set; }
		[LoadColumn(1)]
		public string Title { get; set; }
		[LoadColumn(2)]
		public string Text { get; set; }
		[LoadColumn(3)]
		public string Type { get; set; }
	}

	public class TypePrediction
	{
		[ColumnName("PredictedLabel")]
		public string Type;
	}
}
