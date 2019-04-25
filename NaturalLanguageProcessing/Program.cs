using System;
using System.Collections.Generic;
using System.IO;

namespace NaturalLanguageProcessing
{
	class Program
	{
		private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
		private static string _filePath => Path.Combine(_appPath, "..", "..", "TestSample\\");
		static void Main(string[] args)
		{
			NLPHelper nlp = new NLPHelper();
			string text = File.ReadAllText(_filePath + "sample.txt");
			nlp.POSTagger_Method(text);
		}
	}
}
