using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Memcomb.Models;
using System.Net;
using System.Web.Security;
using System.IO;

namespace Memcomb.Controllers
{
    public class SignUpPageController : Controller
    {
        // GET: SignUpPage
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
                

                // Email already registered
                #region
                var isRegistered = IsEmailRegistered(user.Email_ID);
                if (isRegistered)
                {
                    ModelState.AddModelError("EmailRegistered", "Email already registered");
                    return View(user);
                }
                #endregion

                /*
                #region Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion
                */


                #region Password Hashing
                //user.Password = Encrypt.Hash(user.Password);
                //user.ConfirmPassword = Encrypt.Hash(user.ConfirmPassword);
                #endregion

               // user.IsEmailVerified = false;

                
                #region Save to database
                using (memcombdbEntities dc = new memcombdbEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    //Send confirmation email to user
                    SendVerificationLinkEmail(user.Email_ID, user.User_ID.ToString());
                    message = "Registration was successful. Check your email for verification link " +
                        "at your email: " + user.Email_ID;
                    Status = true;
                    Directory.CreateDirectory(Server.MapPath("~/Memories/User_ID" + user.User_ID));
                    return View("~/Views/LogInPage/Index.cshtml");
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

        /*
        //Verify Email
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (memcombdbEntities dc = new memcombdbEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; //Makes sure confirm password field does not match on save
                var v = dc.Users.Where(a => a.Activation_Code == new Guid(id)).FirstOrDefault(); 
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }*/

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /* Move to future log out button
        //Logout 
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        */

        [NonAction]
        public bool IsEmailRegistered(string emailID)
        {
            using (memcombdbEntities dc = new memcombdbEntities())
            {
                var v = dc.Users.Where(a => a.Email_ID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("memcombemailsender@gmail.com");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "Lol123123";

            string subject = "";
            string body = "";


            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created";

                body = "<br/><br/>You created an account and it was successful. " +
                    "Click the link to verify the account."
                    + "<br/><br/> <a href ='" + link + "'> " + link + " </a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";

                body = "Hello,<br/><br/>We have received your request to reset the account password associated with your Email Address." +
                    "Please click the link below to reset your password.<br/><br/><a href=" + link + ">Reset Password</a>";
            }



            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Timeout = 20000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("memcombemailsender@gmail.com", "Lol123123")
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

                smtp.Send(message);
        }
        
        //Forgot password

        public ActionResult ForgotPassword()
        {
            return View();
        }

        /* Login Page
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link
            //Send Email
            string message = "";
            bool status = false;

            using (memcombdbEntities dc = new memcombdbEntities())
            {
                var account = dc.Users.Where(a => a.Email_ID == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for password reset
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email_ID, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;

                    dc.Configuration.ValidateOnSaveEnabled = false; //Avoids checking if the passwords match (confirm password from Model1
                    dc.SaveChanges();
                    message = "Reset password link has been sent to your Email Address";
                }
                else
                {
                    message = "Email not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }*/

            /* Profile Page
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with the link
            //redirect to reset password page
            using (memcombdbEntities dc = new memcombdbEntities())
            {
                var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }*/

        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (memcombdbEntities dc = new memcombdbEntities())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Encrypt.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";

                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }

            ViewBag.Message = message;
            return View(model);
        }*/
    }
}
