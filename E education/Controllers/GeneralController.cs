using E_education.App_Code;
using E_education.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace E_education.Controllers
{
    public class GeneralController : Controller
    {
        // GET: General

        EducationDBEntities db = new EducationDBEntities();
        static cryptography c = new cryptography();
        public ActionResult Index()
        {
            c = c.getimagenamecode();
            string msg = "/content/captchapic/" + c.imagename;
            ViewBag.message = msg;
            return View();

        }

        public ActionResult NewStudent()
        {
            c = c.getimagenamecode();
            string msg = "/content/captchapic/" + c.imagename;
            ViewBag.message = msg;

            return View();
        }
        public ActionResult Enquiry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Enquiry(Tbl_Enquiry tm)
        {
            string msg = string.Empty;
            try
            {
                Tbl_Enquiry t = new Tbl_Enquiry();
                t.Name = tm.Name;
                t.Message = tm.Message;
                t.Mobile_No = tm.Mobile_No;
                t.Message = tm.Message;
                    t.Enquiry = DateTime.Now.ToString();
                    
                db.Tbl_Enquiry.Add(t);
                db.SaveChanges();
                msg = "Enquiry added successfully";
               
            }
            catch
            {
                msg = "Technical Issued occured";
            }
            ViewBag.Message = msg;
            return View();
        }
        public JsonResult getimagecaptcha()
        {
            c = c.getimagenamecode();
            string msg = "/content/captchapic/" + c.imagename;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult NewStudent(Registration_Master rm, string cpass, string captcha)
        {

            Registration_Master rg = new Registration_Master();
            Tbl_Login lg = new Tbl_Login();
            try
            {
                if (c.imagecode.Trim() == captcha.Trim())
                {
                    if (cpass == rm.Pass)
                    {
                        //file uploading
                        HttpPostedFileBase f = Request.Files["file_upload"];
                        if (f.ContentLength > 0)
                        {
                            int sizeinkb = 0, size;
                            string extension;
                            size = f.ContentLength;
                            sizeinkb = size / 1024;
                            string fpath = Path.GetRandomFileName() + "_" + f.FileName;
                            extension = fpath.Substring(fpath.LastIndexOf(".")).ToUpper();
                            if (sizeinkb < 300)
                            {
                                if (extension == ".PNG" || extension == ".JPG" || extension == "JPEG")
                                {
                                    string filepath = "/content/pic/";
                                    f.SaveAs(Server.MapPath(filepath + fpath));// "/content/pic/ViewBag.Userpic "
                                    rg.Email_id = rm.Email_id;
                                    rg.Name = rm.Name;
                                    rg.Gender = rm.Gender;
                                    rg.College = rm.College;
                                    rg.Year = rm.Year;
                                    rg.Course = rm.Course;
                                    rg.Mobile_No = rm.Mobile_No;
                                    rg.pic_Upload = fpath;
                                    rg.DOB = rm.DOB;
                                    rg.Address = rm.Address;
                                    incriptionManager eg = new incriptionManager();
                                    string encypass = eg.encryptmypass(rm.Pass);
                                    rg.Registered_on = DateTime.Now.ToString();
                                    //save in loginmaster
                                    lg.Email_id = rm.Email_id;
                                    lg.Pass = encypass;
                                    lg.UType = "STUDENT";
                                    db.Registration_Master.Add(rg);
                                    db.Tbl_Login.Add(lg);
                                    db.SaveChanges();
                                    ViewBag.RMessage = "Congratulation !You Are Registered successfully";
                                    EmailSender em = new EmailSender();
                                    string emsg = "Thanks for registering to our web portel.\n We will contact you soon";
                                    em.sendMyEmail(rm.Email_id, "E-Education", emsg);
                                }
                                else
                                {
                                    ViewBag.RMessage = "Invalid file type.please choose valid file type";
                                }
                            }
                            else
                            {
                                ViewBag.RMessage = "Please upload file having size less then 300kb";
                            }
                        }
                        else
                        {
                            ViewBag.RMeassage = "Please choose your profile pic";
                        }
                    }
                    else
                    {
                        ViewBag.RMessage = "Password and confirm password must be same";
                    }
                }
                else
                {
                    ViewBag.RMessage = "Invalid captcha.please try again";
                }
            }
            catch (Exception ex)
            {
                ViewBag.RMessage = "technical error";
            }
            c = c.getimagenamecode();
            string msg = "/content/captchapic/" + c.imagename;
            ViewBag.message = msg;
            return View();
        }
        public ActionResult VisionMission()
        {
            return View();
        }
        public ActionResult Benifits()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(Tbl_Login lm)
        { 
            try
            {
                incriptionManager em = new incriptionManager();
                lm.Pass = em.encryptmypass(lm.Pass);
                Tbl_Login lmdb = db.Tbl_Login.SingleOrDefault(x => x.Email_id.Trim() == lm.Email_id.Trim() &&x.Pass.Trim()==lm.Pass.Trim());
                if (lmdb != null)
                {
                     
                    if (lmdb.UType == "STUDENT" &&lm.UType=="STUDENT")
                    {
                        Session["Uid"] = lm.Email_id;
                        return RedirectToAction("HDashboard","Student");
                    }
                    else if (lmdb.UType.Trim() == "ADMIN" && lm.UType.Trim()== "ADMIN")
                    {
                        Session["Aid"] = lm.Email_id;
                        return RedirectToAction("Dashboard", "Admin");
                    }
                   
                }
                else
                {
                    ViewBag.Message = "Invalid Userid and password.please try again";
                    }

            }
            catch
            {
                ViewBag.Message = " sorry !Technical issue accured.";
            }


            return View();




    }
        }
}