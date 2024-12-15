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

			viewModel = new ViewModel
			{
				BuildingsNames = buildingsNames,
				BuildingProduceRatio = buildingHelper.CreateBuilding(buildingName1).ProduceRatio,
				BuildingName = buildingName1,
				ShowListOfComponents = false,
				Items = items1,
				ItemName = itemName1,
				ItemCount = 1,
				ItemInfo = GetInfoAboutItem(itemName1, 1, false),
				ApiVersion = apiVersion
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

			viewModel.ItemInfo = GetInfoAboutItem(viewModel.ItemName, viewModel.ItemCount, viewModel.ShowListOfComponents);
			viewModel.Items = items;
			viewModel.BuildingProduceRatio = buildingHelper.CreateBuilding(viewModel.BuildingName).ProduceRatio;

			return View(viewModel);
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