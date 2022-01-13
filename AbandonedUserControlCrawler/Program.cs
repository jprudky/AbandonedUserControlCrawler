using System;

namespace AbandonedUserControlCrawler
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Path to the folder to check .as*x files for abandoned registered controls in page/control definition:");

			string sPath = Console.ReadLine();

			new Crawler().Crawl(sPath); // default parameter values sSearchPattern = "TagName=\"(.*?)\"", sFileType = "*.as*x"

			Console.WriteLine("Press a key to exit.");
			Console.ReadKey();
		}
	}
}
