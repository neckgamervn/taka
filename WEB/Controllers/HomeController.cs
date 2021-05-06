﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using taka.Models.DatabaseInteractive;
using taka.Models.Enitities;
using taka.Utils;

namespace taka.Controllers
{
    public class HomeController : Controller
    {

        TakaDB dB = new TakaDB();

        public ActionResult Index(int page = 1, string text = "", int cate = 0, int sort = 0)
        {
            ViewBag.ListCate = dB.GetCategories();
            ViewBag.Cate = cate;
            ViewBag.Sort = sort;
            ViewBag.CurrentPage = page;
            if (cate != 0)
            {
                ViewBag.TextCate = dB.findTextCategory(cate);
            }
            if (sort != 0)
            {
                ViewBag.TextSort = sort == 1 ? "Giá thấp nhất" : "Giá cao nhất";
            }
            ListBook listBook = dB.GetListBook(page, text, cate, sort);
            ViewBag.ListPage = HelperFunctions.getNumPage(page, listBook.pages);
            ViewBag.maxPage = listBook.pages;
            ViewBag.TextSearch = text;
            return View(listBook.books);
        }

        [HttpPost]
        public ActionResult Login(string phone, string password, string callbackUrl)
        {

            User user = dB.Login(phone, password);
            if (user != null)
            {
                Session["isLogin"] = true;
                Session["UserInfo"] = user;
            }
            else
            {
                TempData["Message"] = "Sai tài khoản hoặc mật khẩu";
                TempData["RequireLogin"] = true;
            }
            return Redirect(callbackUrl);
        }

        [HttpPost]
        public ActionResult Register(string phone, string password, string email, string gender, string fullname, string birthday, string callbackUrl)
        {
            User user = dB.Register(phone, password, email, gender, fullname, birthday);

            if (user != null)
            {
                Session["isLogin"] = true;
                Session["UserInfo"] = user;
            }
            return Redirect(callbackUrl);
        }

        public ActionResult Logout()
        {
            Session["isLogin"] = null;
            Session["UserInfo"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Detail(int id = -1)
        {
            try
            {
                if (id == -1)
                    throw new Exception("Not found");
                var item = dB.GetBookDetail(id);
                return View(item);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home");
            }

        }
        public ActionResult Error()
        {
            return View();
        }
    }
}