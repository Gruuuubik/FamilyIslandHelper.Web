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

	let stepTimer = null;

	form.addEventListener('click', function (event) {
		const stepButton = event.target.closest('.stepper-btn');

		if (stepButton && form.contains(stepButton)) {
			const input = document.getElementById(stepButton.dataset.target);
			const min = Number(input.min) || 1;
			const max = Number(input.max) || 20;
			const current = parseInt(input.value, 10) || min;

			input.value = Math.min(max, Math.max(min, current + Number(stepButton.dataset.step)));

			clearTimeout(stepTimer);
			stepTimer = setTimeout(refresh, 150);
			return;
		}

		const treeItem = event.target.closest('.treeview div');

		if (treeItem && form.contains(treeItem)) {
			const nextUl = treeItem.nextElementSibling;

			if (nextUl && nextUl.tagName === 'UL') {
				nextUl.style.display = nextUl.style.display === 'none' ? 'block' : 'none';
			}
		}
	});

	form.addEventListener('keydown', function (event) {
		if (event.key === 'Enter' && event.target.matches('.itemCount')) {
			event.preventDefault();
			refresh();
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