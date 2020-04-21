using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

            List<User> userList = new List<User>();
            List<Memory> memoryList = new List<Memory>();
            List<Fragment> fragmentList = new List<Fragment>();
            List<Comment> commentList = new List<Comment>();


            foreach (var u in db.Users)
            {
                User user = db.Users.Find(u.User_ID);

                var getProFilePic = "";

                if (u.Profile_Picture != null)
                {
                    var temp = u.Profile_Picture.Replace(@"C:\Users\17347\Desktop\Capstone Project\Github\MemcombRepo\Memcomb", "");
                    getProFilePic = temp;
                }
                else
                {
                    getProFilePic = @"\Users\Default\Profile_Pic\rename.jpg";
                }

                var m = db.Memories.Where(a => a.User_ID == u.User_ID);
                foreach (var item in m)
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
                            Memory_Description = s.Memory_Description,
                            Fragment_Location = s.Fragment_Location,
                            Is_Highlight = s.Is_Highlight
                        });
                    }

                    var x = db.Comments.Where(a => a.Memory_ID == item.Memory_ID);

                    foreach (var com in x)
                    {
                        commentList.Add(new Comment
                        {
                            Memory_ID = mem.Memory_ID,
                            Comment1 = com.Comment1,
                            firstName = user.First_Name,
                            lastName = user.Last_Name
                        });
                    }
                    

                    memoryList.Add(new Memory
                    {
                        User_ID = mem.User_ID,
                        getFirstName = user.First_Name,
                        getLastName = user.Last_Name,
                        getProfilePic = getProFilePic,
                        Memory_ID = mem.Memory_ID,
                        Memory_Title = mem.Memory_Title,
                        Memory_Description = mem.Memory_Description,
                        Date_Created = mem.Date_Created,
                        Comments = commentList,
                        Fragments = fragmentList
                    });
                }

                userList.Add(new User
                {
                    User_ID = u.User_ID,
                    First_Name = u.First_Name,
                    Last_Name = u.Last_Name,
                    memoryList = memoryList
                });
            }

            memoryList = memoryList.OrderBy(e => e.Date_Created).ToList();

            return View(memoryList); 
            //return View(test);
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
                            Date_Created = DateTime.Now,
                            Memory_Title = model.Memory_Title,
                            Memory_Description = model.Memory_Description
                        };

                        Directory.CreateDirectory(Server.MapPath("~/Memories/User_ID_" + v.User_ID + "/Memory_ID_" + memoryIDForFolder));

                        List<Fragment> fragmentList = new List<Fragment>();

                        var checkForHighlight = false;

                        foreach (Fragment frag in model.Fragments.ToList())
                        {
                            HttpPostedFileBase file = frag.getImagePath;

                            if (file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/Memories/User_ID_" + v.User_ID + "/Memory_ID_" + memoryIDForFolder), fragmentIDPath + "_" + fileName);
                                file.SaveAs(path);

                                if (frag.Is_Highlight == true)
                                {
                                    checkForHighlight = true;
                                }

                                fragmentList.Add(new Fragment
                                {
                                    Fragment_Date = frag.Fragment_Date,
                                    Fragment_Data = path,
                                    Memory_Description = frag.Memory_Description,
                                    Fragment_Location = frag.Fragment_Location,
                                    Is_Highlight = frag.Is_Highlight
                                });
                            }

                        }

                        if(checkForHighlight == false)
                        {
                            var firstElement = fragmentList.First();
                            firstElement.Is_Highlight = true;
                        }

                        dc.Memories.Add(newMemory);
                        foreach (var frag in fragmentList)
                        {
                            dc.Fragments.Add(frag);
                        }
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

        }

        public ActionResult Comment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                using (memcombdbEntities db = new memcombdbEntities())
                {
                    HttpCookie cookie = HttpContext.Request.Cookies.Get("userIDCookie");
                    var v = db.Users.Where(a => a.Email_ID == cookie.Value).FirstOrDefault();

                    Comment newComment = new Comment()
                    {
                        User_ID = v.User_ID,
                        Datetime_Posted = DateTime.Now,
                        Comment1 = comment.Comment1,
                        Memory_ID = comment.Memory_ID
                    };

                    db.Comments.Add(newComment);
                    db.SaveChanges();
                }
            }


            return RedirectToAction("Index");
        }

    }
}