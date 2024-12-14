using System.Collections.Generic;

namespace FamilyIslandHelper.Web.Models
{
	public class ViewModel
	{
		public List<string> BuildingsNames { get; set; }
		public double BuildingProduceRatio { get; set; }
		public string BuildingName { get; set; }
		public bool ShowListOfComponents { get; set; }

		public List<string> Items { get; set; }
		public int ItemCount { get; set; }
		public string ItemName { get; set; }
		public string ItemInfo { get; set; }
	}
}
