function connectToSignalR() {
	console.log("Connecting to SignalR...");
	const conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
	conn.on("DisplayPriceNotification", DisplayPriceNotification);
	conn.start().then(function () {
		console.log("SignalR connected1");
	}).catch(function (err) {
		console.log(err);
	});
}

function DisplayPriceNotification(user, message) {
	console.log(user);
	console.log(message);
	const data = JSON.parse(message);

	var html = `<div>New vehicle! ${data.Make} ${data.Model}<br />
${data.Color}, ${data.Year}. Price ${data.Price} ${data.CurrencyCode}<br />
<a href="/vehicles/details/${data.Registration}">click here for more...<a/></div>`;
	var $html = $(html);
	$html.css("background-color", data.Color);
	var $target = $("#signalr-notifications");
	$target.prepend($html);
	window.setTimeout(function () {
		$html.fadeOut(2000, function () {
			$html.remove();
		});
	}, 2000);


}
