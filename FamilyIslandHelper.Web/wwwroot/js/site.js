const form = document.getElementById('mainFormCompare');

if (form) {
	let currentRequest = null;

	async function refresh() {
		const params = new URLSearchParams(new FormData(form));
		const url = form.action + '?' + params.toString();

		if (currentRequest) {
			currentRequest.abort();
		}

		const request = new AbortController();
		currentRequest = request;
		form.classList.add('loading');

		try {
			const response = await fetch(url, { signal: request.signal });

			if (!response.ok) {
				throw new Error('HTTP ' + response.status);
			}

			const html = await response.text();
			const doc = new DOMParser().parseFromString(html, 'text/html');
			const newForm = doc.getElementById('mainFormCompare');

			if (!newForm) {
				throw new Error('Form not found in response');
			}

			form.innerHTML = newForm.innerHTML;
			history.pushState(null, '', url);
		} catch (error) {
			if (error.name === 'AbortError') {
				return;
			}

			// запасной вариант: обычная отправка формы
			form.submit();
		} finally {
			if (currentRequest === request) {
				currentRequest = null;
				form.classList.remove('loading');
			}
		}
	}

	form.addEventListener('submit', function (event) {
		event.preventDefault();
		refresh();
	});

	form.addEventListener('change', function (event) {
		if (event.target.matches('input[type="radio"], #ItemCount1, #ItemCount2, #ShowListOfComponentsForAll, #ShowComponentsWithEnergyForAll')) {
			refresh();
		}
	});

	form.addEventListener('click', function (event) {
		const treeItem = event.target.closest('.treeview div');

		if (treeItem && form.contains(treeItem)) {
			const nextUl = treeItem.nextElementSibling;

			if (nextUl && nextUl.tagName === 'UL') {
				nextUl.style.display = nextUl.style.display === 'none' ? 'block' : 'none';
			}
		}
	});

	window.addEventListener('popstate', function () {
		location.reload();
	});

	window.refreshCompareForm = refresh;
}

function setItemName(event, itemNameId) {
	document.getElementById(itemNameId).value = event.currentTarget.id;
	window.refreshCompareForm();
}

function setBuildingName(event, buildingNameId) {
	document.getElementById(buildingNameId).value = event.currentTarget.id;
	window.refreshCompareForm();
}