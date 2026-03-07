using E_education.App_Code;
using E_education.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Sql;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_education.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        EducationDBEntities dbs =new EducationDBEntities();
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult StudentManagement()
        {
            List<Registration_Master> ldb = dbs.Registration_Master.ToList();
            return View(ldb);
        }
        public ActionResult UploadAssignment()
        {

            return View();
        }
        [HttpPost]
        public ActionResult UploadAssignment(Admin_UploadAssignment ua)
        {
            string msg = string.Empty;
            Admin_UploadAssignment adb = new Admin_UploadAssignment();
            try
            {
                string filepath = "/content/AdminUploadAssignment/";
                HttpPostedFileBase hp = Request.Files["Filename"];
                if (hp.ContentLength > 0)
                {
                    string fname = Path.GetRandomFileName() + "_" + hp.FileName;
                    adb.Description = ua.Description;
                    adb.Title = ua.Title;
                    adb.Description = ua.Description;
                    adb.FileName = fname;
                    adb.Upload_Dt = DateTime.Now.ToString();
                    dbs.Admin_UploadAssignment.Add(adb);
                    dbs.SaveChanges();
                    msg = "Assignment upload successfully";

                }
                else
                {
                    msg = "Please Upload assignment";
                }
            }
            catch(Exception ex)
            {
                msg = "Technical issue Accoured.Please try again";
            }
            ViewBag.Message = msg;
            return View();
        }
      
        public ActionResult StudyMaterials()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StudyMaterials(Tbl_uploadstudyMaterial ua)
        {
            string msg = string.Empty;
            Tbl_uploadstudyMaterial addb = new Tbl_uploadstudyMaterial();
            try
            {
                string filepath = "/content/Tbl_uploadstudyMaster/";
                HttpPostedFileBase hp = Request.Files["FileName"];
                if (hp.ContentLength > 0)
                {
                    string fname = Path.GetRandomFileName() + "_" + hp.FileName;
                    hp.SaveAs(Server.MapPath(filepath + fname));
                    addb.Description = ua.Description;
                    addb.title = ua.title;
                    addb.Subject = ua.Description;
                    addb.FileName = fname;
                    addb.Upload_DT = DateTime.Now.ToString();
                    dbs.Tbl_uploadstudyMaterial.Add(addb);
                    dbs.SaveChanges();
                    msg = "Study material upload successfully";

                }
                else
                {
                    msg = "Please upload assignment";
                }
            }
            catch(Exception ex)
            {
                msg = "Technical issue accured.please try again";
            }
            ViewBag.Message = msg;
            return View();
        }
        public ActionResult NotificationManagement()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NotificationManagement(Tbl_Notification nm)
        {
            List<Tbl_Notification> listenq = dbs.Tbl_Notification.OrderByDescending(z => z.Noti_Message).ToList();
            return View(listenq);
        }
        public ActionResult FeedbackManagement()
        {
            List<Feedback_Tbl> lst = dbs.Feedback_Tbl.ToList();
            return View(lst);
        }
        public ActionResult AddNotification(Tbl_Notification nm)
        {
            string msg = " ";
            try
            {
                Tbl_Notification dnm = new Tbl_Notification();
                dnm.Noti_Message = nm.Noti_Message;
                dnm.Noti_DT = DateTime.Now.ToString();
                dbs.Tbl_Notification.Add(dnm);
                dbs.SaveChanges();
                msg = "Notification added successfully";
            }
            catch
            {
                msg = "Technical issue accured";
            }
            ViewBag.Message = msg;
            return View();
        }
        public ActionResult DeleteNotification(int id)
        {
            string msg = " ";
            try
            {
                Tbl_Notification em = dbs.Tbl_Notification.Find(id);
                    if (em != null)
                {
                    dbs.Tbl_Notification.Remove(em);
                    dbs.SaveChanges();
                    msg = "Notification deleted successfully";
                }
            }
            catch
            {
                msg = "Technical issue accoured";
            }
            TempData["Message"] = msg;
            return RedirectToAction("NotificationManagement");
        }
        public ActionResult EnquiryManagement()
        {
            List<Tbl_Enquiry> listenq = dbs.Tbl_Enquiry.OrderByDescending(z => z.Enquiry_id).ToList();

            return View(listenq);
        }
        //delete enquiry
        public ActionResult DeleteEnquery(int id)
        {
            string msg = " ";
            try
            {
                Tbl_Enquiry em = dbs.Tbl_Enquiry.Find(id);
                if (em != null)
                {
                    dbs.Tbl_Enquiry.Remove(em);
                    dbs.SaveChanges();
                    msg = "Enquery deleted successfully";
                }
            }
            catch
            {
                msg = "Technical issued accured";
            }
            TempData["Message"] = msg;

            return RedirectToAction("EnquiryManagement");
        }
        public ActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(string senderEmail,string Subject,string Message)
        {
            string msg = " ";
            try
            {
                EmailSender em = new EmailSender();
                em.sendMyEmail(senderEmail, Subject, Message);
                msg = "Email send successfully";
            }
            catch
            {
                msg = "Sorry! Some technical issue accured";
            }
            ViewBag.Message = msg;
            return View();
        }
        public ActionResult AssignmentManagement()
        {
            List<Tbl_uploadAssignment> lst = dbs.Tbl_uploadAssignment.ToList();
            return View(lst);
        }
        public FileResult AdminDownloadAssignment(string filename)
        {
            string path = "/content/suploadassignment/" + filename;
            return File(path, "application-force/download", Path.GetFileName(path));
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string CurrPass,string NewPass,string ConfPass)
        {
            string msg=string .Empty;
            try
            {
                string adminid = Session["Aid"].ToString();//uid for student zone
                incriptionManager em = new incriptionManager();
                string encrypass = em.encryptmypass(CurrPass);
                Tbl_Login lmdb = dbs.Tbl_Login.SingleOrDefault(a => a.Email_id.Trim() == adminid.Trim() && a.Pass.Trim() == encrypass.Trim());
                if (lmdb == null)
                {
                    if (NewPass == ConfPass)
                    {
                        if (NewPass != CurrPass)
                        {
                            encrypass = em.encryptmypass(NewPass);
                            lmdb.Pass = encrypass;
                            dbs.Entry(lmdb);
                            dbs.SaveChanges();
                            msg = "password changed Successfully";
                        }
                        else
                        {
                            msg = "Please enter new Password";
                        }
                    }
                    else
                    {
                        msg = "New Password and confirm password must be same";
                    }
                }
                else
                {
                    msg = "Invalid current pasword.Please try again";
                }

            }
            catch
            {
                msg = "Technical issue accured";
            }
            ViewBag.Message = msg;
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.Remove("Aid");
            Session.Clear();
            return View();
        }

    }
}