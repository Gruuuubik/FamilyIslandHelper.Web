var BuildingName1 = document.getElementById("BuildingName1");
var BuildingName2 = document.getElementById("BuildingName2");

var ItemCount1 = document.getElementById("ItemCount1");
var ItemCount2 = document.getElementById("ItemCount2");

var ShowListOfComponentsForAll = document.getElementById("ShowListOfComponentsForAll");
var showComponentsWithEnergyForAll = document.getElementById("ShowComponentsWithEnergyForAll");

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

if (ShowListOfComponentsForAll != null) {
	ShowListOfComponentsForAll.addEventListener("change", function () {
		document.getElementById('mainFormCompare').submit();
	});
}
if (showComponentsWithEnergyForAll != null) {
	showComponentsWithEnergyForAll.addEventListener("change", function () {
		document.getElementById('mainFormCompare').submit();
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

var itemsCountOnView1 = document.getElementById("ItemsCountOnView1");
var itemsCountOnView2 = document.getElementById("ItemsCountOnView2");

if (itemsCountOnView1 != null) {
	itemsCountOnView1.addEventListener("click", function () {
		document.getElementById('mainFormCompare').submit();
	});
}
if (itemsCountOnView2 != null) {
	itemsCountOnView2.addEventListener("click", function () {
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
