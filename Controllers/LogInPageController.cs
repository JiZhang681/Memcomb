using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Memcomb.Models;
using System.Net;
using System.Web.Security;

namespace Memcomb.Controllers
{

	public class LogInPageController : Controller
	{


		public ActionResult Submit()
		{

			return View();
		}

		public ActionResult Index()
		{
			return View();
		}

        
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(/*[Bind(Exclude = "IsEmailVerified,ActivationCode")]*/ User user)
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

                    Status = true;
                    return View();
                }
                #endregion

            }
            else
            {
                message = "Invalid request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);

        }

        public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
        
        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl)
        {
            string message = "";
            using (memcombdbEntities dc = new memcombdbEntities())
            {
                var v = dc.Users.Where(a => a.Email_ID == login.Email_ID).FirstOrDefault();
                if (v != null)
                {
		   // login.Password = Encrypt.Hash(login.Password); this is problem child
                    if (string.Compare(login.Password, v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20;
                        
                        HttpCookie userIDCookie = new HttpCookie("userIDCookie", login.Email_ID);
                        userIDCookie.Expires = DateTime.Now.AddMinutes(timeout);
                        Response.Cookies.Add(userIDCookie);

                        var ticket = new FormsAuthenticationTicket(login.Email_ID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        HttpCookie emailaddress = new HttpCookie("email");
                        emailaddress.Value = login.Email_ID;
                        Response.Cookies.Add(emailaddress);
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "HomePage");
                        }
                    }
                    else
                    {
                        message = "Invalid password provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }

            ViewBag.Message = message;
            return View();
        }
    }
}
