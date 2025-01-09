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
			driver.Navigate().GoToUrl(mainUrl);
		}

		[Test]
		[Description(@"
			1. Navigate to FamilyIslandHelper.Web.
				1.1. Check buildings count.
			2. Select API version.
				2.1. Check buildings count.
			3. Select Building.
				3.1. Check that building is selected.
				3.2. Check that the 1st item from this building is selected.
				3.3. Check item info.
				3.4. Check total time info.
				3.5. Check that another item from this building is not selected.
			4. Select Item 2 from current building.
				4.1. Check that Item 2 from this building is selected.
				4.2. Check item info.
			5. Turn on showing of list of components.
				5.1. Check that item info contains Components info.
				5.2. Check that components tree contains all necessary components.
			6. Select producable item in components tree.
				6.1. Check that producable item is not visible.
			7. Set ItemCount = 2.
				7.1. Check that item info is updated.
		")]
		[TestCase("ApiVersionHome1", 13, "JewelryWorkshop", "PearlEarrings", "01:15:00, 180", "03:15:00", "SapphireBracelet", "01:15:00, 485", 25, "Sackcloth", "Cloth", "02:30:00, 970", 1)]
		[TestCase("ApiVersionHome2", 17, "Forge", "Needle", "00:08:00, 216", "00:08:00", "Glass", "00:35:00, 1928", 8, "Grass", "Axe", "01:10:00, 3856", 2)]
		public void HomePageTest(string apiVersionId, int buildingsCount, string buildingId, string item1Id, string item1Info, string totalTimeInfo1, string item2Id, string item2Info, int expectedComponentsTreeImagesCount, string componentsTreeItemComponentImageId, string componentsTreeItemImageId, string item1InfoFor2, int testCaseNumber)
		{
			try
			{
				Assert.That(driver.FindElements(By.ClassName("buildingImageName")), Has.Count.EqualTo(17));

				driver.FindElement(By.Id(apiVersionId)).Click();

				Assert.That(driver.FindElements(By.ClassName("buildingImageName")), Has.Count.EqualTo(buildingsCount));

				var building = driver.FindElement(By.Id(buildingId));
				building.Click();

				building = driver.FindElement(By.Id(buildingId));
				Assert.That(building.GetAttribute("class"), Is.EqualTo(ClassesForSelectedBuilding));

				var item1 = driver.FindElement(By.Id(item1Id));
				Assert.That(item1.GetAttribute("class"), Is.EqualTo("itemNameImage selectedImage"));

				var itemInfo = driver.FindElement(By.Id(ItemInfoId));
				Assert.That(itemInfo.Text, Does.Contain(item1Info));

				//3.4
				var totalTimeInfo = driver.FindElement(By.Id("totalTimeInfo"));
				Assert.That(totalTimeInfo.Text, Is.EqualTo(totalTimeInfo1));

				var item2 = driver.FindElement(By.Id(item2Id));
				Assert.That(item2.GetAttribute("class"), Is.EqualTo("itemNameImage"));
				item2.Click();
				item2 = driver.FindElement(By.Id(item2Id));
				Assert.That(item2.GetAttribute("class"), Is.EqualTo("itemNameImage selectedImage"));

				itemInfo = driver.FindElement(By.Id(ItemInfoId));
				Assert.That(itemInfo.Text, Does.Contain(item2Info));

				driver.FindElement(By.Id("ShowListOfComponents")).Click();

				//5.1
				itemInfo = driver.FindElement(By.Id(ItemInfoId));
				Assert.That(itemInfo.Text, Does.Contain("Components"));

				//5.2
				var componentsTree = driver.FindElement(By.Id("componentsTree"));
				var componentsTreeImages = componentsTree.FindElements(By.TagName("img"));
				Assert.That(componentsTreeImages, Has.Count.EqualTo(expectedComponentsTreeImagesCount));

				var itemComponentImage = driver.FindElement(By.Id(componentsTreeItemComponentImageId));
				Assert.That(itemComponentImage.Displayed, Is.True);
				driver.FindElement(By.Id(componentsTreeItemImageId)).Click();

				//6.1
				Assert.That(itemComponentImage.Displayed, Is.False);

				//7
				var itemCount = driver.FindElement(By.Id("ItemCount"));
				new Actions(driver).MoveToElement(itemCount).Click().SendKeys(Keys.Backspace).SendKeys("2").SendKeys(Keys.Enter).Perform();

				itemInfo = driver.FindElement(By.Id(ItemInfoId));
				Assert.That(itemInfo.Text, Does.Contain(item1InfoFor2));
			}
			catch
			{
				MakeScreen($"TestCase{testCaseNumber}_Exception");

				throw;
			}
		}
	}
}