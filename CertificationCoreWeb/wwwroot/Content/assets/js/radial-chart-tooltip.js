"use strict";
var radial_tooltip = {
    init: function() {
	$('[data-toggle="tooltip"]').tooltip();
	    $('[data-toggle="popover"]').popover({
	        animation: true,
	        delay: {
	            show: 100,
	            hide: 100
	        }
	    });
	}
};
jQuery(document).ready(function() {
    radial_tooltip.init()
});