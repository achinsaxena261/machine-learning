using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning.Bayes;
using Accord.Math;
using Accord.Statistics.Analysis;
using Accord.Statistics.Filters;
using Microsoft.VisualBasic.FileIO;

namespace NaiveBayesClassification
{
	class Program
	{
		private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
		private static string _trainDataPath => Path.Combine(_appPath, "..", "..", "Data", "fake_or_real_news.csv");
		static void Main(string[] args)
		{
			DataTable data = new DataTable("Dataset of fake news");
			using (TextFieldParser parser = new TextFieldParser(_trainDataPath))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				int rowNum = 0;
				while (!parser.EndOfData)
				{
					//Processing row
					string[] fields = parser.ReadFields();
					if (rowNum == 0)
					{
						foreach (string field in fields)
						{
							data.Columns.Add(field);
						}
					}
					data.Rows.Add(fields);
					rowNum++;
				}
			}

			// Create a new codification codebook to
			// convert strings into discrete symbols
			Codification codebook = new Codification(data,
				"Id", "Title", "Text", "Type");

			// Extract input and output pairs to train
			DataTable symbols = codebook.Apply(data);
			int[][] inputs = symbols.ToArray<int>("Title", "Text");
			int[] outputs = symbols.ToArray<int>("Type");

			// Create a new Naive Bayes learning
			var learner = new NaiveBayesLearning();

			// Learn a Naive Bayes model from the examples
			NaiveBayes nb = learner.Learn(inputs, outputs);

			int[] instance = codebook.Translate(data.Rows[50], "Id", "Title", "Text");

			// Let us obtain the numeric output that represents the answer
			int c = nb.Decide(instance); // answer will be 0

			// Now let us convert the numeric output to an actual 
			string result = codebook.Translate("Type", c);

			Console.WriteLine("The news is {0}", result);
			// We can also extract the probabilities for each possible answer
			double[] probs = nb.Probabilities(instance);
			Console.Read();
		}
	}
}
