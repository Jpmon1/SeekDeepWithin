$(document).ready(function () {
});

function createLight() {
   $.ajax({
      type: 'POST',
      url: "/Light/Create",
      data: {text:$('#textNewLight').val()}
   }).done(function (d) {
      // TODO: Add new light to list...
      $('#divNewLight').velocity('slideUp', { duration: 200 });
   }).fail(function (d) {
      alert(d.responseText);
   });
}

function showAddTruth() {
   $('#divNewTruth').velocity('slideDown', { duration: 300 });
   $('#btnAddTruth').velocity("transition.slideLeftOut", 100, false);
}

function hideAddTruth() {
   $('#divNewTruth').velocity('slideUp', { duration: 300 });
   $('#btnAddTruth').velocity("transition.slideLeftIn");
}

function newTruth() {
   $.ajax({
      url: "/Truth/New"
   }).done(function (d) {
      $('#formattedTruth').append(d);
   }).fail(function (d) {
      alert(d.responseText);
   });
}

function createTruth() {
   
}

function formatTruth() {
   $.ajax({
      type: 'POST',
      url: "/Truth/Format",
      data: { list: $('#textNewTruth').val() }
   }).done(function (d) {
      $('#formattedTruth').html(d);
   }).fail(function (d) {
      alert(d.responseText);
   });
}
