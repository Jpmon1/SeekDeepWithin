var SdwEdit = {

   init: function () {
      $('#linkArea').hide();
      SdwEdit._initAC($('#txtNewLight'));
      SdwEdit._initAC($('#editLightSearch'));
      $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
      $('#addTruthCurrentRegex').change(function() {
         $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
      });
      $('#addTruthAppendText').autocomplete('clearCache');
      SdwEdit._getForLink();

      $('#btnAddToEdit').click(function (e) {
         e.preventDefault();
         var id = $('#editLightSearch').data('lightId');
         SdwEdit._addToHist(0, id);
      });
      $('#btnAddToLink').click(function (e) {
         e.preventDefault();
         var id = $('#editLightSearch').data('lightId');
         SdwEdit._addToHist(1, id);
      });
      $('#btnEdit').click(function (e) {
         e.preventDefault();
         $('#linkArea').hide();
         $('#editArea').show();
      });
      $('#btnLink').click(function (e) {
         e.preventDefault();
         $('#editArea').hide();
         $('#linkArea').show();
      });
      $('#btnCreateTruth').click(function(e) {
         e.preventDefault();
         SdwEdit.truthCreate();
      });
      $('#btnCreateLight').click(function(e) {
         e.preventDefault();
         SdwEdit.lightCreate();
      });
      $('#btnFormat').click(function (e) {
         e.preventDefault();
         SdwEdit.truthFormat();
      });
      $('#btnAddLove').click(function (e) {
         e.preventDefault();
         SdwEdit.loveAdd(0);
      });
      $('#btnAddLoveTruth').click(function (e) {
         e.preventDefault();
         SdwEdit.loveAdd(1);
      });
      $('#btnAddLight').click(function (e) {
         e.preventDefault();
         SdwEdit.lightAdd(0);
      });
      $('#btnAddLightTruth').click(function (e) {
         e.preventDefault();
         SdwEdit.lightAdd(1);
      });
      SdwEdit._histChange();
   },

   lightCreate: function () {
      $('#btnCreateLight').hide();
      SdwEdit._post('/Edit/Illuminate', { text: $('#txtNewLight').val() }, function () {
         $('#txtNewLight').val('');
         $('#btnCreateLight').show();
      });
   },

   lightGet: function (id, isLink) {
      SdwCommon.get('/Edit/GetLightItem', { id: parseInt(id), link: isLink }, function (html) {
         if (isLink == 1) {
            $('#linkLove').append(html);
            $('#savell' + id).click(function(e) {
               e.preventDefault();
               SdwEdit._post('/Edit/LightEdit', { id: id, text: $('#txtll' + id).val() });
            });
            $('#removell' + id).click(function(e) {
               e.preventDefault();
               SdwEdit._removeFromHist(1, id);
               //$('#ll' + id).remove();
               SdwEdit._getForLink();
            });
            SdwEdit._getForLink();
         } else {
            $('#editLove').append(html);
            $('#saveel' + id).click(function (e) {
               e.preventDefault();
               SdwEdit._post('/Edit/LightEdit', { id: id, text: $('#txtel' + id).val() });
            });
            $('#removeel' + id).click(function (e) {
               e.preventDefault();
               SdwEdit._removeFromHist(0, id);
               //$('#el' + id).remove();
               SdwEdit._getForEdit();
            });
            SdwEdit._getForEdit();
         }
         SdwCommon.loadStop();
      });
   },

   truthCreate: function() {
      var versions = [];
      var truthLinks = '';
      $('#btnCreateTruth').hide();
      var lights = SdwEdit._getEditLight();
      var hashId = new Hashids('GodisLove');
      $('#addVersionLinks').find('.switch-input').each(function (i, o) {
         var input = $(o);
         if (input.prop('checked')) {
            versions.push(parseInt(input.data('l')));
         }
      });
      $('#addTruthLinks').find('.switch-input').each(function (i, o) {
         var input = $(o);
         if (input.prop('checked')) {
            if (truthLinks != '') {
               truthLinks += ',';
            }
            truthLinks+= input.data('h');
         }
      });
      SdwEdit._post('/Edit/Love', {
         light: hashId.encode(lights),
         truth: $('#addTruthText').val(),
         versions: hashId.encode(versions),
         truthLinks: truthLinks
      }, function() {
         $('#addTruthText').val('');
         $('#addTruthFormatText').val('');
         $('#btnCreateTruth').show();
         SdwEdit._getForEdit();
      });
   },

   truthEdit: function (id) {
      var curr = $('#et' + id);
      if (curr.length > 0) { curr.remove(); }
      SdwCommon.get('/Edit/TruthEdit', { id: id }, function (html) {
         var added = $(html);
         var hfText = added.find('#txtHF').first();
         var lightText = added.find('#txtLight').first();
         added.find('a').each(function (i, l) {
            var link = $(l), text, sel;
            var lId = link.attr('id');
            link.click(function (e) {
               e.preventDefault();
               if (lId == 'addE') {
                  SdwEdit._addToHist(0, link.data('l'));
               } else if (lId == 'addL') {
                  SdwEdit._addToHist(1, link.data('l'));
               } else if (lId == 'saveL') {
                  SdwEdit._post('/Edit/LightEdit', {id: link.data('l'), text: lightText.val()});
               } else if (lId == 'del') {
                  SdwEdit.truthRemove(id);
               } else if (lId == 'hide') {
                  $('#et' + id).remove();
               } else if (lId == 'saveT') {
                  SdwEdit.truthSave(id);
               } else if (lId == 'index') {
                  sel = lightText.get_selection();
                  $('#sI' + id).val(sel.start);
                  $('#eI' + id).val(sel.end);
               } else if (lId == 'addH') {
                  text = $('#addTruthText').val();
                  if (text != '') { text += '\n'; }
                  text += '0|' + $('#number' +id).val() + '|' +hfText.val();
                  $('#addTruthText').val(text);
               } else if (lId == 'addF') {
                  sel = lightText.get_selection();
                  var fIndex = -sel.start;
                  text = $('#addTruthText').val();
                  if (text != '') { text += '\n'; }
                  text += fIndex + '|' +$('#number' +id).val() + '|' + hfText.val();
                  $('#addTruthText').val(text);
               } else if (lId == 'addS') {
                  SdwEdit.styleAdd(id);
               } else if (lId == 'delS') {
                  SdwEdit.styleRemove(link.data('s'));
               } else if (lId == 'link') {
                  var input = link.prev('input');
                  SdwEdit._post('/Edit/LightAddLight', { id: link.data('l'), truth: input.val() }, function () { input.val(''); });
               } else if (lId == 'btnB') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' +id).val(sel.end);
                  $('#sS' + id).val('<strong>');
                  $('#sE' + id).val('</strong>');
               } else if (lId == 'btnI') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' +id).val(sel.end);
                  $('#sS' + id).val('<em>');
                  $('#sE' + id).val('</em>');
               } else if (lId == 'btnL') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' +id).val(sel.end);
                  $('#sS' + id).val('<div class="text-left">');
                  $('#sE' + id).val('</div>');
               } else if (lId == 'btnC') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' +id).val(sel.end);
                  $('#sS' + id).val('<div class="text-center">');
                  $('#sE' + id).val('</div>');
               } else if (lId == 'btnR') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' +id).val(sel.end);
                  $('#sS' + id).val('<div class="text-right">');
                  $('#sE' + id).val('</div>');
               } else if (lId == 'btnD') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' +id).val(sel.end);
                  $('#sS' + id).val('<div>');
                  $('#sE' + id).val('</div>');
               } else if (lId == 'btnQ') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' +id).val(sel.end);
                  $('#sS' + id).val('<blockquote>');
                  $('#sE' + id).val('</blockquote>');
               } else if (lId == 'btnS') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' + id).val(sel.end);
                  $('#sS' + id).val('<div class="small-text">');
                  $('#sE' + id).val('</div>');
               } else if (lId == 'btnXS') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' + id).val(sel.end);
                  $('#sS' + id).val('<div class="smaller-text">');
                  $('#sE' + id).val('</div>');
               } else if (lId == 'btnOL') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' + id).val(sel.end);
                  $('#sS' + id).val('<ol>');
                  $('#sE' + id).val('</ol>');
               } else if (lId == 'btnUL') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' +id).val(sel.end);
                  $('#sS' + id).val('<ul>');
                  $('#sE' + id).val('</ul>');
               } else if(lId == 'btnLI') {
                  sel = lightText.get_selection();
                  $('#sI' + id).val(sel.start);
                  $('#eI' +id).val(sel.end);
                  $('#sS' + id).val('<li>');
                  $('#sE' + id).val('</li>');
               } else if (lId == 'btnDI') {
                  sel = lightText.get_selection();
                  $('#sI' +id).val(sel.start);
                  $('#eI' + id).val(sel.end);
                  $('#sS' + id).val('<div class="italic">');
                  $('#sE' + id).val('</div>');
               }
            });
         });
         SdwEdit._initAC(added.find('#txtLink').first());
         SdwEdit._initAC(added.find('#txtHeader').first());
         SdwEdit._initAC(added.find('#txtFooter').first());
         $('#ct' +id).after(added);
         SdwCommon.loadStop();
      });
   },

   truthRemove: function (id) {
      SdwEdit._post('/Edit/TruthRemove', { id: id }, function () {
         $('#et' + id).remove();
         $('#ct' + id).remove();
      });
   },

   truthSave: function (id) {
      SdwEdit._post('/Edit/TruthEdit', {
         id: id,
         order: $('#order' + id).val(),
         number: $('#number' + id).val()
      });
   },

   truthAddAsTruth: function (id) {
      var lights = SdwEdit._getEditLight();
      var hashId = new Hashids('GodisLove');
      SdwEdit._post('/Edit/TruthAddTruth', {id:id,light: hashId.encode(lights)});
   },

   loveAdd: function (toTruth) {
      var links = SdwEdit._getLinkLight();
      var lights = SdwEdit._getEditLight();
      var hashId = new Hashids('GodisLove');
      SdwEdit._post('/Edit/TruthAddLove', {
         link: hashId.encode(links),
         light: hashId.encode(lights),
         toTruth: toTruth
      });
   },

   lightAdd: function (toTruth) {
      var id = $('#linkLove').find('.columns').first().attr('id').substr(2);
      var lights = SdwEdit._getEditLight();
      var hashId = new Hashids('GodisLove');
      SdwEdit._post('/Edit/TruthAddLight', {
         id: id,
         light: hashId.encode(lights),
         toTruth: toTruth
      });
   },

   truthFormat: function () {
      var regex = $('#addTruthFormatRegex').val();
      SdwEdit._post('/Edit/Format', {
         regex: encodeURIComponent(regex),
         text: encodeURIComponent($('#addTruthFormatText').val()),
         startOrder: $('#addTruthFormatOrder').val(),
         startNumber: $('#addTruthFormatNumber').val()
      }, function (d) {
         if ($("#addTruthCurrentRegex option[value='" + d.regexId + "']").length <= 0) {
            $('#addTruthCurrentRegex').append($("<option></option>").attr("value", d.regexId).text(decodeURIComponent(d.regexText)));
         }
         $('#addTruthText').val(d.items);
      });
   },

   getIndexes: function () {
      var sel = $('#editLightText').get_selection();
      $('#editStyleStartIndex').val(sel.start);
      $('#editStyleEndIndex').val(sel.end);
   },

   styleAdd: function (id) {
      var end = $('#sE' + id).val();
      var start = $('#sS' + id).val();
      var endI = $('#eI' + id).val();
      var startI = $('#sI' + id).val();
      if ((end == undefined || end == '') ||
          (start == undefined || start == '') ||
          (endI == undefined || endI == '') ||
          (startI == undefined || startI == '')) {
         alert('Please add all style info.');
         return;
      }
      SdwEdit._post('/Edit/TruthAddStyle', {
         id: id,
         startIndex: startI,
         endIndex: endI,
         start: encodeURIComponent(start),
         end: encodeURIComponent(end)
      }, function () {
         SdwEdit.truthEdit(id);
      });
   },

   styleRemove: function (id) {
      SdwEdit._post('/Edit/TruthRemoveStyle', { id: id }, function () {
         $('#style' + id).remove();
      });
   },

   _getLinkLight: function () {
      var lights = [];
      $('#linkTruths').html('Loading...');
      $('#linkLove').find('.columns').each(function (i, o) {
         var light = $(o);
         lights.push(parseInt(light.attr('id').substr(2)));
      });
      return lights;
   },

   _getEditLight: function () {
      var lights = [];
      $('#editLove').find('.columns').each(function (i, o) {
         var light = $(o);
         lights.push(parseInt(light.attr('id').substr(2)));
      });
      return lights;
   },

   _getForEdit: function () {
      var lights = SdwEdit._getEditLight();
      var hashId = new Hashids('GodisLove');
      $('#truthArea').empty();
      $('#addTruthLinks').empty();
      if (lights.length > 0) {
         $('#truthArea').html('Loading...');
         $('#addTruthLinks').html('Loading...');
         var hash = hashId.encode(lights);
         SdwCommon.get('/Edit/TruthLinks', { lights: hash }, function(d) {
            $('#addTruthLinks').html(d);
            SdwCommon.loadStop();
         });
         SdwCommon.get('/Edit/Truths', { lights: hash, link: 0 }, function (d) {
            var added = $(d);
            added.each(function (i, row) {
               $(row).find('a').each(function(index, l) {
                  var truthId = $(row).attr('id').substr(2);
                  $(l).click(function () {
                     SdwEdit.truthEdit(truthId);
                  });
               });
            });
            $('#truthArea').html(added);
            SdwCommon.loadStop();
         });
      }
   },

   _getForLink: function () {
      var lights = SdwEdit._getLinkLight();
      var hashId = new Hashids('GodisLove');
      if (lights.length > 0) {
         $('#linkButtons').show();
         var hash = hashId.encode(lights);
         SdwCommon.get('/Edit/Truths', { lights: hash, link: 1 }, function (d) {
            var added = $(d);
            added.each(function (i, row) {
               $(row).find('a').each(function (index, l) {
                  var truthId = $(row).attr('id').substr(2);
                  $(l).click(function () { SdwEdit.truthAddAsTruth(truthId); });
               });
            });
            $('#linkTruths').html(added);
            SdwCommon.loadStop();
         });
      } else {
         $('#linkButtons').hide();
         $('#linkTruths').empty();
      }
   },

   _post: function (url, data, success) {
      SdwCommon.loadStart();
      setTimeout(function() {
         $.ajax({ type: 'POST', url: 'http://' + location.host + url, data: data }).done(function(d) {
            var ok = true;
            if (d.status) {
               if (d.status == 'fail') {
                  SdwCommon.loadStop(true);
                  console.log(url + ': ' + JSON.stringify(d));
                  alert(d.message);
                  ok = false;
               }
            }
            if (ok && success) {
               success(d);
            }
            SdwCommon.loadStop();
         }).fail(function (d) {
            SdwCommon.loadStop(true);
            console.log(url + ': ' + JSON.stringify(d));
         });
      }, 300);
   },

   _initAC:function(item) {
      item.autocomplete({
         serviceUrl: '/Seek/AutoComplete',
         paramName: 'text',
         noCache: true,
         deferRequestBy:500,
         triggerSelectOnValidInput: false,
         onSelect: function(suggestion) {
            item.val(suggestion.value);
            item.data('lightId', suggestion.data);
         }
      });
   },

   _histChange:function() {
      var currEdit = SdwEdit._getEditLight();
      var currLink = SdwEdit._getLinkLight();
      var editParam = SdwCommon.getParam('edit');
      var linkParam = SdwCommon.getParam('link');
      var hash = new Hashids('GodisLove');
      var urlEdit = hash.decode(editParam);
      var urlLink = hash.decode(linkParam);
      var editChanged = false;
      var linkChanged = false;
      SdwEdit._testLight(currEdit, urlEdit, function(id) {
          $('#el' +id).remove();
         editChanged = true;
      });
      SdwEdit._testLight(urlEdit, currEdit, function(id) {
          SdwEdit.lightGet(id, 0);
         editChanged = true;
      });
      SdwEdit._testLight(currLink, urlLink, function(id) {
          $('#ll' + id).remove();
         linkChanged = true;
      });
      SdwEdit._testLight(urlLink, currLink, function(id) {
          SdwEdit.lightGet(id, 1);
         linkChanged = true;
      });
      if (editChanged) {SdwEdit._getForEdit(); }
      if (linkChanged) {SdwEdit._getForLink(); }
   },

   _testLight:function(a, b, notFound) {
      for (var i = 0; i < a.length; i++) {
         var found = false;
         for (var j = 0; j < b.length; j++) {
            if (b[j] == a[i]) {
               found = true;
               break;
            }
         }
         if (!found) {
            notFound(a[i]);
         }
      }
   },

   _addToHist:function(type, id) {
      var hash = new Hashids('GodisLove');
      var editParam = SdwCommon.getParam('edit');
      var linkParam = SdwCommon.getParam('link');
      if (type == 0) {
         var urlEdit = hash.decode(editParam);
         urlEdit.push(id);
         editParam = hash.encode(urlEdit);
      } else {
         var urlLink = hash.decode(linkParam);
         urlLink.push(id);
         linkParam = hash.encode(urlLink);
      }
      History.pushState('Seek Deep Within', 'Seek Deep Within', '?edit=' + editParam + '&link=' + linkParam);
   },

   _removeFromHist:function(type, id) {
      var index;
      var hash = new Hashids('GodisLove');
      var editParam = SdwCommon.getParam('edit');
      var linkParam = SdwCommon.getParam('link');
      if (type == 0) {
         var urlEdit = hash.decode(editParam);
         index = urlEdit.indexOf(id);
         urlEdit.splice(index, 1);
         editParam = hash.encode(urlEdit);
      } else {
         var urlLink = hash.decode(linkParam);
         index = urlLink.indexOf(id);
         urlLink.splice(index, 1);
         linkParam = hash.encode(urlLink);
      }
      History.pushState('Seek Deep Within', 'Seek Deep Within', '?edit=' + editParam + '&link=' + linkParam);
   },

};
$(document).ready(function () {
   SdwCommon.loadStop();
   SdwEdit.init();
   History.Adapter.bind(window, 'statechange', SdwEdit._histChange);
});