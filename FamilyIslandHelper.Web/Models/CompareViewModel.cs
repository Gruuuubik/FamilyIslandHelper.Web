using FamilyIslandHelper.Api;
using System.Collections.Generic;

namespace FamilyIslandHelper.Web.Models
{
	public class CompareViewModel
	{
		public ApiVersion ApiVersion { get; set; }

		public List<string> BuildingsNames { get; set; }
		public string BuildingName1 { get; set; }
		public string BuildingName2 { get; set; }

		public bool ShowListOfComponentsForAll { get; set; }
		public bool ShowComponentsWithEnergyForAll { get; set; }

		public List<string> Items1 { get; set; }
		public List<string> Items2 { get; set; }
		public int ItemCount1 { get; set; } = 1;
		public string ItemName1 { get; set; }
		public string ItemInfo1 { get; set; }
		public int ItemCount2 { get; set; } = 1;
		public string ItemName2 { get; set; }
		public string ItemInfo2 { get; set; }

		public string ComponentsTreeHtml1 { get; set; }
		public string ComponentsTreeHtml2 { get; set; }

		public string ItemCompareInfo { get; set; }

		public int ItemsCountOnView { get; set; }
	}
}
