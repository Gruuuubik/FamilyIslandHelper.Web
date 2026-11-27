using FamilyIslandHelper.Api;
using FamilyIslandHelper.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace FamilyIslandHelper.Web.Controllers
{
	public class ResourcesController : Controller
	{
		private const string contentType = "image/png";
		private const int cacheDurationSeconds = 86400; // сутки

		private static readonly Dictionary<ApiVersion, BuildingHelper> buildingHelperDictionary = new()
		{
			[ApiVersion.v2] = new BuildingHelper(ApiVersion.v2)
		};
		private static readonly Dictionary<ApiVersion, ItemHelper> itemHelperDictionary = new()
		{
			[ApiVersion.v2] = new ItemHelper(ApiVersion.v2)
		};

		[HttpGet("/resources/{apiVersion}/{buildingName}")]
		[ResponseCache(Duration = cacheDurationSeconds, Location = ResponseCacheLocation.Any)]
		public IActionResult GetBuildingImage(string buildingName, ApiVersion apiVersion)
		{
			return GetImage(() => buildingHelperDictionary[apiVersion].GetBuildingImageStreamByName(buildingName));
		}

		[HttpGet("/resources/{apiVersion}/{buildingName}/{itemName}")]
		[ResponseCache(Duration = cacheDurationSeconds, Location = ResponseCacheLocation.Any)]
		public IActionResult GetItemImage(string buildingName, string itemName, ApiVersion apiVersion)
		{
			return GetImage(() => itemHelperDictionary[apiVersion].GetItemImageStreamByName(buildingName, itemName));
		}

		[HttpGet("/resources/{apiVersion}/res/{resourceName}")]
		[ResponseCache(Duration = cacheDurationSeconds, Location = ResponseCacheLocation.Any)]
		public IActionResult GetResourceImage(string resourceName, ApiVersion apiVersion)
		{
			return GetImage(() => itemHelperDictionary[apiVersion].GetResourceImageStreamByName(resourceName));
		}

		private IActionResult GetImage(Func<MemoryStream> getImageStream)
		{
			try
			{
				return File(getImageStream(), contentType);
			}
			catch (FileNotFoundException)
			{
				// the helpers throw it when there is no such picture
				return NotFound();
			}
		}
	}
}