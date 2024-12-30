using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace FamilyIslandHelper.Web.Tests
{
	[TestFixture]
	public class CompareControllerTest : BaseTest
	{
		private const string ClassesForSelectedBuilding = "buildingImageName selectedImage";

		[SetUp]
		public void Setup()
		{
			driver.Navigate().GoToUrl("https://gruuuubik.bsite.net/Compare");
		}

		[Test]
		[Description(@"
			1. Navigate to page Compare of FamilyIslandHelper.Web.
			2. Select API version.
			3. Check Component 1.
				3.1. Select building.
					3.1.1. Check that building is selected.
					3.1.2. Check that the 1st item from this building is selected.
					3.1.3. Check item info.
					3.1.4. Check that another item from this building is not selected.
				3.2. Select Item 2 from current building.
					3.2.1. Check that Item 2 from this building is selected.
					3.2.2. Check item info.
				3.3. Turn on showing of list of components.
					3.3.1. Check that item info contains Components info.
				3.4. Set ItemCount = 2.
					3.4.1. Check that item info is updated.
			4. Turn off showing of list of components.
			5. Check Component 2.
				5.1. Select building.
					5.1.1. Check that building is selected.
					5.1.2. Check that the 1st item from this building is selected.
					5.1.3. Check item info.
					5.1.4. Check that another item from this building is not selected.
				5.2. Select Item 2 from current building.
					5.2.1. Check that Item 2 from this building is selected.
					5.2.2. Check item info.
				5.3. Turn on showing of list of components.
					5.3.1. Check that item info contains Components info.
				5.4. Set ItemCount = 2.
					5.4.1. Check that item info is updated.
			6. Check item compare info.
			7. Make screenshot.
		")]
		[TestCase("ApiVersionCompare1", "JewelryWorkshop", "PearlEarrings", "01:15:00, 180", "SapphireBracelet", "01:15:00, 485", "02:30:00, 970", 1)]
		[TestCase("ApiVersionCompare2", "Forge", "Needle", "00:08:00, 216", "Glass", "00:35:00, 1928", "01:10:00, 3856", 2)]
		public void ComparePageTest(string apiVersionId, string buildingId, string item1Id, string item1Info, string item2Id, string item2Info, string item1InfoFor2, int testCaseNumber)
		{
			driver.FindElement(By.Id(apiVersionId)).Click();

			CheckComponent(buildingId, item1Id, item1Info, item2Id, item2Info, item1InfoFor2, "Component1", "ItemInfo1", "ItemCount1");

			driver.FindElement(By.Id("ShowListOfComponentsForAll")).Click();

			CheckComponent(buildingId, item1Id, item1Info, item2Id, item2Info, item1InfoFor2, "Component2", "ItemInfo2", "ItemCount2");

			Assert.That(driver.FindElement(By.Id("ItemCompareInfo")).Text, Is.Not.Empty);

			MakeScreen($"TestCase{testCaseNumber}Finish");
		}

		private void CheckComponent(string buildingId, string item1Id, string item1Info, string item2Id, string item2Info, string item1InfoFor2, string componentId, string itemInfoId, string itemCountId)
		{
			var component = driver.FindElement(By.Id(componentId));

			var building = component.FindElement(By.Id(buildingId));
			building.Click();

			component = driver.FindElement(By.Id(componentId));
			building = component.FindElement(By.Id(buildingId));
			Assert.That(building.GetAttribute("class"), Is.EqualTo(ClassesForSelectedBuilding));

			var item1 = component.FindElement(By.Id(item1Id));
			Assert.That(item1.GetAttribute("class"), Is.EqualTo("itemNameImage selectedImage"));

			var itemInfo = component.FindElement(By.Id(itemInfoId));
			Assert.That(itemInfo.Text, Does.Contain(item1Info));

			var item2 = component.FindElement(By.Id(item2Id));
			Assert.That(item2.GetAttribute("class"), Is.EqualTo("itemNameImage"));
			item2.Click();
			component = driver.FindElement(By.Id(componentId));
			item2 = component.FindElement(By.Id(item2Id));
			Assert.That(item2.GetAttribute("class"), Is.EqualTo("itemNameImage selectedImage"));

			itemInfo = component.FindElement(By.Id(itemInfoId));
			Assert.That(itemInfo.Text, Does.Contain(item2Info));

			driver.FindElement(By.Id("ShowListOfComponentsForAll")).Click();

			component = driver.FindElement(By.Id(componentId));
			itemInfo = component.FindElement(By.Id(itemInfoId));
			Assert.That(itemInfo.Text, Does.Contain("Components"));

			var itemCount = component.FindElement(By.Id(itemCountId));
			new Actions(driver).MoveToElement(itemCount).Click().SendKeys(Keys.Backspace).SendKeys("2").SendKeys(Keys.Enter).Perform();

			component = driver.FindElement(By.Id(componentId));
			itemInfo = component.FindElement(By.Id(itemInfoId));
			Assert.That(itemInfo.Text, Does.Contain(item1InfoFor2));
		}
	}
}