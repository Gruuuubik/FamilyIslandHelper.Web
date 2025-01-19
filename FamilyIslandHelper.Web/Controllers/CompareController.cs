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

			var itemCount = 1;
			var showListOfComponents = true;
			var showComponentsWithTimeAndEnergyForAll = false;
			var item1 = itemHelper.FindItemByName(itemName1);
			var item2 = itemHelper.FindItemByName(itemName2);

			compareViewModel = new CompareViewModel
			{
				BuildingsNames = buildingsNames,
				Building1ProduceRatio = buildingHelper.CreateBuilding(buildingName1).ProduceRatio,
				Building2ProduceRatio = buildingHelper.CreateBuilding(buildingName2).ProduceRatio,
				BuildingName1 = buildingName1,
				BuildingName2 = buildingName2,
				ShowListOfComponentsForAll = showListOfComponents,
				ShowComponentsWithTimeAndEnergyForAll = showComponentsWithTimeAndEnergyForAll,
				Items1 = items1,
				ItemName1 = itemName1,
				ItemCount1 = itemCount,
				TotalTimeInfo1 = ItemInfoService.GetTotalTime(item1, itemCount),
				ComponentsTreeHtml1 = ItemInfoService.GetComponentsTree(apiVersion, item1, showListOfComponents, "componentsTree1"),
				Items2 = items2,
				ItemName2 = itemName2,
				ItemCount2 = itemCount,
				TotalTimeInfo2 = ItemInfoService.GetTotalTime(item2, itemCount),
				ComponentsTreeHtml2 = ItemInfoService.GetComponentsTree(apiVersion, item2, showListOfComponents, "componentsTree2"),
				ApiVersion = apiVersion
			};

			if (showComponentsWithTimeAndEnergyForAll)
			{
				compareViewModel.ItemInfo1 = ItemInfoService.GetInfoAboutItem(item1, itemCount, showListOfComponents);
				compareViewModel.ItemInfo2 = ItemInfoService.GetInfoAboutItem(item2, itemCount, showListOfComponents);
			}

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

			var item1 = itemHelper.FindItemByName(compareViewModel.ItemName1);
			var item2 = itemHelper.FindItemByName(compareViewModel.ItemName2);

			if (compareViewModel.ShowComponentsWithTimeAndEnergyForAll)
			{
				compareViewModel.ItemInfo1 = ItemInfoService.GetInfoAboutItem(item1, compareViewModel.ItemCount1, compareViewModel.ShowListOfComponentsForAll);
				compareViewModel.ItemInfo2 = ItemInfoService.GetInfoAboutItem(item2, compareViewModel.ItemCount2, compareViewModel.ShowListOfComponentsForAll);
			}

			compareViewModel.TotalTimeInfo1 = ItemInfoService.GetTotalTime(item1, compareViewModel.ItemCount1);
			compareViewModel.TotalTimeInfo2 = ItemInfoService.GetTotalTime(item2, compareViewModel.ItemCount2);

			compareViewModel.ItemCompareInfo = ItemHelper.CompareItems(itemHelper.FindItemByName(compareViewModel.ItemName1), compareViewModel.ItemCount1, itemHelper.FindItemByName(compareViewModel.ItemName2), compareViewModel.ItemCount2);

			compareViewModel.Items1 = items1;
			compareViewModel.Items2 = items2;

			compareViewModel.Building1ProduceRatio = buildingHelper.CreateBuilding(compareViewModel.BuildingName1).ProduceRatio;
			compareViewModel.Building2ProduceRatio = buildingHelper.CreateBuilding(compareViewModel.BuildingName2).ProduceRatio;

			compareViewModel.ComponentsTreeHtml1 = ItemInfoService.GetComponentsTree(apiVersion, item1, compareViewModel.ShowListOfComponentsForAll, "componentsTree1");
			compareViewModel.ComponentsTreeHtml2 = ItemInfoService.GetComponentsTree(apiVersion, item2, compareViewModel.ShowListOfComponentsForAll, "componentsTree2");

			return View(compareViewModel);
		}
	}
}