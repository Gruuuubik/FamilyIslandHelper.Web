using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace FamilyIslandHelper.Web.Tests
{
	[TestFixture]
	public class HomeControllerTest : BaseTest
	{
		private const string ClassesForSelectedBuilding = "buildingImageName selectedImage";
		private const string ItemInfoId = "ItemInfo";

		[SetUp]
		public void Setup()
		{
			driver.Navigate().GoToUrl("https://gruuuubik.bsite.net/");
		}

		[Test]
		[TestCase("ApiVersionHome1", "JewelryWorkshop", "PearlEarrings", "01:15:00, 180", "SapphireBracelet", "01:15:00, 485", "02:30:00, 970", 1)]
		[TestCase("ApiVersionHome2", "Forge", "Needle", "00:08:00, 216", "Glass", "00:35:00, 1928", "01:10:00, 3856", 2)]
		public void HomePageTest(string apiVersionId, string buildingId, string item1Id, string item1Info, string item2Id, string item2Info, string item1InfoFor2, int testCaseNumber)
		{
			driver.FindElement(By.Id(apiVersionId)).Click();

			var building = driver.FindElement(By.Id(buildingId));
			building.Click();

			building = driver.FindElement(By.Id(buildingId));
			Assert.That(building.GetAttribute("class"), Is.EqualTo(ClassesForSelectedBuilding));

			var item = driver.FindElement(By.Id(item1Id));
			Assert.That(item.GetAttribute("class"), Is.EqualTo("itemNameImage selectedImage"));

			var itemInfo = driver.FindElement(By.Id(ItemInfoId));		
			Assert.That(itemInfo.Text, Does.Contain(item1Info));

			item = driver.FindElement(By.Id(item2Id));
			Assert.That(item.GetAttribute("class"), Is.EqualTo("itemNameImage"));
			item.Click();
			item = driver.FindElement(By.Id(item2Id));
			Assert.That(item.GetAttribute("class"), Is.EqualTo("itemNameImage selectedImage"));

			itemInfo = driver.FindElement(By.Id(ItemInfoId));

			Assert.That(itemInfo.Text, Does.Contain(item2Info));

			driver.FindElement(By.Id("ShowListOfComponents")).Click();
			itemInfo = driver.FindElement(By.Id(ItemInfoId));
			Assert.That(itemInfo.Text, Does.Contain("Components"));

			var itemCount = driver.FindElement(By.Id("ItemCount"));
			new Actions(driver).MoveToElement(itemCount).Click().SendKeys(Keys.Backspace).SendKeys("2").SendKeys(Keys.Enter).Perform();

			itemInfo = driver.FindElement(By.Id(ItemInfoId));
			Assert.That(itemInfo.Text, Does.Contain(item1InfoFor2));

			MakeScreen($"TestCase{testCaseNumber}Finish");
		}
	}
}