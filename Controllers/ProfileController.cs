using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Memcomb.Models;
using System.Data.Entity.Validation;
namespace Memcomb.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            User user = new User();
            using (memcombdbEntities dc = new memcombdbEntities())
            {
                if (HttpContext.Request.Cookies["userIDCookie"] != null)
                {
                    var cookie = HttpContext.Request.Cookies.Get("userIDCookie");
                    var v = dc.Users.Where(a => a.Email_ID == cookie.Value);
                    foreach (var u in v)
                    {
                        user.First_Name = u.First_Name;
                        user.Last_Name = u.Last_Name;
                        if (u.Profile_Picture != null)
                        {
                            var temp = u.Profile_Picture.Replace(@"C:\inetpub\wwwroot", "~"); //update for server info
                            user.Profile_Picture = temp;
                        }
                        else
                        {
                            user.Profile_Picture = @"~\Users\Default\Profile_Pic\rename.jpg"; //update for server info
                        }
                        if (u.Background_Pic != null)
                        {
                            var temp = u.Background_Pic.Replace(@"C:\inetpub\wwwroot", "~"); //update for server info
                            user.Background_Pic = temp;
                        }
                        else
                        {
                            user.Background_Pic = @"~\Users\Default\Background_Pic\default.jpg"; //update for server info
                        }
                        if (u.Biography != null)
                        {
                            user.Biography = u.Biography;
                        }
                        user.Memories = u.Memories;
                    }
                }
            }
            //ProfilePageModel profiler = new ProfilePageModel();
            return View(user);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilePicture(User model)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                using (memcombdbEntities dc = new memcombdbEntities())
                {
                    if (HttpContext.Request.Cookies["userIDCookie"] != null)
                    {
                        HttpCookie cookie = HttpContext.Request.Cookies.Get("userIDCookie");
                        var v = dc.Users.Where(a => a.Email_ID == cookie.Value).FirstOrDefault();
                        Directory.CreateDirectory(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Profile_Pic"));
                        HttpPostedFileBase file = model.Profile_Picture_imgPath;
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Profile_Pic"), fileName);
                            file.SaveAs(path);
                            /*model.User_ID = v.User_ID;
                            model.First_Name = v.First_Name;
                            model.Last_Name = v.Last_Name;
                            model.Email_ID = v.Email_ID;
                            model.Password = v.Password;*/
                            v.Profile_Picture = path;
                            //model.Background_Pic = v.Background_Pic;*/
                      
                        }
                        dc.Users.Include(v.Profile_Picture);
                        try
                        {
                            dc.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            // Retrieve the error messages as a list of strings.
                            var errorMessages = ex.EntityValidationErrors
                                    .SelectMany(x => x.ValidationErrors)
                                    .Select(x => x.ErrorMessage);

                            // Join the list to a single string.
                            var fullErrorMessage = string.Join("; ", errorMessages);

                            // Combine the original exception message with the new one.
                            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                            // Throw a new DbEntityValidationException with the improved exception message.
                            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                        }
                        Status = true;
                    }
                }
            }
            else
            {
                message = "Invalid request";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult BackgroundPicture(User model)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                using (memcombdbEntities dc = new memcombdbEntities())
                {
                    if (HttpContext.Request.Cookies["userIDCookie"] != null)
                    {
                        HttpCookie cookie = HttpContext.Request.Cookies.Get("userIDCookie");
                        var v = dc.Users.Where(a => a.Email_ID == cookie.Value).FirstOrDefault();
                        Directory.CreateDirectory(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Background_Pic"));
                        HttpPostedFileBase file = model.Background_Photo;
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Background_Pic"), fileName);
                            file.SaveAs(path);
                            v.Background_Pic = path;
                        }
                        dc.Users.Include(v.Background_Pic);
                        try
                        {
                            dc.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            // Retrieve the error messages as a list of strings.
                            var errorMessages = ex.EntityValidationErrors
                                    .SelectMany(x => x.ValidationErrors)
                                    .Select(x => x.ErrorMessage);

                            // Join the list to a single string.
                            var fullErrorMessage = string.Join("; ", errorMessages);

                            // Combine the original exception message with the new one.
                            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                            // Throw a new DbEntityValidationException with the improved exception message.
                            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                        }
                        Status = true;
                    }
                }
            }
            else
            {
                message = "Invalid request";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Biography(User model)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                using (memcombdbEntities dc = new memcombdbEntities())
                {
                    if (HttpContext.Request.Cookies["userIDCookie"] != null)
                    {
                        HttpCookie cookie = HttpContext.Request.Cookies.Get("userIDCookie");
                        var v = dc.Users.Where(a => a.Email_ID == cookie.Value).FirstOrDefault();
                        if (model.Biography.Length > 0 && model.Biography.Length <= 300)
                        {
                            v.Biography = model.Biography;
                        }
                        dc.Users.Include(v.Biography);
                        dc.SaveChanges();
                    }
                }
            }
            else
            {
                message = "Invalid request";
            }
           return RedirectToAction("Index");
        } 
        // GET: Profile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpPost]
        // GET: Profile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profile/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Profile/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Profile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Profile/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult _post_profile()
        {
            return PartialView("~/Views/Profile/_post_profile.cshtml");
        }
        public ActionResult _post_background()
        {
            return PartialView("~/Views/Profile/_post_background.cshtml");
        }
        public ActionResult _post_bio()
        {
            return PartialView("~/Views/Profile/_post_bio.cshtml");
        }
    }
}
