using System;
using System.IO;
using System.Linq;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace FakeNewsAnalysis
{
	class Program
	{
		private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
		private static string _trainDataPath => Path.Combine(_appPath, "..", "..", "..", "Data", "fake_or_real_news.csv");
		private static string _modelPath => Path.Combine(_appPath, "..", "..", "..", "Models", "model.zip");

		private static MLContext _mlContext;
		private static PredictionEngine<NewsType, TypePrediction> _predEngine;
		private static ITransformer _trainedModel;
		static IDataView _trainingDataView;
		static void Main(string[] args)
		{
			_mlContext = new MLContext(seed: 0);
			var reader = _mlContext.Data.CreateTextLoader(
				columns: new TextLoader.Column[]
				{
					// The four features of the Iris dataset will be grouped together as one Features column.
					new TextLoader.Column("Id",DataKind.String,0),
					new TextLoader.Column("Title",DataKind.String,1),
					new TextLoader.Column("Text",DataKind.String,2),
					new TextLoader.Column("Type",DataKind.String,4)
				},
				// Default separator is tab, but the dataset has semicolon.
				separatorChar: ',',
				// First line of the file is a header, not a data row.
				hasHeader: true
			);
			_trainingDataView = reader.Load(_trainDataPath);
			var pipeline = ProcessData();
			var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);
			SaveModelAsFile(_mlContext, _trainedModel);
			Console.Read();
		}

		public static IEstimator<ITransformer> ProcessData()
		{
			var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Type", outputColumnName: "Label")
				.Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title", outputColumnName: "TitleFeaturized"))
				.Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Text", outputColumnName: "DescriptionFeaturized"))
				.Append(_mlContext.Transforms.Concatenate("Features", "TitleFeaturized", "DescriptionFeaturized"));
			return pipeline;
		}

		public static IEstimator<ITransformer> BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
		{
			var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(DefaultColumnNames.Label, DefaultColumnNames.Features));
			TrainCatalogBase.TrainTestData splitDataSet = _mlContext.MulticlassClassification.TrainTestSplit(trainingDataView, testFraction: 0.1);
			_trainedModel = trainingPipeline.Fit(splitDataSet.TrainSet);
			_predEngine = _trainedModel.CreatePredictionEngine<NewsType, TypePrediction>(_mlContext);

			NewsType news = new NewsType()
			{
				Title = "Indian Election of loksabha",
				Text = "Rahul gandhi is going to become the next prime minister of india as per early reports",
			};

			var prediction = _predEngine.Predict(news);
			Console.WriteLine($"=============== Single Prediction just-trained-model - Result: {prediction.Type} ===============");
			return trainingPipeline;
		}

		private static void SaveModelAsFile(MLContext mlContext, ITransformer model)
		{
			using (var fs = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
				mlContext.Model.Save(model, fs);
			Console.WriteLine("The model is saved to {0}", _modelPath);
		}
	}
}
