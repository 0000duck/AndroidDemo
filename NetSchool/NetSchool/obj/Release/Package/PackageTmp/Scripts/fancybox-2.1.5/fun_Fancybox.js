function showDialog(w, h, url, onClosed) {
	var funCallBackClosed = onClosed || function(){
	};
    var stype = "iframe";
	var autoSize = "true";
    if (url.toString().indexOf('#') == 0) {
        stype = "inline";
    }
	
	if(arguments.length == 5 && typeof(arguments[4]) == "boolean"){
		autoSize = arguments[4];
	}

    $.fancybox({
        'width': w,
        'height': h,
		'autoSize': autoSize,
        'autoResize': true,
        'type': stype,
        'href': url,
        'showCloseButton': true,
        'afterClose': funCallBackClosed,
        helpers: { overlay : { closeClick :false} }
    });
}