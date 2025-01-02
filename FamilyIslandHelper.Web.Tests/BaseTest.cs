using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FamilyIslandHelper.Web.Tests
{
	public class BaseTest
	{
		protected WebDriver driver;
		protected string mainUrl = "https://gruuuubik.bsite.net";
		protected string testDataFolder = "TestData";
		private const string ScreensDirectory = "Screens";

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			if (!Directory.Exists(ScreensDirectory))
			{
				Directory.CreateDirectory(ScreensDirectory);
			}

			driver = new ChromeDriver();
			driver.Manage().Window.Maximize();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			driver.Quit();
			driver.Dispose();
		}

		protected void MakeScreen(string stepName)
		{
			((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(Path.Combine(ScreensDirectory, $"{TestContext.CurrentContext.Test.MethodName}_{stepName}.png"));
		}

		protected Dictionary<string, string> GetTestData(string testDataJsonFile)
		{
			var pathToTestDataJson = Path.Combine(testDataFolder, $"{testDataJsonFile}.json");
			var jsonString = File.ReadAllText(pathToTestDataJson);

			return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
		}
	}
}
