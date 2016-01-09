var Seek = {

   load: function () {
      var page = $('#pageId').val();
      if (page == 'index') {
         var l = SdwCommon.getParam('l');
         if (l == null || l == '') {
            Light.load();
         } else {
            Love.load(l);
         }
      }
   }

};

$(document).ready(function() {
   //History.Adapter.bind(window, 'statechange', Seek.load);
   Seek.load();
});

/*jsPlumb.ready(function () {

   var color = "gray";
   var instance = jsPlumb.getInstance({
      // notice the 'curviness' argument to this Bezier curve.  the curves on this page are far smoother
      // than the curves on the first demo, which use the default curviness value.
      Connector: ["Bezier", { curviness: 50 }],
      //DragOptions: { cursor: "pointer", zIndex: 2000 },
      PaintStyle: { strokeStyle: color, lineWidth: 4 },
      EndpointStyle: { radius: 9, fillStyle: color },
      HoverPaintStyle: { strokeStyle: "#0067a7" },
      EndpointHoverStyle: { fillStyle: "#0067a7" },
      Container: "lightWeb",
      ConnectionsDetachable: false
   });

   // suspend drawing and initialise.
   instance.batch(function () {
      // add endpoints, giving them a UUID.
      // you DO NOT NEED to use this method. You can use your library's selector method.
      // the jsPlumb demos use it so that the code can be shared between all three libraries.
      var windows = jsPlumb.getSelector(".chart-demo .window");
      for (var i = 0; i < windows.length; i++) {
         instance.addEndpoint(windows[i], {
            uuid: windows[i].getAttribute("id") + "-bottom",
            anchor: "Bottom",
            maxConnections: -1
         });
         instance.addEndpoint(windows[i], {
            uuid: windows[i].getAttribute("id") + "-top",
            anchor: "Top",
            maxConnections: -1
         });
      }

      instance.connect({ uuids: ["chartWindow3-bottom", "chartWindow6-top"] });
      instance.connect({ uuids: ["chartWindow1-bottom", "chartWindow2-top"] });
      instance.connect({ uuids: ["chartWindow1-bottom", "chartWindow3-top"] });
      instance.connect({ uuids: ["chartWindow2-bottom", "chartWindow4-top"] });
      instance.connect({ uuids: ["chartWindow2-bottom", "chartWindow5-top"] });

      //instance.draggable(windows, {grid:[50,50]});

   });

   jsPlumb.fire("jsPlumbDemoLoaded", instance);
});*/
