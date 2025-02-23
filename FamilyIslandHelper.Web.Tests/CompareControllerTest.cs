using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;

namespace FamilyIslandHelper.Web.Tests
{
	[TestFixture]
	public class CompareControllerTest : BaseTest
	{
		private const string ClassesForSelectedBuilding = "buildingImageName selectedImage";

		[SetUp]
		public void Setup()
		{
			driver.Navigate().GoToUrl($"{mainUrl}/Compare");
		}

		[Test]
		[Description(@"
			1. Navigate to page Compare of FamilyIslandHelper.Web.
			2. Select API version.
				2.1. Check buildings count.
			3. Check Component 1.
				3.1. Select building.
					3.1.1. Check that building is selected.
					3.1.2. Check that the 1st item from this building is selected.
					3.1.3. Check total time info.
					3.1.4. Check that another item from this building is not selected.
				3.2. Select Item 2 from current building.
					3.2.1. Check that Item 2 from this building is selected.
					3.2.2. Check that components tree contains all necessary components.
			4. Turn on showing of components with time and energy.
			5. Set Items count = 2.
			6. Check Component 2.
				6.1. Select building.
					6.1.1. Check that building is selected.
					6.1.2. Check that the 1st item from this building is selected.
					6.1.3. Check item info.
					6.1.4. Check total time info.
					6.1.5. Check that another item from this building is not selected.
				6.2. Select Item 2 from current building.
					6.2.1. Check that Item 2 from this building is selected.
					6.2.2. Check item info.
					6.2.3. Check that item info contains Components info.
					6.2.4. Check that components tree contains all necessary components.
				6.3. Set ItemCount = 2.
					6.3.1. Check that item info is updated.
			7. Check item compare info.
		")]
		[TestCase("ComparePage_TestCase1")]
		[TestCase("ComparePage_TestCase2")]
		public void ComparePageTest(string testDataJsonFileName)
		{
			try
			{
				var testDataDictionary = GetTestData(testDataJsonFileName);

				driver.FindElement(By.Id(testDataDictionary["apiVersionId"])).Click();

				Assert.That(driver.FindElements(By.ClassName("buildingImageName")), Has.Count.EqualTo(int.Parse(testDataDictionary["buildingsCount"])));

				//3
				CheckComponent(testDataDictionary, 1);

				//4
				driver.FindElement(By.Id("ShowComponentsWithTimeAndEnergyForAll")).Click();

				//5
				driver.FindElement(By.Id("ItemsCountOnView2")).Click();

				//6
				CheckComponent(testDataDictionary, 2);

				Assert.That(driver.FindElement(By.Id("ItemCompareInfo")).Text, Is.Not.Empty);
			}
			catch
			{
				MakeScreen($"{testDataJsonFileName}_Exception");
				throw;
			}
		}

		private void CheckComponent(Dictionary<string, string> testDataDictionary, int componentNumber)
		{
			var componentId = "Component" + componentNumber;
			var itemInfoId = "ItemInfo" + componentNumber;
			var totalTimeInfoId = "totalTimeInfo" + componentNumber;
			var itemCountId = "ItemCount" + componentNumber;
			var componentsTreeId = "componentsTree" + componentNumber;

			var component = driver.FindElement(By.Id(componentId));

			var building = component.FindElement(By.Id(testDataDictionary["buildingId"]));
			building.Click();

			component = driver.FindElement(By.Id(componentId));
			building = component.FindElement(By.Id(testDataDictionary["buildingId"]));
			Assert.That(building.GetAttribute("class"), Is.EqualTo(ClassesForSelectedBuilding));

			//3.1.2/6.1.2
			var item1 = component.FindElement(By.Id(testDataDictionary["item1Id"]));
			Assert.That(item1.GetAttribute("class"), Is.EqualTo("itemNameImage selectedImage"));

			if (componentNumber == 2)
			{
				//6.1.3
				var itemInfo = component.FindElement(By.Id(itemInfoId));
				Assert.That(itemInfo.Text, Does.Contain(testDataDictionary["item1Info"]));
			}

			//3.1.3/6.1.4
			var totalTimeInfo = driver.FindElement(By.Id(totalTimeInfoId));
			Assert.That(totalTimeInfo.Text, Is.EqualTo(testDataDictionary["totalTimeInfo1"]));

			//3.1.4/6.1.5
			var item2 = component.FindElement(By.Id(testDataDictionary["item2Id"]));
			Assert.That(item2.GetAttribute("class"), Is.EqualTo("itemNameImage"));

			//3.2/6.2
			item2.Click();

			//3.2.1/6.2.1
			component = driver.FindElement(By.Id(componentId));
			item2 = component.FindElement(By.Id(testDataDictionary["item2Id"]));
			Assert.That(item2.GetAttribute("class"), Is.EqualTo("itemNameImage selectedImage"));

			if (componentNumber == 2)
			{
				//6.2.2
				var itemInfo = component.FindElement(By.Id(itemInfoId));
				Assert.That(itemInfo.Text, Does.Contain(testDataDictionary["item2Info"]));

				//6.2.3
				component = driver.FindElement(By.Id(componentId));
				itemInfo = component.FindElement(By.Id(itemInfoId));
				Assert.That(itemInfo.Text, Does.Contain("Components"));
			}

			//3.2.2/6.2.4
			var componentsTree = driver.FindElement(By.Id(componentsTreeId));
			var componentsTreeImages = componentsTree.FindElements(By.TagName("img"));
			Assert.That(componentsTreeImages, Has.Count.EqualTo(int.Parse(testDataDictionary["expectedComponentsTreeImagesCount"])));

			if (componentNumber == 2)
			{
				//6.3
				var itemCount = component.FindElement(By.Id(itemCountId));
				new Actions(driver).MoveToElement(itemCount).Click().SendKeys(Keys.Backspace).SendKeys("2").SendKeys(Keys.Enter).Perform();

				//6.3.1
				component = driver.FindElement(By.Id(componentId));
				var itemInfo = component.FindElement(By.Id(itemInfoId));
				Assert.That(itemInfo.Text, Does.Contain(testDataDictionary["item1InfoFor2"]));
			}
		}
	}
}