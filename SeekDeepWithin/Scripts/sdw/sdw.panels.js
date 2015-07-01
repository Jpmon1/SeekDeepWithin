var panel_left = $('#panel_left'),
   panel_right = $('#panel_right'),
   panel_top = $('#panel_top'),
   panel_bottom = $('#panel_bottom'),
   panel_overlay = $('.panels_overlay'),
   body = $('body');

$(document).ready(function() {
   panel_overlay.click(panels_hideAll);
});

function panels_hideAll() {
   panels_hideTop();
   panels_hideLeft();
   panels_hideRight();
   panels_hideBottom();
   panels_hideOverlay();
}

function panels_showLeft() {
   panels_showOverlay();
   panel_overlay.css({ 'left': '240px' });
   panel_left.addClass('panels-open');
}
function panels_hideLeft() {
   panel_left.removeClass('panels-open');
}

function panels_showRight() {
   panels_showOverlay();
   panel_overlay.css({ 'left': '-240px' });
   panel_right.addClass('panels-open');
}
function panels_hideRight() {
   panel_right.removeClass('panels-open');
}

function panels_showTop() {
   panel_top.addClass('panels-open');
}
function panels_hideTop() {
   panel_top.removeClass('panels-open');
}

function panels_showBottom() {
   panel_bottom.addClass('panels-open');
}
function panels_hideBottom() {
   panel_bottom.removeClass('panels-open');
}

function panels_showOverlay() {
   panel_overlay.addClass('panels-open');
}
function panels_hideOverlay() {
   panel_overlay.removeClass('panels-open');
   panel_overlay.css({ 'left': '0' });
   panel_overlay.css({ 'top': '0' });
}
