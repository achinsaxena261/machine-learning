using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalLanguageProcessing
{
	public class ReportedSpechIdentifier
	{
		public static string[] IndentifyProperNounGroups(string[] tokens, string[] tags)
		{
			List<string> NounCollection = new List<string>();
			StringBuilder temp = new StringBuilder();
			for (int index = 0; index < tokens.Length; index++)
			{
				if (tags[index] == "NNP" || tags[index] == "NNPS")
				{
					temp.Append(tokens[index] + " ");
				}
				else if (temp.Length != 0 && (tags[index] == "NN" || tags[index] == "NNS"))
				{
					temp.Append(tokens[index] + " ");
				}
				else
				{
					if (temp.Length != 0)
					{
						NounCollection.Add(temp.ToString().Trim());
						temp.Clear();
					}
				}
			}
			//If last token of string is also proper noun
			if (temp.Length != 0)
			{
				NounCollection.Add(temp.ToString().Trim());
				temp.Clear();
			}

			return NounCollection.ToArray();
		}

		private static bool ThirdPersonMentioned(string[] tokens, string[] tags)
		{
			return tags.Contains("VBZ");
		}
		public static bool IsReportedSpeech(string[] tokens, string[] tags)
		{
			var intersect = POS.DirectPronoun.Intersect<string>(tokens);
			bool foundPresentVerb = false;
			for (int order = 0; order < tags.Length; order++) {
				if (tags[order].Equals("VB") && tags[order].Equals("VBG") && tags[order].Equals("VBP"))
				{
					if (order > 0 && tags[order - 1].Equals("TO"))
						foundPresentVerb = false;
					else
						foundPresentVerb = true;
				}
			}
			return !foundPresentVerb && intersect.Count() == 0;
		}
	}
}
