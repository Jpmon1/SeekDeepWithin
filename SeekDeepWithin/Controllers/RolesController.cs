using System.Web.Mvc;
using System.Web.Security;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Controller used to manage roles.
   /// </summary>
   public class RolesController : Controller
   {
      /// <summary>
      /// Gets the index page for roles.
      /// </summary>
      /// <returns></returns>
      [Authorize (Roles = "Administrator")]
      public ActionResult Index ()
      {
         return View (Roles.GetAllRoles ());
      }

      /// <summary>
      /// Get the page to create.
      /// </summary>
      /// <returns>The create page.</returns>
      [Authorize (Roles = "Administrator")]
      public ActionResult Create ()
      {
         return View ();
      }

      /// <summary>
      /// Creates a role.
      /// </summary>
      /// <returns>The index page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Administrator")]
      public ActionResult Create (string roleName)
      {
         if (!Roles.RoleExists (roleName))
            Roles.CreateRole (roleName);
         return RedirectToAction ("Index");
      }

      /// <summary>
      /// Gets the view to assign a role.
      /// </summary>
      /// <returns>The assign role view.</returns>
      [Authorize (Roles = "Administrator")]
      public ActionResult Assign ()
      {
         var list = new SelectList (Roles.GetAllRoles ());
         ViewBag.Roles = list;
         return View ();
      }

      /// <summary>
      /// Assigns the given role to the given user.
      /// </summary>
      /// <param name="userName">The user to assign a role to.</param>
      /// <param name="roleName">Role to assign</param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Administrator")]
      public ActionResult Assign (string userName, string roleName)
      {
         if (Roles.IsUserInRole (userName, roleName))
         {
            ViewBag.ResultMessage = "This user already has the role.";
         }
         else
         {
            Roles.AddUserToRole (userName, roleName);
            ViewBag.ResultMessage = "User added to the role.";
         }

         var list = new SelectList (Roles.GetAllRoles ());
         ViewBag.Roles = list;
         return View ();
      }

      /// <summary>
      /// Deletes a role.
      /// </summary>
      /// <returns>The index page.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Administrator")]
      public ActionResult Delete (string roleName)
      {
         if (Roles.RoleExists (roleName))
            Roles.DeleteRole (roleName);
         return RedirectToAction ("Index");
      }
   }
}
