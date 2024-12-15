using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Helpers;
using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyIslandHelper.Web.Controllers
{
	public class CompareController : Controller
	{
		private CompareViewModel compareViewModel;

		private ApiVersion apiVersion = ApiVersion.v2;
		private BuildingHelper buildingHelper;
		private ItemHelper itemHelper;
		private List<string> buildingsNames;

		public CompareController()
		{
			InitHelpers();
		}

		private void InitHelpers()
		{
			buildingHelper = new BuildingHelper(apiVersion);
			itemHelper = new ItemHelper(apiVersion);
			buildingsNames = buildingHelper.GetBuildingsNames();
		}

		[HttpGet]
		public IActionResult Index()
		{
			var buildingName1 = buildingsNames.First();
			var buildingName2 = buildingsNames.First();

			var items1 = buildingHelper.GetItemsOfBuilding(buildingName1);
			var items2 = buildingHelper.GetItemsOfBuilding(buildingName2);

			var itemName1 = items1.First();
			var itemName2 = items2.First();

			compareViewModel = new CompareViewModel
			{
				BuildingsNames = buildingsNames,
				Building1ProduceRatio = buildingHelper.CreateBuilding(buildingName1).ProduceRatio,
				Building2ProduceRatio = buildingHelper.CreateBuilding(buildingName2).ProduceRatio,
				BuildingName1 = buildingName1,
				BuildingName2 = buildingName2,
				ShowListOfComponentsForAll = false,
				Items1 = items1,
				ItemName1 = itemName1,
				ItemCount1 = 1,
				ItemInfo1 = GetInfoAboutItem(itemName1, 1, false),
				Items2 = items2,
				ItemName2 = itemName2,
				ItemCount2 = 1,
				ItemInfo2 = GetInfoAboutItem(itemName2, 1, false),
				ApiVersion = apiVersion
			};

			return View(compareViewModel);
		}

		[HttpPost]
		public IActionResult Index(CompareViewModel compareViewModel)
		{
			if (compareViewModel == null)
			{
				throw new ArgumentNullException(nameof(compareViewModel));
			}

			if (compareViewModel.ApiVersion != apiVersion)
			{
				apiVersion = compareViewModel.ApiVersion;
				InitHelpers();
			}

			compareViewModel.BuildingsNames = buildingsNames;

			if (!buildingsNames.Contains(compareViewModel.BuildingName1))
			{
				compareViewModel.BuildingName1 = buildingsNames.First();
			}

			if (!buildingsNames.Contains(compareViewModel.BuildingName2))
			{
				compareViewModel.BuildingName2 = buildingsNames.First();
			}

			var items1 = buildingHelper.GetItemsOfBuilding(compareViewModel.BuildingName1);
			var items2 = buildingHelper.GetItemsOfBuilding(compareViewModel.BuildingName2);

			if (!items1.Contains(compareViewModel.ItemName1))
			{
				compareViewModel.ItemName1 = items1.FirstOrDefault();
			}

			if (!items2.Contains(compareViewModel.ItemName2))
			{
				compareViewModel.ItemName2 = items2.FirstOrDefault();
			}

			compareViewModel.ItemInfo1 = GetInfoAboutItem(compareViewModel.ItemName1, compareViewModel.ItemCount1, compareViewModel.ShowListOfComponentsForAll);
			compareViewModel.ItemInfo2 = GetInfoAboutItem(compareViewModel.ItemName2, compareViewModel.ItemCount2, compareViewModel.ShowListOfComponentsForAll);

			compareViewModel.ItemCompareInfo = ItemHelper.CompareItems(itemHelper.FindItemByName(compareViewModel.ItemName1), compareViewModel.ItemCount1, itemHelper.FindItemByName(compareViewModel.ItemName2), compareViewModel.ItemCount2);

			compareViewModel.Items1 = items1;
			compareViewModel.Items2 = items2;

			compareViewModel.Building1ProduceRatio = buildingHelper.CreateBuilding(compareViewModel.BuildingName1).ProduceRatio;
			compareViewModel.Building2ProduceRatio = buildingHelper.CreateBuilding(compareViewModel.BuildingName2).ProduceRatio;

			return View(compareViewModel);
		}

		private string GetInfoAboutItem(string itemName, int itemCount, bool showListOfComponents)
		{
			if (itemName == null)
			{
				throw new ArgumentNullException(nameof(itemName));
			}

			var item = itemHelper.FindItemByName(itemName);

			var info = item.ToString(itemCount);
			if (item is ProducibleItem producibleItem)
			{
				if (showListOfComponents)
				{
					info += "\n";
					info += "Components:\n";
					foreach (var componentInfo in producibleItem.ComponentsInfo(0, itemCount))
					{
						info += componentInfo + "\n";
					}
				}
				info += "\n";
				info += "Итого времени на производство: " + TimeSpan.FromSeconds(producibleItem.TotalProduceTime.TotalSeconds * itemCount) + "\n";
			}
			return info;
		}
	}
}