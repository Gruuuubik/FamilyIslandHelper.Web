using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FamilyIslandHelper.Web.Controllers
{
	public class ResourcesController : Controller
	{
		private const string contentType = "image/png";

		[HttpGet("/resources/{apiVersion}/{buildingName}")]
		public IActionResult GetBuildingImage(string buildingName, ApiVersion apiVersion)
		{
			var buildingHelper = new BuildingHelper(apiVersion);
			var stream = buildingHelper.GetBuildingImageStreamByName(buildingName);

			if (stream == null)
			{
				return NotFound();
			}

			return File(stream, contentType);
		}

		[HttpGet("/resources/{apiVersion}/{buildingName}/{itemName}")]
		public IActionResult GetItemImage(string buildingName, string itemName, ApiVersion apiVersion)
		{
			var itemHelper = new ItemHelper(apiVersion);
			var stream = itemHelper.GetItemImageStreamByName(buildingName, itemName);

			if (stream == null)
			{
				return NotFound();
			}

			return File(stream, contentType);
		}

		[HttpGet("/resources/{apiVersion}/res/{resourceName}")]
		public IActionResult GetResourceImage(string resourceName, ApiVersion apiVersion)
		{
			var itemHelper = new ItemHelper(apiVersion);
			var stream = itemHelper.GetResourceImageStreamByName(resourceName);

			if (stream == null)
			{
				return NotFound();
			}

			return File(stream, contentType);
		}
	}
}
