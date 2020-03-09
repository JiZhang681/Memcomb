using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Memcomb.Models;
using System.Web.Security;


namespace Memcomb.Controllers
{
    public class HomePageController : Controller
    {

        // GET: HomePage
        public ActionResult Index()
        {
            return View();
        }

        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(/*[Bind(Exclude = "IsEmailVerified,ActivationCode")]*/ HomePageModel model)
        {
            bool Status = false;
            string message = "";
            //
            // Model Validation
            if (ModelState.IsValid)
            {
                #region Save to database
                using (memcombdbEntities dc = new memcombdbEntities())
                {
                     
                    if (HttpContext.Request.Cookies["userIDCookie"] != null)
                    {
                        HttpCookie cookie = HttpContext.Request.Cookies.Get("userIDCookie");
                        var v = dc.Users.Where(a => a.Email_ID == cookie.Value).FirstOrDefault();

                        int memoryIDForFolder = dc.Memories.Max(u => u.Memory_ID);
                        int fragmentIDPath = dc.Fragments.Max(u => u.Fragment_ID);

                       memoryIDForFolder = memoryIDForFolder + 1;
                       fragmentIDPath = fragmentIDPath + 1;

                        Memory newMemory = new Memory()
                        {
                            User_ID = v.User_ID,
                            Memory_Title = model.memory.Memory_Title,
                            Memory_Description = model.memory.Memory_Description
                        };

                        Directory.CreateDirectory(Server.MapPath("~/Memories/User_ID_" + v.User_ID + "/Memory_ID_" + memoryIDForFolder));
                        
                        HttpPostedFileBase file = model.fragment.getImagePath;
                        
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Memories/User_ID_" + v.User_ID + "/Memory_ID_" + memoryIDForFolder), fragmentIDPath + "_" + fileName );
                            file.SaveAs(path);
                       
                            model.fragment.Fragment_Data = path;
                        }

                        dc.Memories.Add(newMemory);
                        dc.Fragments.Add(model.fragment);
                        dc.SaveChanges();
                        Status = true;  
                    }
                }
                #endregion
            }
            else
            {
                message = "Invalid request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(model);

        }

        public ActionResult CreateMemory()
        {
            return View();
        }
    }
}