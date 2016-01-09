var Light = {

   _Radius: 180,

   add: function (id, text) {
      var item = $('#light_' + id);
      if (item.length <= 0) {
         var posX = 0, posY = 0;
         var lightWeb = $('#lightWeb');
         lightWeb.append('<div class="lightItem" id="light_' + id + '" onclick="Light.toggle(' + id + ');">' + text + '</div>');
         item = $('#light_' + id);
         var itemWidth = item.width();
         var itemHeight = item.height();
         var halfWidth = itemWidth / 2;
         var halfHeight = itemHeight / 2;
         var c = SdwCommon.getParam('c');
         var hashids = new Hashids("GodisLove");
         var clicked = hashids.decode(c);
         if (window.Lights.length <= 0) {
            var container = $('#lightWebContainer');
            var webWidth = container.width();
            var webHeight = container.height();
            posX = (webWidth / 2) - halfWidth;
            posY = (webHeight / 2) - halfHeight;
         } else {
            var iter = 1;
            var keepOn = true;
            var root = $('#light_' + clicked[0]);
            var offset = root.position();
            var centerX = offset.left + halfWidth;
            var centerY = offset.top + halfHeight;
            while (keepOn) {
               for (var a = 30; a < 360; a += 60) {
                  var angle = SdwCommon.toRadian(a);
                  posX = (centerX + (Light._Radius * iter) * Math.cos(angle)) - halfWidth;
                  posY = (centerY + (Light._Radius * iter) * Math.sin(angle)) - halfHeight;
                  var found = false;
                  for (var i = 0; i < window.Lights.length; i++) {
                     var light = window.Lights[i];
                     if (light.x == posX && light.y == posY) {
                        found = true;
                        break;
                     }
                  }
                  if (!found) {
                     keepOn = false;
                     break;
                  }
               }
               iter++;
            }
         }
         item.css({ 'top': posY, 'left': posX }).velocity("transition.bounceIn");
         window.Lights.push({ x: posX, y: posY, id: id });
      }
   },

   toggle: function (id) {
      $('#lightWeb').show();
      $('#lightList').hide();
      $('#lightWeb').append('<div class="row love" id="lvl_0"><div class="small-12 medium-6 medium-offset-3 large-4 large-offset-4 column">' +
         '<div class="callout" id="light_' + id + '" onclick="Love.load(' + id + ');">' + $('#l' + id).text() + '</div></div></div>');
      $('#light_' + id).velocity("transition.fadeIn");
      $('#lightList').empty();
      Love.load(id);
   },

   load: function () {
      $('#lightWeb').hide();
      $('#lightWeb').empty();
      $('#lightList').show();
      $('#lightList').empty();
      SdwCommon.get('/Light/Get', {}, function (d) {
         var count = d.count;
         var container = $('#lightList');
         for (var i = 0; i < count; i++) {
            var light = d.light[i];
            container.append('<div class="small-6 medium-4 large-3 column' + (i == (count - 1) ? ' end' : '') +
               '"><div class="callout" onclick="Light.toggle(' + light.id + ');" id="l' + light.id + '">' + light.text + '</a></div>');
         }
         SdwCommon.loadStop();
      });
   }

};
