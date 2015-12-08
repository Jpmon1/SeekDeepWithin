$(document).ready(load);
History.Adapter.bind(window, 'statechange', load);

function load() {
   var btn = $('#btnAddTruth');
   var data = getURLParameter('data');
   if (data == null) {
      if (btn.length > 0) {
         btn.hide();
      }
      loadDefault();
   } else {
      if (btn.length > 0) {
         btn.show();
      }
      loadData(data);
   }
}

function loadDefault() {
   $('#lightList').empty();
   $.ajax({
      url: "/Light/Get"
   }).done(function (d) {
      if (d.status == "success") {
         var count = d.count;
         var container = $('#lightLinks');
         for (var i = 0; i < count; i++) {
            var light = d.light[i];
            container.append('<div class="col span_3_of_12"><a href="javascript:void(0);" onclick="addLight(\'' +
               light.id + '\');" class="sdw-button white expand">' + light.text + '</a></div>');
         }
      }
   }).fail(function (d) {
      alert(d.responseText);
   });
}

function loadData(data) {
   $('#lightLinks').empty();
   // TODO: Show loading....
   var lightIds = Base64.decode(data);
   var idArray = lightIds.split(",");
   // TODO: Get data from server...
   // TODO: Add light(s) to selected lights...
   var lightsToAdd = [];
   for (var i = 0; i < idArray.length; i++) {
      if (!$('light' + idArray[i]).length) {
         lightsToAdd.push(idArray[i]);
      }
   }
   addLightArray(lightsToAdd);
   // TODO: Display data...
   // TODO: Hide loading...
}

function addLightArray(lights) {
   if (lights.length > 0) {
      var id = lights.shift();
      $.ajax({
         url: "/Light/GetButton",
         data: {id: id, links:true, select:true, remove:true}
      }).done(function (d) {
         $('#lightList').append(d);
         $('#light' + id).velocity("transition.bounceIn");
         addLightArray(lights);
      }).fail(function (d) {
         alert(d.responseText);
      });
   }
}

function getLove(id) {
   
}

function addLight(id) {
   var dataDecoded = '';
   var data = getURLParameter('data');
   if (data != null) {
      dataDecoded = Base64.decode(data);
      dataDecoded += '|';
   }
   dataDecoded += id;
   History.pushState(null, null, "?data=" + Base64.encode(dataDecoded));
}

function getURLParameter(name) {
   return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null;
}
