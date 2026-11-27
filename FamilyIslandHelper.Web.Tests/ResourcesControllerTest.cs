using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Helpers;
using FamilyIslandHelper.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FamilyIslandHelper.Web.Tests
{
	/// <summary>
	/// Tests of the pictures that are used by the pages. They do not need a browser.
	/// </summary>
	[TestFixture]
	public class ResourcesControllerTest
	{
		private const ApiVersion Version = ApiVersion.v2;

		private readonly ResourcesController controller = new ResourcesController();
		private readonly BuildingHelper buildingHelper = new BuildingHelper(Version);
		private readonly ItemHelper itemHelper = new ItemHelper(Version);

		[Test]
		public void AllBuildingsPicturesAreAvailable()
		{
			var notFound = buildingHelper.GetBuildingsNames()
				.Where(b => controller.GetBuildingImage(b, Version) is not FileStreamResult)
				.ToList();

			Assert.That(notFound, Is.Empty, $"Pictures of buildings were not found: {string.Join(", ", notFound)}");
		}

		[Test]
		public void AllResourcesPicturesAreAvailable()
		{
			var notFound = itemHelper.GetResourcesNames()
				.Where(r => controller.GetResourceImage(r, Version) is not FileStreamResult)
				.ToList();

			Assert.That(notFound, Is.Empty, $"Pictures of resources were not found: {string.Join(", ", notFound)}");
		}

		[Test]
		public void AllPicturesOfComponentsTreesAreAvailable()
		{
			var notFound = new List<string>();
			var checkedPictures = 0;

			foreach (var buildingName in buildingHelper.GetBuildingsNames())
			{
				foreach (var itemName in buildingHelper.GetItemsOfBuilding(buildingName))
				{
					var item = itemHelper.FindItemByName(itemName);
					var html = ItemInfoService.GetComponentsTree(Version, item, 1, true, "componentsTree");

					foreach (Match match in Regex.Matches(html, "src=\"(/resources/[^\"]+)\""))
					{
						checkedPictures++;

						if (!IsAvailable(match.Groups[1].Value))
						{
							notFound.Add($"{itemName}: {match.Groups[1].Value}");
						}
					}
				}
			}

			Assert.That(checkedPictures, Is.GreaterThan(0));
			Assert.That(notFound, Is.Empty, $"Pictures of components were not found:{Environment.NewLine}{string.Join(Environment.NewLine, notFound)}");
		}

		[TestCase("/resources/v2/res/NotExistedResource")]
		[TestCase("/resources/v2/NotExistedBuilding")]
		[TestCase("/resources/v2/Bench/NotExistedItem")]
		public void NotExistedPictureIsNotFound(string url)
		{
			Assert.That(Resolve(url), Is.InstanceOf<NotFoundResult>());
		}

		private bool IsAvailable(string url)
		{
			return Resolve(url) is FileStreamResult;
		}

		private IActionResult Resolve(string url)
		{
			// /resources/{apiVersion}/res/{resourceName}  or  /resources/{apiVersion}/{buildingName}[/{itemName}]
			var parts = url.Split('/', StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length == 4 && parts[2] == "res")
			{
				return controller.GetResourceImage(parts[3], Version);
			}

			return parts.Length == 4
				? controller.GetItemImage(parts[2], parts[3], Version)
				: controller.GetBuildingImage(parts[2], Version);
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			controller.Dispose();
		}
	}
}
