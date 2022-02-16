using StudentList.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentList.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        StudentsEntities dbConnection = new StudentsEntities();

        public ActionResult Index()
        {
            var getdata = dbConnection.Students.ToList();
            return View(getdata);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student s)
        {
            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                string extention = Path.GetExtension(s.ImageFile.FileName);
                HttpPostedFileBase postedFile = s.ImageFile;
                int length = postedFile.ContentLength;
                if (extention.ToLower() == ".jpg" || extention.ToLower() == ".jpeg" || extention.ToLower() == ".png")
                {
                    if (length <= 100000)
                    {
                        fileName = fileName + extention;
                        s.Image = "~/images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                        s.ImageFile.SaveAs(fileName);
                        dbConnection.Students.Add(s);
                        int a = dbConnection.SaveChanges();
                        if (a > 0)
                        {
                            TempData["CreateMessage"] = "<script>alert('Data inserted Successfully.')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["CreateMessage"] = "<script>alert('Data not inserted.')</script>";
                        }
                    }
                    else
                    {
                        TempData["SizeMessage"] = "<script>alert('Image size should be less than 100 kB')</script>";
                    }
                }
                else
                {
                    TempData["ExtentionMessage"] = "<script>alert('Format Not Supported')</script>";
                }
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            var StudnetR = dbConnection.Students.Where(model => model.Id == id).FirstOrDefault();
            Session["Image"] = StudnetR.Image;
            return View(StudnetR);
        }
        [HttpPost]
        public ActionResult Edit(Student s)
        {
            if (ModelState.IsValid == true)
            {
                if (s.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
                    string extention = Path.GetExtension(s.ImageFile.FileName);
                    HttpPostedFileBase postedFile = s.ImageFile;
                    int length = postedFile.ContentLength;
                    if (extention.ToLower() == ".jpg" || extention.ToLower() == ".jpeg" || extention.ToLower() == ".png")
                    {
                        if (length <= 100000)
                        {
                            fileName = fileName + extention;
                            s.Image = "~/images/" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            s.ImageFile.SaveAs(fileName);
                            dbConnection.Entry(s).State = EntityState.Modified;
                            int a = dbConnection.SaveChanges();
                            if (a > 0)
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data Updated Successfully.')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data not updated.')</script>";
                            }
                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('Image size should be less than 100kb')</script>";
                        }
                    }
                    else
                    {
                        TempData["ExtentionMessage"] = "<script>alert('Format Not Supported.')</script>";
                    }
                }
                else
                {
                    s.Image = Session["Image"].ToString();
                    dbConnection.Entry(s).State = EntityState.Modified;
                    int a = dbConnection.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data Updated Successfully')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data not Updated')</script>";
                    }
                }
            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var StudentR = dbConnection.Students.Where(model => model.Id == id).FirstOrDefault();
                if (StudentR != null)
                {
                    dbConnection.Entry(StudentR).State = EntityState.Deleted;
                    int a = dbConnection.SaveChanges();
                    if (a > 0)
                    {
                        TempData["DeleteMessage"] = "<script>alert('Data deleted successfully.')</script>";
                    }
                    else
                    {
                        TempData["DeleteMessage"] = "<script>alert('Data not deleted.')</script>";
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}