using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FamilyIslandHelper.Web.Controllers
{
	public class ResourcesController : Controller
	{
		private const string contentType = "image/png";
		private static readonly Dictionary<ApiVersion, BuildingHelper> buildingHelperDictionary = new()
		{
			[ApiVersion.v1] = new BuildingHelper(ApiVersion.v1),
			[ApiVersion.v2] = new BuildingHelper(ApiVersion.v2)
		};
		private static readonly Dictionary<ApiVersion, ItemHelper> itemHelperDictionary = new()
		{
			[ApiVersion.v1] = new ItemHelper(ApiVersion.v1),
			[ApiVersion.v2] = new ItemHelper(ApiVersion.v2)
		};

		[HttpGet("/resources/{apiVersion}/{buildingName}")]
		public IActionResult GetBuildingImage(string buildingName, ApiVersion apiVersion)
		{
			var stream = buildingHelperDictionary[apiVersion].GetBuildingImageStreamByName(buildingName);

			if (stream == null)
			{
				return NotFound();
			}

			return File(stream, contentType);
		}

		[HttpGet("/resources/{apiVersion}/{buildingName}/{itemName}")]
		public IActionResult GetItemImage(string buildingName, string itemName, ApiVersion apiVersion)
		{
			var stream = itemHelperDictionary[apiVersion].GetItemImageStreamByName(buildingName, itemName);

			if (stream == null)
			{
				return NotFound();
			}

			return File(stream, contentType);
		}

		[HttpGet("/resources/{apiVersion}/res/{resourceName}")]
		public IActionResult GetResourceImage(string resourceName, ApiVersion apiVersion)
		{
			var stream = itemHelperDictionary[apiVersion].GetResourceImageStreamByName(resourceName);

			if (stream == null)
			{
				return NotFound();
			}

			return File(stream, contentType);
		}
	}
}
