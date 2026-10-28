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
		public IActionResult Index(CompareViewModel model)
		{
			// первый заход без параметров: значения по умолчанию
			if (!Request.Query.Any())
			{
				return View(CreateDefaultModel());
			}

			return View(BuildModel(model));
		}

		private CompareViewModel CreateDefaultModel()
		{
			var itemCount = 1;
			var showListOfComponents = true;

			var buildingName1 = buildingsNames.First();
			var items1 = buildingHelper.GetItemsOfBuilding(buildingName1);
			var itemName1 = items1.First();
			var item1 = itemHelper.FindItemByName(itemName1);

			var model = new CompareViewModel
			{
				BuildingsNames = buildingsNames,
				BuildingName1 = buildingName1,
				ShowListOfComponentsForAll = showListOfComponents,
				Items1 = items1,
				ItemName1 = itemName1,
				ItemCount1 = itemCount,
				ComponentsTreeHtml1 = ItemInfoService.GetComponentsTree(apiVersion, item1, itemCount, showListOfComponents, "componentsTree1"),
				ItemCount2 = itemCount,
				ApiVersion = apiVersion,
				ItemsCountOnView = 1,
				EnergyCost1 = (item1 as ProducibleItem)?.ProduceEnergyCost * itemCount
			};

			FillDisplayNames(model);

			return model;
		}

		private CompareViewModel BuildModel(CompareViewModel compareViewModel)
		{
			if (compareViewModel == null)
			{
				throw new ArgumentNullException(nameof(compareViewModel));
			}

			// если в ссылке нет версии API, используем версию по умолчанию
			if (!Request.Query.ContainsKey(nameof(CompareViewModel.ApiVersion)))
			{
				compareViewModel.ApiVersion = apiVersion;
			}

			if (compareViewModel.ApiVersion != apiVersion)
			{
				apiVersion = compareViewModel.ApiVersion;
				InitHelpers();
			}

			compareViewModel.ItemsCountOnView = compareViewModel.ItemsCountOnView == 2 ? 2 : 1;
			compareViewModel.ItemCount1 = Math.Clamp(compareViewModel.ItemCount1, 1, 20);
			compareViewModel.ItemCount2 = Math.Clamp(compareViewModel.ItemCount2, 1, 20);

			compareViewModel.BuildingsNames = buildingsNames;

			if (!buildingsNames.Contains(compareViewModel.BuildingName1))
			{
				compareViewModel.BuildingName1 = buildingsNames.First();
			}

			var items1 = buildingHelper.GetItemsOfBuilding(compareViewModel.BuildingName1);

			if (!items1.Contains(compareViewModel.ItemName1))
			{
				compareViewModel.ItemName1 = items1.FirstOrDefault();
			}

			var item1 = itemHelper.FindItemByName(compareViewModel.ItemName1);
			compareViewModel.EnergyCost1 = (item1 as ProducibleItem)?.ProduceEnergyCost * compareViewModel.ItemCount1;

			compareViewModel.Items1 = items1;
			compareViewModel.ComponentsTreeHtml1 = ItemInfoService.GetComponentsTree(apiVersion, item1, compareViewModel.ItemCount1, compareViewModel.ShowListOfComponentsForAll, "componentsTree1");

			if (compareViewModel.ItemsCountOnView == 2)
			{
				if (!buildingsNames.Contains(compareViewModel.BuildingName2))
				{
					compareViewModel.BuildingName2 = buildingsNames.First();
				}

				var items2 = buildingHelper.GetItemsOfBuilding(compareViewModel.BuildingName2);

				if (!items2.Contains(compareViewModel.ItemName2))
				{
					compareViewModel.ItemName2 = items2.FirstOrDefault();
				}

				var item2 = itemHelper.FindItemByName(compareViewModel.ItemName2);
				compareViewModel.EnergyCost2 = (item2 as ProducibleItem)?.ProduceEnergyCost * compareViewModel.ItemCount2;

				compareViewModel.Items2 = items2;
				compareViewModel.ComponentsTreeHtml2 = ItemInfoService.GetComponentsTree(apiVersion, item2, compareViewModel.ItemCount2, compareViewModel.ShowListOfComponentsForAll, "componentsTree2");

				compareViewModel.ItemCompareInfo = ItemHelper.CompareItems(item1, compareViewModel.ItemCount1, item2, compareViewModel.ItemCount2);
			}

			FillDisplayNames(compareViewModel);

			return compareViewModel;
		}

		private void FillDisplayNames(CompareViewModel model)
		{
			foreach (var b in buildingHelper.GetBuildingsClasses())
			{
				model.DisplayNames[b.Value] = b.Name;
			}

			var selectedBuildings = new List<string> { model.BuildingName1 };

			if (model.ItemsCountOnView == 2)
			{
				selectedBuildings.Add(model.BuildingName2);
			}

			foreach (var buildingName in selectedBuildings)
			{
				if (string.IsNullOrEmpty(buildingName)) continue;

				foreach (var item in buildingHelper.CreateBuilding(buildingName).Items)
				{
					model.DisplayNames[item.GetType().Name] = item.Name;
				}
			}
		}
	}
}