var panel_left = $('#panel_left'),
   panel_right = $('#panel_right'),
   panel_top = $('#panel_top'),
   panel_bottom = $('#panel_bottom'),
   panel_overlay = $('.panels_overlay');
var media_queries = {
   'small': 'only screen',
   'medium': 'only screen and (min-width:40.0625em)',
   'large': 'only screen and (min-width:64.0625em)',
   'xlarge': 'only screen and (min-width:90.0625em)',
   'xxlarge': 'only screen and (min-width:120.0625em)'
}

$(document).ready(function() {
   var offset = 300;
   panel_overlay.click(function () {
      $('.cd-nav-trigger').toggleClass('menu-is-open');
      panels_hideAll();
   });
   var navigationContainer = $('#cd-nav');
   //hide or show the "menu" link
   checkMenu();
   $(window).scroll(function () {
      checkMenu();
   });

   //open or close the menu clicking on the bottom "menu" link
   $('.cd-nav-trigger').on('click', function () {
      var nav = $(this);
      nav.toggleClass('menu-is-open');
      if (nav.hasClass('menu-is-open')) {
         panels_showLeft();
      } else {
         panels_hideAll();
      }
   });

   function checkMenu() {
      if (is_small_only()) {
         if ($(window).scrollTop() > offset && !navigationContainer.hasClass('is-fixed')) {
            navigationContainer.addClass('is-fixed').find('.cd-nav-trigger').one('webkitAnimationEnd oanimationend msAnimationEnd animationend', function() {
            });
         } else if ($(window).scrollTop() <= offset) {
            //check if the menu is open when scrolling up
            if (!$('html').hasClass('no-csstransitions')) {
               navigationContainer.removeClass('is-fixed');
               $('.cd-nav-trigger').removeClass('menu-is-open');
               //check if the menu is open when scrolling up - fallback if transitions are not supported
            } else if ($('html').hasClass('no-csstransitions')) {
               navigationContainer.removeClass('is-fixed');
               $('.cd-nav-trigger').removeClass('menu-is-open');
               //scrolling up with menu closed
            } else {
               navigationContainer.removeClass('is-fixed');
            }
         }
      }
   }
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

function match(mq) {
   return window.matchMedia(mq).matches;
}
function is_small_up() {
   return this.match(media_queries['small']);
}
function is_medium_up() {
   return this.match(media_queries['medium']);
}
function is_large_up() {
   return this.match(media_queries['large']);
}
function is_xlarge_up() {
   return this.match(media_queries['xlarge']);
}
function is_xxlarge_up() {
   return this.match(media_queries['xxlarge']);
}
function is_small_only() {
   return !this.is_medium_up() && !this.is_large_up() && !this.is_xlarge_up() && !this.is_xxlarge_up();
}
function is_medium_only() {
   return this.is_medium_up() && !this.is_large_up() && !this.is_xlarge_up() && !this.is_xxlarge_up();
}
function is_large_only() {
   return this.is_medium_up() && this.is_large_up() && !this.is_xlarge_up() && !this.is_xxlarge_up();
}
function is_xlarge_only() {
   return this.is_medium_up() && this.is_large_up() && this.is_xlarge_up() && !this.is_xxlarge_up();
}
function is_xxlarge_only() {
   return this.is_medium_up() && this.is_large_up() && this.is_xlarge_up() && this.is_xxlarge_up();
}
