using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Memcomb.Models;
namespace Memcomb.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ProfilePageModel model, string id)
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
                        if (id == "profileImage")
                        {
                            string ProfileIDPath = dc.Users.Max(u => u.Profile_Picture);
                            string ProfileDirectory = dc.Users.Max(u => u.Profile_Picture);
                            ProfileDirectory = ProfileDirectory + 1;
                            ProfileIDPath = ProfileIDPath + 1;
                            Directory.CreateDirectory(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Profile_ID_" + ProfileDirectory));
                            HttpPostedFileBase file = model.user.Profile_Picture_imgPath;
                            if (file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Profile_ID_" + ProfileDirectory), ProfileIDPath + "_" + fileName);
                                file.SaveAs(path);
                                model.user.Profile_Picture = path;
                            }
                        }
                        if (id == "backgroundImage")
                        {
                            string BackgroundIDPath = dc.Users.Max(u => u.Background_Pic);
                            string BackgroundDirectory = dc.Users.Max(u => u.Background_Pic);
                            BackgroundDirectory = BackgroundDirectory + 1;
                            BackgroundIDPath = BackgroundIDPath + 1;
                            Directory.CreateDirectory(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Background_ID_" + BackgroundDirectory));
                            HttpPostedFileBase file1 = model.user.Background_Photo;
                            if (file1.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file1.FileName);
                                var path = Path.Combine(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Background_ID_" + BackgroundDirectory), BackgroundIDPath + "_" + fileName);
                                file1.SaveAs(path);
                                model.user.Background_Pic = path;
                            }
                        }
                        dc.Users.Add(model.user);
                        dc.SaveChanges();
                        Status = true;
                    }
                }
            }
            return View();
        }
        // GET: Profile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpPost]
        /* ActionResult Upload(HttpPostedFileBase file) {
                if (file != null && file.ContentLength > 0) {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                }
            return RedirectToAction("Index");
        }*/
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

       /* [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilePic(ProfilePageModel model)
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
                        string ProfileIDPath = dc.Users.Max(u => u.Profile_Picture);
                        string ProfileDirectory = dc.Users.Max(u => u.Profile_Picture);
                        ProfileDirectory = ProfileDirectory + 1;
                        ProfileIDPath = ProfileIDPath + 1;
                        Directory.CreateDirectory(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Profile_ID_" + ProfileDirectory));
                        HttpPostedFileBase file = model.user.Profile_Picture_imgPath;
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Profile_ID_" + ProfileDirectory), ProfileIDPath + "_" + fileName);
                            file.SaveAs(path);
                            model.user.Profile_Picture = path;
                        }
                        dc.Users.Add(model.user);
                        dc.SaveChanges();
                        Status = true;
                    }
                }
            }
            return View("Index", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult BackgroundPic(ProfilePageModel model)
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
                        string BackgroundIDPath = dc.Users.Max(u => u.Background_Pic);
                        string BackgroundDirectory = dc.Users.Max(u => u.Background_Pic);
                        BackgroundDirectory = BackgroundDirectory + 1;
                        BackgroundIDPath = BackgroundIDPath + 1;
                        Directory.CreateDirectory(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Background_ID_" + BackgroundDirectory));
                        HttpPostedFileBase file1 = model.user.Background_Photo;
                        if (file1.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file1.FileName);
                            var path = Path.Combine(Server.MapPath("~/Users/User_ID_" + v.User_ID + "/Background_ID_" + BackgroundDirectory), BackgroundIDPath + "_" + fileName);
                            file1.SaveAs(path);
                            model.user.Background_Pic = path;
                        }
                        dc.Users.Add(model.user);
                        dc.SaveChanges();
                        Status = true;
                    }
                }
            }
            return View("Index", model);
        }*/
    }
}
