using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Models.Abstract;
using System.Net;
using System.Text;

namespace FamilyIslandHelper.Web
{
	public static class ItemInfoService
	{
		public static string GetComponentsTree(ApiVersion apiVersion, Item item, int itemCount, bool showListOfComponents, string componentsTreeId)
		{
			var builder = new StringBuilder($@"<div class=""treeview"" id=""{componentsTreeId}"">");

			if (showListOfComponents)
			{
				AddItemComponentsToHtml(apiVersion, builder, item, itemCount);
			}

			builder.AppendLine("</div>");

			return builder.ToString();
		}

		public static void AddItemComponentsToHtml(ApiVersion apiVersion, StringBuilder builder, Item item, int multiplier)
		{
			if (item is ProducibleItem producibleItem)
			{
				builder.AppendLine("<ul>");

				foreach (var (childItem, count) in producibleItem.Components)
				{
					var totalCount = count * multiplier;
					var itemName = childItem.GetType().Name;
					string apiToCall;
					int unitEnergy;

					if (childItem is ProducibleItem producibleChildItem)
					{
						var buildingName = producibleChildItem.BuildingToCreate.GetType().Name;

						apiToCall = $"/resources/{apiVersion}/{buildingName}/{itemName}";
						unitEnergy = producibleChildItem.ProduceEnergyCost;
					}
					else
					{
						apiToCall = $"/resources/{apiVersion}/res/{itemName}";
						unitEnergy = (childItem as ResourceItem)?.EnergyCost ?? 0;
					}

					var displayName = WebUtility.HtmlEncode(childItem.Name);

					builder.AppendLine("	<li>");
					builder.AppendLine(@"<div class=""treeItem"">");
					builder.AppendLine(@$"	<img id=""{itemName}"" width=""40"" height=""40"" loading=""lazy"" src=""{apiToCall}"" alt=""{displayName}"" title=""{displayName}"">");
					builder.AppendLine($"	<span>{displayName}: {totalCount} шт. · {unitEnergy * totalCount} энергии</span>");
					builder.AppendLine("</div>");

					AddItemComponentsToHtml(apiVersion, builder, childItem, totalCount);

					builder.AppendLine("	</li>");
				}

				builder.AppendLine("</ul>");
			}
		}
	}
}
