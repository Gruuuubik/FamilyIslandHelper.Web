using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Helpers;
using FamilyIslandHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyIslandHelper.Web.Controllers
{
	public class HomeController : Controller
	{
		private ViewModel viewModel;

		private ApiVersion apiVersion = ApiVersion.v2;
		private BuildingHelper buildingHelper;
		private ItemHelper itemHelper;
		private List<string> buildingsNames;

		public HomeController()
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
			var items1 = buildingHelper.GetItemsOfBuilding(buildingName1);
			var itemName1 = items1.First();

			var itemCount = 1;
			var showListOfComponents = false;
			var item1 = itemHelper.FindItemByName(itemName1);

			viewModel = new ViewModel
			{
				BuildingsNames = buildingsNames,
				BuildingProduceRatio = buildingHelper.CreateBuilding(buildingName1).ProduceRatio,
				BuildingName = buildingName1,
				ShowListOfComponents = showListOfComponents,
				Items = items1,
				ItemName = itemName1,
				ItemCount = itemCount,
				TotalTimeInfo = ItemInfoService.GetTotalTime(item1, itemCount),
				ItemInfo = ItemInfoService.GetInfoAboutItem(item1, itemCount, showListOfComponents),
				ApiVersion = apiVersion,
				ComponentsTreeHtml = ItemInfoService.GetComponentsTree(apiVersion,item1, showListOfComponents, "componentsTree")
			};

			return View(viewModel);
		}

		[HttpPost]
		public IActionResult Index(ViewModel viewModel)
		{
			if (viewModel == null)
			{
				throw new ArgumentNullException(nameof(viewModel));
			}

			if (viewModel.ApiVersion != apiVersion)
			{
				apiVersion = viewModel.ApiVersion;
				InitHelpers();
			}

			viewModel.BuildingsNames = buildingsNames;

			if (!buildingsNames.Contains(viewModel.BuildingName))
			{
				viewModel.BuildingName = buildingsNames.First();
			}

			var items = buildingHelper.GetItemsOfBuilding(viewModel.BuildingName);

			if (!items.Contains(viewModel.ItemName))
			{
				viewModel.ItemName = items.FirstOrDefault();
			}

			var item = itemHelper.FindItemByName(viewModel.ItemName);

			viewModel.ItemInfo = ItemInfoService.GetInfoAboutItem(item, viewModel.ItemCount, viewModel.ShowListOfComponents);
			viewModel.TotalTimeInfo = ItemInfoService.GetTotalTime(item, viewModel.ItemCount);

			viewModel.Items = items;
			viewModel.BuildingProduceRatio = buildingHelper.CreateBuilding(viewModel.BuildingName).ProduceRatio;

			viewModel.ComponentsTreeHtml = ItemInfoService.GetComponentsTree(apiVersion, item, viewModel.ShowListOfComponents, "componentsTree");

			return View(viewModel);
		}
	}
}