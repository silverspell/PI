using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace PIWorksQuestionnaire
{
	class MainClass
	{
		const string FILENAME = @"exhibitA-input.csv";
		const string OUTFILE = @"output.txt";

		public static void Main (string[] args)
		{
			var lines = File.ReadLines (FILENAME).Select(line => line.Split('\t'));
			var results = (from l in lines
			               where l [3].StartsWith ("10/08/2016")
			               select new {
										SONG_ID = Convert.ToInt32 (l [1]), 
										CLIENT_ID = Convert.ToInt32 (l [2])
										}
			              ).Distinct ()
							.GroupBy (x => x.CLIENT_ID)
							.Select (x => new {
								CLIENT_ID = x.Key,
								CLIENT_DIST_COUNT = x.Count ()
							})
							.GroupBy (x => x.CLIENT_DIST_COUNT)
							.Select (x => new {
								DIST_COUNT = x.Key,
								CLIENT_COUNT = x.Count ()
				});
					//		.OrderByDescending(x => x.DIST_COUNT);





			File.WriteAllLines (OUTFILE, new string[]{ @"DISTINCT_PLAY_COUNT	CLIENT_COUNT" });
			File.AppendAllLines (OUTFILE, results.Select(x => x.DIST_COUNT + "\t" + x.CLIENT_COUNT));
		}
	}
}
