<script id="light-template" type="text/x-underscore-template">
   <div class="small-12 medium-<%= span %> large-<%= span %> column end">
      <div class="light" id="l<%= id %>">
         <div class="truth" id="t<%= tId %>">
            <%= text %>
         </div>
      </div>
   </div>
</script>

<script id="light-list-template" type="text/x-handlebars-template">
   {{#each []}}
      <div class="small-12 medium-{{span}} large-{{span}} column">
         <div class="light" id="l{{lId}}">
            <div class="truth" id="t{{tId}}">
               {{text}}
            </div>
         </div>
      </div>
   {{/each}}
</script>