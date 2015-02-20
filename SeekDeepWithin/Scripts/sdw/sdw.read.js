$(document).ready(function () {
   $('#contentTree').jstree({
      'core': {
         'animation': 0,
         "multiple": false
      },
      'types': {
         '#': {
            'max_depth': 4,
            'valid_children': ['version']
         },
         'version': {
            'icon': 'icon-book',
            'valid_children': ['subbook']
         },
         'subbook': {
            'icon': 'icon-bookalt',
            'valid_children': ['chapter']
         },
         'chapter': {
            'icon': 'icon-bookmark',
            'valid_children': []
         }
      },
      'plugins': ['types', 'wholerow']
   });
});