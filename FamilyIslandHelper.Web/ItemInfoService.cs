using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Models.Abstract;
using System.Text;

namespace FamilyIslandHelper.Web
{
	public static class ItemInfoService
	{
		public static string GetInfoAboutItem(Item item, int itemCount, bool showListOfComponents)
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

		public static string GetComponentsTree(ApiVersion apiVersion, Item item, bool showListOfComponents, string componentsTreeId)
		{
			var builder = new StringBuilder($@"<div class=""treeview"" id=""{componentsTreeId}"">");

			if (showListOfComponents)
			{
				AddItemComponentsToHtml(apiVersion, builder, item);
			}

			builder.AppendLine("</div>");

			return builder.ToString();
		}

		public static void AddItemComponentsToHtml(ApiVersion apiVersion, StringBuilder builder, Item item)
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

					AddItemComponentsToHtml(apiVersion, builder, childItem);

					builder.AppendLine("	</li>");
				}

				builder.AppendLine("</ul>");
			}
		}
	}
}
