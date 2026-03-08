using E_education.App_Code;
using E_education.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_education.Controllers
{
    //[AuthorisedStudent]
    public class StudentController : Controller
    {
        // GET: Student
       EducationDBEntities dbs=new EducationDBEntities();
        public void getuser()
        {
            string userid = Session["uid"].ToString();
            Registration_Master rm = dbs.Registration_Master.Find(userid);
            if(rm != null)
            {
                ViewBag.Username = rm.Name;
                ViewBag.Userpic = rm.pic_Upload;
            }
        }
        public ActionResult HDashboard()
        {
            getuser();
            return View();
        }
        public  ActionResult ViewAssignment()
        {
            List<Tbl_uploadAssignment> lst = dbs.Tbl_uploadAssignment.OrderByDescending(x => x.Upload_Id).ToList();
            getuser();
            return View();
        }
        public FileResult DownloadAssignment(string filename)
        {
            string path = "/content/Tbl_uploadstudyMaterial/" + filename;
            return File(path, "application-force/download", Path.GetFileName(path));
        }
        public ActionResult ViewStudyMaterial()
        {
            List<Tbl_uploadstudyMaterial> lst = dbs.Tbl_uploadstudyMaterial.OrderByDescending(x => x.Upload_DT).ToList();
            getuser();
            return View(lst);
        }
        public ActionResult UploadAssignment()
        {
            getuser();
            return View();
        }
        [HttpPost]
        public ActionResult UploadAssignment(Tbl_uploadAssignment ua)
        {
            string msg = string.Empty;
            string userid = Session["uid"].ToString();
            Tbl_uploadAssignment adb = new Tbl_uploadAssignment();
            try
            {
                string filepath = "/content/suploadassignment/";
                HttpPostedFileBase hp = Request.Files["FileName"];
                if (hp.ContentLength > 0)
                {
                    string fname = Path.GetRandomFileName() + "_" + hp.FileName;
                    hp.SaveAs(Server.MapPath(filepath + fname));
                    adb.Description = ua.Description;
                    adb.FileName = fname;
                    //adb.Sub = Userid;
                    adb.Upload_DT = DateTime.Now.ToString();
                    dbs.Tbl_uploadAssignment.Add(adb);
                    dbs.SaveChanges();
                    msg = "Assignment uploaded successfully";

                }
                else
                {
                    msg = "please upload assignment";
                }
            }
            catch(Exception ex)
            {
                msg = "Technical error accored.please try again";
            }
            ViewBag.Message = msg;
            return View();
        }
        public ActionResult Feedback()
        {
            getuser();
            return View();
        }
        [HttpPost]
        public ActionResult Feedback(Feedback_Tbl fm)
        {
            string msg = string.Empty;
            try
            {
                Feedback_Tbl f = new Feedback_Tbl();
                string userid = Session["uid"].ToString();
                f.Topic = fm.Message;
                f.Message = fm.Message;
                f.UserId = userid;
                f.Feedback_DT = DateTime.Now.ToString();
                dbs.Feedback_Tbl.Add(f);
                dbs.SaveChanges();
                msg = "Feedback added Successfully";
            }
            catch
            {
                msg = "Technical issue accured";
            }
            getuser();
            ViewBag.Message=msg;
            return View();
        }
        public ActionResult MyProfile()
        {
            string userid=Session["uid"].ToString();
            Registration_Master rm = dbs.Registration_Master.Find(userid);
            getuser();
            return View(rm);
        }
        [HttpPost]
        public ActionResult MyProfile(Registration_Master rm)
        {
            string userid = Session["uid"].ToString();
            Registration_Master rg = new Registration_Master();
            try
            {
                Registration_Master rgbd = dbs.Registration_Master.Find(userid);
                //file uploading
                HttpPostedFileBase f = Request.Files["pic_Upload"];
                if (f.ContentLength > 0)
                {
                    int sizeinkb = 0, size;
                    string extension;
                    size = f.ContentLength;
                    sizeinkb = size / 1024;
                    string fpath = Path.GetRandomFileName() + "_" + f.FileName;
                    extension = fpath.Substring(fpath.LastIndexOf(".")).ToUpper();
                    if (sizeinkb < 2048)
                    {
                        if (extension == ".PNG" || extension == ".JPEG" || extension == ".JPG")
                        {
                            string filepath = "/content/pic";
                            rgbd.pic_Upload = fpath;
                            f.SaveAs(Server.MapPath(filepath + fpath));
                        }
                        else
                        {
                            ViewBag.RMessage = "Invalid file type.Please choose valid file";
                        }
                    }
                    else
                    {
                        ViewBag.RMessage = "file size large .please upload file having size less then 300kb";
                    }

                }
                //rgbd.Email_id = rm.Email_id;
                rgbd.Name = rm.Name;
                rgbd.Gender = rm.Gender;
                rgbd.College = rm.College;
                rgbd.Year = rm.Year;
                rgbd.Course = rm.Course;
                // rgbd.pic_Upload = rm.pic_Upload;
                rgbd.DOB = rm.DOB;
                rgbd.Address = rm.Address;
                rg.Registered_on = rm.Registered_on;
                //saving in login master
                dbs.Entry(rgbd);
                dbs.SaveChanges();
                ViewBag.RMessage = "Congratulations ! profile updated successfully";
            }
            catch
            {
                ViewBag.RMessage = "Technical issue accored";
            }
            getuser();
            //getting the data of current user
            Registration_Master rms = dbs.Registration_Master.Find(userid);
            return View(rms);
        }
        
        public ActionResult ChangePassword() 
        {
            getuser();
          
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string Cpass,string NewPass,string ConfPass)
        {
            string msg = string.Empty;
            try
            {
                string studentid = Session["uid"].ToString();
                incriptionManager em = new incriptionManager();
                Tbl_Login lm = new Tbl_Login();
                string encrypass = em.encryptmypass(Cpass);
                Tbl_Login lmdb = dbs.Tbl_Login.SingleOrDefault(a => a.Email_id.Trim() == studentid.Trim() && a.Pass.Trim() == encrypass.Trim());
                if (lmdb != null)
                {
                    if (NewPass == ConfPass)
                    {
                        if (NewPass != Cpass)
                        {
                            encrypass = em.encryptmypass(NewPass);
                            lmdb.Pass = encrypass;
                            dbs.Entry(lmdb);
                            dbs.SaveChanges();
                            msg = "password changed succeessfully";
                        }
                        else
                        {
                            msg = "please enter new password";
                        }
                    }
                    else
                    {
                        msg = "New password and confirm password must be same ";
                    }
                }
                else
                {
                    msg = "Invalid current password .please try again";
                }
            
            }
            catch
            {
                msg = "Technical issue accored";
            }
            ViewBag.Message = msg;
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.Remove("uid");
            Session.Clear();

            return View();
        }
    }
}