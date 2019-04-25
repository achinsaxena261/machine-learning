using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ContentModerator
{
	class Program
	{
		//The name of the file that contains the image URLs to evaluate.
		private static string ImageUrlFile = "ImageFiles.txt";

		///The name of the file to contain the output from the evaluation.
		private static string OutputFile = "ModerationOutput.json";
		static void Main(string[] args)
		{
			// Create an object to store the image moderation results.
			List<EvaluationData> evaluationData = new List<EvaluationData>();

			// Create an instance of the Content Moderator API wrapper.
			using (var client = Clients.NewClient())
			{
				// Read image URLs from the input file and evaluate each one.
				using (StreamReader inputReader = new StreamReader(ImageUrlFile))
				{
					while (!inputReader.EndOfStream)
					{
						string line = inputReader.ReadLine().Trim();
						if (line != String.Empty)
						{
							EvaluationData imageData = EvaluateImage(client, line);
							evaluationData.Add(imageData);
						}
					}
				}
			}

			// Save the moderation results to a file.
			using (StreamWriter outputWriter = new StreamWriter(OutputFile, false))
			{
				outputWriter.WriteLine(JsonConvert.SerializeObject(
					evaluationData, Formatting.Indented));

				outputWriter.Flush();
				outputWriter.Close();
			}
		}

		// Evaluates an image using the Image Moderation APIs.
		private static EvaluationData EvaluateImage(
		  ContentModeratorClient client, string imageUrl)
		{
			var url = new BodyModel("URL", imageUrl.Trim());

			var imageData = new EvaluationData();

			imageData.ImageUrl = url.Value;

			// Evaluate for adult and racy content.
			imageData.ImageModeration =
				client.ImageModeration.EvaluateUrlInput("application/json", url, true);
			Thread.Sleep(1000);

			// Detect and extract text.
			imageData.TextDetection =
				client.ImageModeration.OCRUrlInput("eng", "application/json", url, true);
			Thread.Sleep(1000);

			// Detect faces.
			imageData.FaceDetection =
				client.ImageModeration.FindFacesUrlInput("application/json", url, true);
			Thread.Sleep(1000);

			return imageData;
		}
	}
}
