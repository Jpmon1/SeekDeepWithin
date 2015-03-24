function tag_remove(tagId, itemId, type) {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/' + type + '/RemoveTag/',
      data: {
         tagId: tagId,
         Id: itemId,
         __RequestVerificationToken: token,
      }
   }).done(function () {
      $('#tag_' + tagId).remove();
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function tag_add(itemId, type) {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/' + type + '/AddTag/',
      data: {
         tagId: $('#tagId').val(),
         Id: itemId,
         __RequestVerificationToken: token,
      }
   }).done(function () {
      location.reload();
   }).fail(function (data) {
      alert(data.responseText);
   });
}