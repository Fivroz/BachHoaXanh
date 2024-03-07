using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BHX.Controllers
{
    public class ADMINController : Controller
    {
        // GET: ADMIN
        public ActionResult NVKD()
        {
            if (Session["ID_ADMIN"] != null && (int)(short)Session["ROLE_ADMIN"] == 2)
            {
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult NVIT()
        {
            if (Session["ID_ADMIN"] != null && (int)(short)Session["ROLE_ADMIN"] == 1)
            {
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Login", "Home");
            } 
        }
        public ActionResult NVGH()
        {
            if (Session["ID_ADMIN"] != null && (int)(short)Session["ROLE_ADMIN"] == 3)
            {
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult NVK()
        {
            if (Session["ID_ADMIN"] != null && (int)(short)Session["ROLE_ADMIN"] == 4)
            {
                return View();
            }
            else
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Login", "Home");
            }
        }
    }
}