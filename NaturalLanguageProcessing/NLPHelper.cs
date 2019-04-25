using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalLanguageProcessing
{
	public class NLPHelper
	{
		private string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
		private string mModelPath => Path.Combine(_appPath, "..", "..", "mModel\\");
		private OpenNLP.Tools.SentenceDetect.MaximumEntropySentenceDetector mSentenceDetector;
		private OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer mTokenizer;
		private OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger mPosTagger;
		private OpenNLP.Tools.Chunker.EnglishTreebankChunker mTreebankChunker;

		private string[] SplitSentences(string paragraph)
		{
			if (mSentenceDetector == null)
			{
				mSentenceDetector = new OpenNLP.Tools.SentenceDetect.EnglishMaximumEntropySentenceDetector(mModelPath + "EnglishSD.nbin");
			}
			return mSentenceDetector.SentenceDetect(paragraph);
		}

		private string[] TokenizeSentence(string sentence)
		{
			if (mTokenizer == null)
			{
				mTokenizer = new OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer(mModelPath + "EnglishTok.nbin");
			}
			return mTokenizer.Tokenize(sentence);
		}

		private string[] TreebankChunker(string[] tokens, string[] tags)
		{
			if (mTreebankChunker == null)
			{
				mTreebankChunker = new OpenNLP.Tools.Chunker.EnglishTreebankChunker(mModelPath + "EnglishChunk.nbin");
			}
			return mTreebankChunker.Chunk(tokens, tags);
		}

		private string[] PosTagTokens(string[] tokens)
		{
			if (mPosTagger == null)
			{
				mPosTagger = new OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger(mModelPath + "EnglishPOS.nbin", mModelPath + @"\Parser\tagdict");
			}
			return mPosTagger.Tag(tokens);
		}

		public void POSTagger_Method(string sent)
		{
			//File.WriteAllText("POSTagged.txt", sent + "\n\n");
			string[] split_sentences = SplitSentences(sent);
			foreach (string sentence in split_sentences)
			{
				//File.AppendAllText("POSTagged.txt", sentence + "\n");
				Console.WriteLine("\nSentence : {0}", sentence);
				string[] tokens = TokenizeSentence(sentence);
				string[] tags = PosTagTokens(tokens);
				string[] chunks = TreebankChunker(tokens, tags);

				Console.WriteLine("is reported speech? : {0}", ReportedSpechIdentifier.IsReportedSpeech(tokens, tags));
				for (int currentTag = 0; currentTag < tags.Length; currentTag++)
				{
					Console.WriteLine("Token - {0} , Tag - {1} , Chunk - {2}", tokens[currentTag], tags[currentTag], chunks[currentTag]);
					//File.AppendAllText("POSTagged.txt", tokens[currentTag] + " - " + tags[currentTag] + "\n");
				}
				//File.AppendAllText("POSTagged.txt", "\n\n");
			}
			Console.Read();
		}

	}
}
