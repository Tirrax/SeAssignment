using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;
using BL;
using Common;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using SEWebiste_OwenAttard.Models;

namespace SEWebiste_OwenAttard.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        public IEnumerable<System.Web.Mvc.SelectListItem> GetCountries()
        {
            var countries = new UsersBL().GetCountries().Select(x => new System.Web.Mvc.SelectListItem
            {
                Value = x.CountryId,
                Text = x.Title.ToString()
            }).OrderBy(x => x.Text);

            return new System.Web.Mvc.SelectList(countries, "Value", "Text");
        }


        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToLocal(returnUrl);

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult UserAccount()
        {

            User user = new UsersBL().GetUserByUsername(User.Identity.Name);

            UserDetails model = new UserDetails()
            {
                isSeller = user.SellerApproved,
                IsAdmin = user.Roles.Any(x => x.RoleName == "Admin")
            };

            return View(model);
        }



        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (model.Password.IsEmpty() || model.UserName.IsEmpty())
                return View(model);

            UsersBL user = new UsersBL();

            int ret = user.AuthenitcateUser(model.UserName, FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5"));
            if (ret == (int)Common.GlobalDefines.LoginResponses.SUCCESS)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return RedirectToLocal(returnUrl);
            }

            String txt = "The user name or password provided is incorrect.";
            
            if (ret == (int)Common.GlobalDefines.LoginResponses.BLOCKED)
            {
                txt = "The Acount is blocked.";
            }


            model.ShowErrorBox = true;
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", txt);
            return View(model);
        }

        //
        // POST: /Account/LogOff        
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterModel
            {
                Countries = GetCountries(),
                ShowErrorBox =  false
            };

            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                User user = new User
                {
                    Address = model.Addres,
                    Country = model.SelectedCountryID,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MobileNumber = model.MobileNum,
                    Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5"),
                    Town = model.TownName,
                    Username = model.UserName,
                    SellerApproved = false
                };

                MembershipCreateStatus status = new UsersBL().AddNewUser(user);

                if (status == MembershipCreateStatus.Success)
                    return RedirectToAction("Index", "Home");

                //Email.SendEmail(model.Email, "Your account has been created please use this pin to login: " + user.Pin, null, "Account Created");

                ModelState.AddModelError("", ErrorCodeToString(status));
                model.ShowErrorBox = true;
            }

            model.Countries = GetCountries();
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        public ActionResult ManageAdmins()
        {
            UsersBL user = new UsersBL();

            if (!user.IsUserAdmin(User.Identity.Name))
                return RedirectToAction("Index", "Home");

            List<User> listUser = user.GetAllUsersExcept(User.Identity.Name).ToList();

            List<GeneralDetails> model = listUser.Select(curUser => new GeneralDetails()
            {
                Email = curUser.Email,
                Username = curUser.Username,
                IsAdmin = curUser.Roles.Any(x => x.RoleName == "Admin")

            }).ToList();

            return View(model);
        }
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
