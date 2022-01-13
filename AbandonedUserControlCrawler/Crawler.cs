using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AbandonedUserControlCrawler
{
	/// <summary>
	/// Crawler to check all files of specified type for TagName occurences (control register) that are never used
	/// </summary>
	public class Crawler
	{
		/// <summary>
		/// Crawl to check all files of specified type for TagName occurences (control register) that are never used
		/// </summary>
		/// <param name="sPath">Folder path to check</param>
		/// <param name="sSearchPattern">file type patterns for DirectoryInfo.GetFiles function</param>
		/// <param name="sFileType">file type patterns for DirectoryInfo.GetFiles function</param>
		public void Crawl(string sPath, string sSearchPattern = "TagName=\"(.*?)\"", string sFileType = "*.as*x")
		{
			if(CheckFolderPath(sPath))
			{
				try
				{
					FileInfo[] files = new DirectoryInfo(sPath).GetFiles(sFileType, SearchOption.AllDirectories);

					Console.WriteLine($"Found {files.Length} {sFileType} files at provided path {sPath}");

					if(files.Length > 0)
						Console.WriteLine($"Program will now list any files containing controls registered but not used:");

					StringBuilder sbFoundTags = new StringBuilder();
					string sFileContent;
					int nFound;

					for (int i = 0; i < files.Length; i++)
					{
						sFileContent = File.ReadAllText(files[i].FullName);
						var matches = Regex.Matches(sFileContent, sSearchPattern);
						nFound = 0;

						foreach (Match item in matches)
						{
							if (item.Success && GetNumberOfTextInTextOccurences(sFileContent, item.Groups[1].Value) == 1)
							{
								if (nFound == 0)
									sbFoundTags.Append($@"
{files[i].Name}: {item.Groups[1].Value}");
								else
									sbFoundTags.Append($", {item.Groups[1].Value}");

								nFound++;
							}
						}
					}

					Console.WriteLine($"{(sbFoundTags.Length > 0 ? sbFoundTags : "Nothing found.")}");

				}
				catch (Exception exc)
				{
					Console.WriteLine($"Could not process files on path with exception: {exc}");
					//throw;
				}

			}

			Console.WriteLine();
			Console.WriteLine("Work done.");
		}

		private bool CheckFolderPath(string sPath)
		{
			sPath = sPath?.Trim();

			if (sPath?.Length == 0)
			{
				Console.WriteLine("Folder path not provided.");
				return false;
			}
			else if (Directory.Exists(sPath) == false)
			{
				Console.WriteLine($"Folder {sPath} could not be found.");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Find how many times the sValue occurs in the sText
		/// </summary>
		/// <param name="sText">Long text to find occurencies in</param>
		/// <param name="sValue">text to search for</param>
		/// <returns>number of occurencies of sValue in sText</returns>
		private int GetNumberOfTextInTextOccurences(string sText, string sValue)
		{
			return (sText.Length - sText.ToLower().Replace(sValue.ToLower(), string.Empty).Length) / sValue.Length;
		}
	}
}
