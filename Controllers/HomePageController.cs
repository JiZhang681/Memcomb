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
using System.Windows;

namespace Memcomb.Controllers
{
    public class HomePageController : Controller
    {

        // GET: HomePage
        public ActionResult Index()
        {
            memcombdbEntities db = new memcombdbEntities();

            List<Memory> memoryList = new List<Memory>();
            List<Fragment> fragmentList = new List<Fragment>();

            //var fragment = from Fragments in db.Fragments select Fragments;

            foreach (var item in db.Memories)
            {
                Memory mem = db.Memories.Find(item.Memory_ID);
                
                var v = db.Fragments.Where(a => a.Memory_ID == item.Memory_ID);
                foreach (var s in v)  
                {   
                    fragmentList.Add(new Fragment
                    {
                        Memory_ID = s.Memory_ID,
                        Fragment_ID = s.Fragment_ID,
                        Fragment_Date = s.Fragment_Date,
                        Fragment_Data = s.Fragment_Data,
                        Fragment_Location = s.Fragment_Location
                    });
                }
                memoryList.Add(new Memory
                {
                    Memory_ID = mem.Memory_ID,
                    Memory_Title = mem.Memory_Title,
                    Memory_Description = mem.Memory_Description,
                    fragmentList = fragmentList
                });
            }   
            return View(memoryList);

        }
        
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(/*[Bind(Exclude = "IsEmailVerified,ActivationCode")]*/ Memory model)
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
                            Memory_Title = model.Memory_Title,
                            Memory_Description = model.Memory_Description
                        };
                        
                        Directory.CreateDirectory(Server.MapPath("~/Memories/User_ID_" + v.User_ID + "/Memory_ID_" + memoryIDForFolder));
                        
                        HttpPostedFileBase file = model.Fragment.getImagePath;
                        
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Memories/User_ID_" + v.User_ID + "/Memory_ID_" + memoryIDForFolder), fragmentIDPath + "_" + fileName );
                            file.SaveAs(path);
                       
                            model.Fragment.Fragment_Data = path;
                        }

                        dc.Memories.Add(newMemory);
                        dc.Fragments.Add(model.Fragment);
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
            return RedirectToAction("Index");

        public ActionResult CreateMemory()
        {
            var co_val = Response.Cookies["email"].Value;
            Console.WriteLine(co_val);
            return View();
        }
    }
}