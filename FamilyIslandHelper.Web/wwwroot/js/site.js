var BuildingName = document.getElementById("BuildingName");
var BuildingName1 = document.getElementById("BuildingName1");
var BuildingName2 = document.getElementById("BuildingName2");

var ItemCount = document.getElementById("ItemCount");
var ItemCount1 = document.getElementById("ItemCount1");
var ItemCount2 = document.getElementById("ItemCount2");

var ShowListOfComponents = document.getElementById("ShowListOfComponents");
var ShowListOfComponentsForAll = document.getElementById("ShowListOfComponentsForAll");

if (BuildingName != null) {
	BuildingName.addEventListener("change", function () {
		document.getElementById('mainForm').submit();
	});
}
if (ItemCount != null) {
	ItemCount.addEventListener("change", function () {
		document.getElementById('mainForm').submit();
	});
}
if (ShowListOfComponents != null) {
	ShowListOfComponents.addEventListener("change", function () {
		document.getElementById('mainForm').submit();
	});
}

if (BuildingName1 != null) {
	BuildingName1.addEventListener("change", function () {
		document.getElementById('mainFormCompare').submit();
	});
}

if (BuildingName2 != null) {
	BuildingName2.addEventListener("change", function () {
		document.getElementById('mainFormCompare').submit();
	});
}

if (ItemCount1 != null) {
	ItemCount1.addEventListener("change", function () {
		document.getElementById('mainFormCompare').submit();
	});
}

if (ItemCount2 != null) {
	ItemCount2.addEventListener("change", function () {
		document.getElementById('mainFormCompare').submit();
	});
}

if (ShowListOfComponents != null) {
	ShowListOfComponents.addEventListener("change", function () {
		document.getElementById('mainForm').submit();
	});
}

if (ShowListOfComponentsForAll != null) {
	ShowListOfComponentsForAll.addEventListener("change", function () {
		document.getElementById('mainFormCompare').submit();
	});
}

function setItemName(event, itemNameId, formId) {
	document.getElementById(itemNameId).value = event.currentTarget.id;

	var form = document.getElementById(formId);
	form.submit();

	var itemNameImages = form.getElementsByClassName('itemNameImage');

	for (let i = 0; i < itemNameImages.length; i++) {
		itemNameImages[i].classList.remove("selectedImage");
	}

	event.currentTarget.classList.add("selectedImage");
}

function setBuildingName(event, buildingNameId, formId) {
	document.getElementById(buildingNameId).value = event.currentTarget.id;

	var form = document.getElementById(formId);
	form.submit();

	var buildingNameImages = form.getElementsByClassName('buildingImageName');

	for (let i = 0; i < buildingNameImages.length; i++) {
		buildingNameImages[i].classList.remove("selectedImage");
	}

	event.currentTarget.classList.add("selectedImage");
}
