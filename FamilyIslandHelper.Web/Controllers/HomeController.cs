using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Helpers;
using FamilyIslandHelper.Api.Models.Abstract;
using FamilyIslandHelper.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
				TotalTimeInfo = GetTotalTime(item1, itemCount),
				ItemInfo = GetInfoAboutItem(item1, itemCount, showListOfComponents),
				ApiVersion = apiVersion,
				ComponentsTreeHtml = GetComponentsTree(item1, showListOfComponents)
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

			viewModel.ItemInfo = GetInfoAboutItem(item, viewModel.ItemCount, viewModel.ShowListOfComponents);
			viewModel.TotalTimeInfo = GetTotalTime(item, viewModel.ItemCount);

			viewModel.Items = items;
			viewModel.BuildingProduceRatio = buildingHelper.CreateBuilding(viewModel.BuildingName).ProduceRatio;

			viewModel.ComponentsTreeHtml = GetComponentsTree(item, viewModel.ShowListOfComponents);

			return View(viewModel);
		}

		private static string GetTotalTime(Item item, int itemCount)
		{
			if (item is ProducibleItem producibleItem)
			{
				return TimeSpan.FromSeconds(producibleItem.TotalProduceTime.TotalSeconds * itemCount).ToString();
			}

			return string.Empty;
		}

		private static string GetInfoAboutItem(Item item, int itemCount, bool showListOfComponents)
		{
			var info = item.ToString(itemCount);

			if (item is ProducibleItem producibleItem)
			{
				if (showListOfComponents)
				{
					info += "\n";
					info += "Components:\n";

					var componentsInfo = producibleItem.ComponentsInfo(0, itemCount);

					info += string.Join("\n", componentsInfo);
				}
			}

			return info;
		}

		private string GetComponentsTree(Item item, bool showListOfComponents)
		{
			var builder = new StringBuilder(@"<div class=""treeview"" id=""componentsTree"">");

			if (showListOfComponents)
			{
				AddItemComponentsToHtml(builder, item);
			}

			builder.AppendLine("</div>");

			return builder.ToString();
		}

		private void AddItemComponentsToHtml(StringBuilder builder, Item item)
		{
			if (item is ProducibleItem producibleItem)
			{
				builder.AppendLine("<ul>");

				for (var i = 0; i < producibleItem.Components.Count; i++)
				{
					var childComponent = producibleItem.Components[i];
					var childItem = childComponent.item;

					builder.AppendLine("	<li>");

					var itemName = childItem.GetType().Name;
					string apiToCall;

					if (childItem is ProducibleItem producibleChildItem)
					{
						var buildingName = producibleChildItem.BuildingToCreate.GetType().Name;

						apiToCall = $"/resources/{apiVersion}/{buildingName}/{itemName}";
					}
					else
					{
						apiToCall = $"/resources/{apiVersion}/res/{itemName}";
					}

					builder.AppendLine(@"<div class=""treeItem"">");

					builder.AppendLine(@$"	<img id=""{itemName}"" width=""40"" src=""{apiToCall}"" alt=""{itemName}"" title=""{itemName}"">");

					builder.AppendLine($"	<span>{childItem.Name}({childComponent.count})</span>");

					builder.AppendLine("</div>");

					AddItemComponentsToHtml(builder, childItem);

					builder.AppendLine("	</li>");
				}

				builder.AppendLine("</ul>");
			}
		}
	}
}