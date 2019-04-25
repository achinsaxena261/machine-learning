using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using Microsoft.Azure.CognitiveServices.Search.WebSearch.Models;
using System.Linq;

namespace AzureWebSearch
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = Clients.NewClient();

			WebResults(client);

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		public static async void WebResults(WebSearchClient client)
		{
			try
			{
				var webData = await client.Web.SearchAsync(query: "Congress party won the general election of 2019, Rahul will be the next PM");
				Console.WriteLine("Searching for \"Rahul Gandhi is pappu\"");

				if (webData?.WebPages?.Value?.Count > 0)
				{
					// find the first web page
					var firstWebPagesResult = webData.WebPages.Value.FirstOrDefault();

					if (firstWebPagesResult != null)
					{
						Console.WriteLine("Webpage Results # {0}", webData.WebPages.Value.Count);
						Console.WriteLine("First web page name: {0} ", firstWebPagesResult.Name);
						Console.WriteLine("First web page URL: {0} ", firstWebPagesResult.Url);
					}
					else
					{
						Console.WriteLine("Didn't find any web pages...");
					}
				}
				else
				{
					Console.WriteLine("Didn't find any web pages...");
				}

				/*
				 * Images
				 * If the search response contains images, the first result's name
				 * and url are printed.
				 */
				if (webData?.Images?.Value?.Count > 0)
				{
					// find the first image result
					var firstImageResult = webData.Images.Value.FirstOrDefault();

					if (firstImageResult != null)
					{
						Console.WriteLine("Image Results # {0}", webData.Images.Value.Count);
						Console.WriteLine("First Image result name: {0} ", firstImageResult.Name);
						Console.WriteLine("First Image result URL: {0} ", firstImageResult.ContentUrl);
					}
					else
					{
						Console.WriteLine("Didn't find any images...");
					}
				}
				else
				{
					Console.WriteLine("Didn't find any images...");
				}

				/*
				 * News
				 * If the search response contains news articles, the first result's name
				 * and url are printed.
				 */
				if (webData?.News?.Value?.Count > 0)
				{
					// find the first news result
					var firstNewsResult = webData.News.Value.FirstOrDefault();

					if (firstNewsResult != null)
					{
						Console.WriteLine("\r\nNews Results # {0}", webData.News.Value.Count);
						Console.WriteLine("First news result name: {0} ", firstNewsResult.Name);
						Console.WriteLine("First news result URL: {0} ", firstNewsResult.Url);
					}
					else
					{
						Console.WriteLine("Didn't find any news articles...");
					}
				}
				else
				{
					Console.WriteLine("Didn't find any news articles...");
				}

				/*
				 * Videos
				 * If the search response contains videos, the first result's name
				 * and url are printed.
				 */
				if (webData?.Videos?.Value?.Count > 0)
				{
					// find the first video result
					var firstVideoResult = webData.Videos.Value.FirstOrDefault();

					if (firstVideoResult != null)
					{
						Console.WriteLine("\r\nVideo Results # {0}", webData.Videos.Value.Count);
						Console.WriteLine("First Video result name: {0} ", firstVideoResult.Name);
						Console.WriteLine("First Video result URL: {0} ", firstVideoResult.ContentUrl);
					}
					else
					{
						Console.WriteLine("Didn't find any videos...");
					}
				}
				else
				{
					Console.WriteLine("Didn't find any videos...");
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine("Encountered exception. " + ex.Message);
			}
		}
	}
}
