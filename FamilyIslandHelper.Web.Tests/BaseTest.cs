using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace FamilyIslandHelper.Web.Tests
{
	public class BaseTest
	{
		protected WebDriver driver;
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
	}
}
