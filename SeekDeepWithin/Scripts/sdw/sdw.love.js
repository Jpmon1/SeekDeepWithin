﻿var Love = {
   
   load: function (selected) {
      /*var lArray = [];
      var l = SdwCommon.getParam('l');
      var hashids = new Hashids("GodisLove");
      if (l != null) { lArray = hashids.decode(l); }
      if ($.inArray(selected, lArray) < 0) { lArray.push(selected); }
      var all = hashids.encode(lArray);
      var history = '?l=' + all;
      History.pushState(null, 'Seek Deep Within', history);*/

      var data = [];
      var last = $('#light_' + selected);
      last.toggleClass('selected');
      $('.love').each(function (i, o) {
         var level = [];
         $(o).find('.callout').each(function (cI, cO) {
            var light = $(cO);
            var id = parseInt(light.attr("id").substr(6));
            var lObj = { i: id, t: light.data('tid'), l: light.data('lid') };
            if (light.hasClass('selected')) {
               lObj.s = 1;
            }
            level.push(lObj);
         });
         data.push(level);
      });

      if ($('#editable').length > 0) { SdwEdit.truthEdit(last); }
      SdwCommon.get('/Love/Get', { lastId: selected, data: JSON.stringify(data) }, function (d) {
         var web = $('#lightWeb');
         var sandbox = $('#sandbox');
         sandbox.html(d);
         var children = sandbox.children();
         var count = children.length;
         for (var i = 0; i < count; i++) {
            var child = $(children[i]);
            var index = child.attr('id').substr(8);
            var current = $('#lvl_' + index);
            if (current.length > 0) {
               current.html(child.html());
               child.remove();
               current.velocity('callout.bounce');
            } else {
               child = child.detach();
               web.append(child);
               child.attr('id', 'lvl_' + index);
               child.velocity('transition.fadeIn');
            }
         }
         // TODO: Draw lines...
         SdwCommon.loadStop();
      });
   }

};
