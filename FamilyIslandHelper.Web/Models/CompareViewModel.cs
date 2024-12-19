using FamilyIslandHelper.Api;
using System.Collections.Generic;

namespace FamilyIslandHelper.Web.Models
{
	public class CompareViewModel
	{
		public ApiVersion ApiVersion { get; set; }

		public List<string> BuildingsNames { get; set; }
		public double Building1ProduceRatio { get; set; }
		public double Building2ProduceRatio { get; set; }
		public string BuildingName1 { get; set; }
		public string BuildingName2 { get; set; }
		public bool ShowListOfComponentsForAll { get; set; }

		public List<string> Items1 { get; set; }
		public List<string> Items2 { get; set; }
		public int ItemCount1 { get; set; }
		public string ItemName1 { get; set; }
		public string ItemInfo1 { get; set; }
		public int ItemCount2 { get; set; }
		public string ItemName2 { get; set; }
		public string ItemInfo2 { get; set; }

		public string ItemCompareInfo { get; set; }
	}
}
