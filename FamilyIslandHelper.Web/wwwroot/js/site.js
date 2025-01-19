var BuildingName = document.getElementById("BuildingName");
var BuildingName1 = document.getElementById("BuildingName1");
var BuildingName2 = document.getElementById("BuildingName2");

var ItemCount = document.getElementById("ItemCount");
var ItemCount1 = document.getElementById("ItemCount1");
var ItemCount2 = document.getElementById("ItemCount2");

var ShowListOfComponents = document.getElementById("ShowListOfComponents");
var ShowListOfComponentsForAll = document.getElementById("ShowListOfComponentsForAll");

var showComponentsWithTimeAndEnergy = document.getElementById("ShowComponentsWithTimeAndEnergy");
var showComponentsWithTimeAndEnergyForAll = document.getElementById("ShowComponentsWithTimeAndEnergyForAll");

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

if (showComponentsWithTimeAndEnergy != null) {
	showComponentsWithTimeAndEnergy.addEventListener("change", function () {
		document.getElementById('mainForm').submit();
	});
}
if (showComponentsWithTimeAndEnergyForAll != null) {
	showComponentsWithTimeAndEnergyForAll.addEventListener("change", function () {
		document.getElementById('mainFormCompare').submit();
	});
}

var apiVersionHome1 = document.getElementById("ApiVersionHome1");
var apiVersionHome2 = document.getElementById("ApiVersionHome2");

if (apiVersionHome1 != null) {
	apiVersionHome1.addEventListener("click", function () {
		document.getElementById('mainForm').submit();
	});
}
if (apiVersionHome2 != null) {
	apiVersionHome2.addEventListener("click", function () {
		document.getElementById('mainForm').submit();
	});
}

var apiVersionCompare1 = document.getElementById("ApiVersionCompare1");
var apiVersionCompare2 = document.getElementById("ApiVersionCompare2");

if (apiVersionCompare1 != null) {
	apiVersionCompare1.addEventListener("click", function () {
		document.getElementById('mainFormCompare').submit();
	});
}
if (apiVersionCompare2 != null) {
	apiVersionCompare2.addEventListener("click", function () {
		document.getElementById('mainFormCompare').submit();
	});
}

document.querySelectorAll('.treeview div').forEach(treeItem => {
	treeItem.addEventListener('click', () => {
		const nextUl = treeItem.nextElementSibling;
		if (nextUl && nextUl.tagName === 'UL') {
			nextUl.style.display = nextUl.style.display === 'none' ? 'block' : 'none';
		}
	});
});

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
