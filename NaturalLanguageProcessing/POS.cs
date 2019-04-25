using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NaturalLanguageProcessing
{
	public class POS
	{
		public string Token { get; set; }
		public string Tag { get; set; }
		public string Chunk { get; set; }
		public static readonly IList<string> DirectPronoun = new ReadOnlyCollection<string>(new List<string> {
			"I", "am", "we", "me", "myself", "you", "us", "today", "my", "yesterday", "mine", "?"
		});
	}
}
