using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StationaryApp.Data;
using StationaryApp.Models;

namespace StationaryApp.Controllers
{
    public class RetailerController : Controller
    {
        private readonly ILogger<RetailerController> _logger;
        private readonly StationaryAppContext db;
        public RetailerController(ILogger<RetailerController> logger, StationaryAppContext db)
        {
            this.db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userEmail = HttpContext.Session.GetString("useremail");
            var userName = HttpContext.Session.GetString("username");

            if (userEmail == null && userName == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.username = userName;
                ViewBag.useremail = userEmail;
            }
            var products = db.ProDetails.ToList();
            return View(products);
        }

        //---------Sign in work
        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signin(string userName, string userPass)
        {
            // 1. Check if either username or password is empty
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPass))
            {
                // Add error message for empty username or password
                ModelState.AddModelError("", "Username and Password cannot be empty.");
                return View();
            }

            // 2. Check if the user exists in the database with the provided username and password
            var retailer = db.Regretailers
                .FirstOrDefault(w => w.UserName == userName && w.UserPass == userPass);

            // var existingWholesaler = db.Regwholesalers.FirstOrDefault(w => w.UserName == ru.UserName);

            if (retailer != null)
            {
                if (retailer.UserStatus == "Deactive")
                {
                    // Account is deactivated, show the deactivation message
                    ViewBag.msg1 = "Your account has been deactivated by the wholesaler. Please contact admin.";
                    return View(); // Return the signup view with the message
                }
                else
                {
                    HttpContext.Session.SetString("username", retailer.UserName);
                    HttpContext.Session.SetString("useremail", retailer.UserEmail);

                    return RedirectToAction("Index", "Retailer");
                }
            }
            //if (wholesaler != null)
            //{
            //    // If user is found, set session and redirect to the dashboard
            //    HttpContext.Session.SetString("username", wholesaler.UserName);
            //    return RedirectToAction("Index", "Wholesaler");
            //}
            else
            {
                // If username or password does not match, add a validation error
                ModelState.AddModelError("", "Username and Password does not match.");
                return View();
            }

        }


        //-----------------logout work
        public IActionResult Signout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("useremail");
            return RedirectToAction("Selectrole", "Home");
        }

        //------sign up work---
        public IActionResult Signup()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Signup(Regretailer ru)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                ViewBag.msg1 = "All fields are required and must be valid.";
                return View(ru);
            }

            // Check if the email already exists in the database
            var existingretailer = db.Regretailers.Any(w => w.UserEmail == ru.UserEmail);
            if (existingretailer)
            {
                // If the email already exists, add an error message and return the view with the model
                ModelState.AddModelError("useremail", "This email has already been taken.");

                return View(ru);  // Return the same view to show the error message
            }

            // If email is unique, save the wholesaler details to the database
            db.Regretailers.Add(ru);
            db.SaveChanges();

            // Set success message using TempData (to be shown on the Signin page)
            TempData["msg1"] = "You can log in now.";

            // Redirect to the Signin page after successful signup
            return RedirectToAction("Signin");
        }

        //---------profile change
        [HttpGet]
        public IActionResult Profile()
        {
            var userEmail = HttpContext.Session.GetString("useremail");
            var userName = HttpContext.Session.GetString("username");

            if (userEmail == null && userName == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.username = userName;
                ViewBag.useremail = userEmail;
            }
            var notifications = db.Notifications
                                  .OrderByDescending(n => n.Timestamp)
                                  .Take(5) // Example: Get the latest 5 notifications
                                  .ToList();

            // Passing the notifications to the layout page
            ViewBag.Notifications = notifications;
            // Find the wholesaler based on email or username from the session
            var retailer = db.Regretailers.FirstOrDefault(w => w.UserEmail == userEmail && w.UserName == userName);

            // If wholesaler is found, pass it to the view
            if (retailer != null)
            {
                ViewBag.username = retailer.UserName;
                ViewBag.useremail = retailer.UserEmail;
                return View(retailer);  // Pass the specific wholesaler object to the view
            }
            else
            {
                return RedirectToAction("Selectrole", "Home");
            }
        }

        [HttpPost]
        public IActionResult Profile(string NewPassword)
        {
            var userEmail = HttpContext.Session.GetString("useremail");
            var userName = HttpContext.Session.GetString("username");

            if (userEmail == null && userName == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.username = userName;
                ViewBag.useremail = userEmail;
            }

            var notifications = db.Notifications
                                  .OrderByDescending(n => n.Timestamp)
                                  .Take(5) // Example: Get the latest 5 notifications
                                  .ToList();

            // Passing the notifications to the layout page
            ViewBag.Notifications = notifications;
            // Find the wholesaler based on email or username from the session
            var retailer = db.Regretailers.FirstOrDefault(w => w.UserEmail == userEmail && w.UserName == userName);

            if (retailer != null)
            {
                // If a new password is provided, update it
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    retailer.UserPass = NewPassword;  // In a real app, hash the password before saving
                    db.SaveChanges();
                    TempData["msg1"] = "Password updated successfully.";
                }

                // Fetch the updated wholesaler from the database to pass to the view
                var updatedretailer = db.Regretailers.FirstOrDefault(w => w.UserEmail == userEmail && w.UserName == userName);

                // Pass the updated wholesaler to the view
                return RedirectToAction("Profile", new { userEmail = updatedretailer.UserEmail, userName = updatedretailer.UserName });
            }
            else
            {
                return RedirectToAction("Selectrole", "Home");
            }
        }

        //forgot password
        [HttpGet]
        public IActionResult FPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FPassword(string email)
        {
            // Find the wholesaler based on the email
            var retailer = db.Regretailers.FirstOrDefault(w => w.UserEmail == email);

            if (retailer != null)
            {
                // If the email is valid, display the password in a popup
                TempData["PasswordPopup"] = $"Your password is: {retailer.UserPass}";


            }
            else
            {
                // If the email is incorrect, show an error message
                TempData["Error"] = "Your email is incorrect, unable to retrieve your password.";
            }

            return RedirectToAction("FPassword");
        }

        public IActionResult rsaleslist()
        {
            var userEmail = HttpContext.Session.GetString("useremail");
            var userName = HttpContext.Session.GetString("username");

            if (userEmail == null && userName == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.username = userName;
                ViewBag.useremail = userEmail;
            }
           
            ViewBag.pro = db.ProDetails.ToList();
            return View();
        }
        public IActionResult rshowsaleslist(int id)
        {
            // Fetch product details
            var productdetails = db.ProDetails
                                    .Where(p => p.ProId == id)
                                    .FirstOrDefault();

            if (productdetails == null)
            {
                return NotFound();
            }

            // Fetch the corresponding order details for the product, including OrderDate
            var orderDetails = db.Retailerorders
                                     .Where(ro => ro.RproIdfk == id)
                                     .Select(ro => new { ro.OrderDate })
                                     .FirstOrDefault();

            // If order details are found, assign the OrderDate
            DateTime? orderDate = orderDetails?.OrderDate;

            // Get user email and username from session
            var userEmail = HttpContext.Session.GetString("useremail");
            var userName = HttpContext.Session.GetString("username");

            if (userEmail == null && userName == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.username = userName;
                ViewBag.useremail = userEmail;
            }

            // Fetch latest notifications (example: last 5 notifications)
            var notifications = db.Notifications
                                  .OrderByDescending(n => n.Timestamp)
                                  .Take(5)
                                  .ToList();

            // Passing the notifications to the layout page
            ViewBag.Notifications = notifications;

            // Passing OrderDate to the view
            ViewBag.OrderDate = orderDate;

            // Passing product details to the view
            return View(productdetails);

            //// Fetch the product by the provided id
            //var productdetails = db.ProDetails.FirstOrDefault(p => p.ProId == id);

            ////// If product is not found, return a NotFound result
            ////if (productdetails != null)
            ////{
            ////  // Pass the product details to the view
            ////ViewBag.prod = productdetails;
            ////}



            //var userEmail = HttpContext.Session.GetString("useremail");
            //var userName = HttpContext.Session.GetString("username");

            //if (userEmail == null && userName == null)
            //{
            //    return RedirectToAction("Selectrole", "Home");
            //}
            //else
            //{
            //    ViewBag.username = userName;
            //    ViewBag.useremail = userEmail;
            //}
            //var notifications = db.Notifications
            //                      .OrderByDescending(n => n.Timestamp)
            //                      .Take(5) // Example: Get the latest 5 notifications
            //                      .ToList();

            //// Passing the notifications to the layout page
            //ViewBag.Notifications = notifications;
            //return View(productdetails);
        }

        [HttpPost]
        public IActionResult rshowsaleslist(int proid, int pqty, string totalprice, string catename, Retailerorder ro)
        {
            // Fetch the product details from the database
            var product = db.ProDetails.FirstOrDefault(p => p.ProId == proid);

            if (product == null)
            {
                return NotFound();
            }

            // Get current user session information
            var userEmail = HttpContext.Session.GetString("useremail");
            var userName = HttpContext.Session.GetString("username");

            if (userEmail == null || userName == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }

            // Fetch notifications for the layout
            var notifications = db.Notifications
                                   .OrderByDescending(n => n.Timestamp)
                                   .Take(5) // Example: Get the latest 5 notifications
                                   .ToList();

            // Passing the notifications to the layout page
            ViewBag.Notifications = notifications;
            ViewBag.username = userName;
            ViewBag.useremail = userEmail;



            // Check for quantity restrictions
            if (pqty < 10 || pqty > 50)
            {
                ViewBag.msg = "Quantity must be between 10 and 50.";
                return View(product);
            }

            // Check if an order for this product already exists for the current user
            var existingOrder = db.Retailerorders
                                   .Where(r => r.RproIdfk == proid && r.Retaileremail == userEmail)
                                   .OrderByDescending(r => r.OrderDate) // Assuming we have OrderDate to sort by the most recent order
                                   .FirstOrDefault();

            if (existingOrder != null)
            {
                // Check the status of the existing order
                if (existingOrder.Rstatus.Equals("Delivered") || existingOrder.Rstatus.Equals("Approved") || existingOrder.Rstatus.Equals("Rejected"))
                {
                    // Create a new order if the previous order is delivered or approved
                    ro.RproIdfk = proid;
                    ro.Catename = catename;
                    ro.Rproqty = pqty;
                    ro.Rtotalprice = totalprice;
                    ro.Retaileremail = userEmail;
                    ro.Rstatus = "Pending"; // Set the status of the new order to Disapproved initially
                    ro.OrderDate = DateTime.Now; // Make sure you have an OrderDate field to track when orders were placed

                    db.Retailerorders.Add(ro);
                    db.SaveChanges();
                    // Add notification for admin about the new product request
                    var adminNotification = new AdminNotification
                    {
                        Message = $"Retailer ({userEmail}) placed an order for product: {product.ProName}",
                        WholesalerEmail = userEmail,
                        Timestamp = DateTime.Now
                    };

                    db.AdminNotifications.Add(adminNotification);  // Save notification for admin
                    db.SaveChanges();
                    return RedirectToAction("rorderslip");


                }
                else
                {
                    // If the previous order is still Disapproved or Pending, prevent placing a new order
                    ViewBag.msg = "You cannot place a new order for this product as your previous order is still in progress or Pending.";
                    return View(product);
                }
            }
            else
            {
                // If no previous order exists, allow creating a new order
                // Create a new order if the previous order is delivered or approved
                ro.RproIdfk = proid;
                ro.Catename = catename;
                ro.Rproqty = pqty;
                ro.Rtotalprice = totalprice;
                ro.Retaileremail = userEmail;
                ro.Rstatus = "Pending"; // Set the status of the new order to Disapproved initially
                ro.OrderDate = DateTime.Now;

                db.Retailerorders.Add(ro);
                db.SaveChanges();

                // Add notification for admin about the new product request
                var adminNotification = new AdminNotification
                {
                    Message = $"Retailer ({userEmail}) placed an order for product: {product.ProName}",
                    WholesalerEmail = userEmail,
                    Timestamp = DateTime.Now
                };

                db.AdminNotifications.Add(adminNotification);  // Save notification for admin
                db.SaveChanges();
                return RedirectToAction("rorderslip");


            }
        }
        public IActionResult rorderslip()
        {
            // Get the current user's email from session (assuming it was stored when they logged in)
            var userEmail = HttpContext.Session.GetString("useremail");
            var userName = HttpContext.Session.GetString("username");

            if (userEmail == null && userName == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.username = userName;
                ViewBag.useremail = userEmail;
            }
            var notifications = db.Notifications
                                  .OrderByDescending(n => n.Timestamp)
                                  .Take(5) // Example: Get the latest 5 notifications
                                  .ToList();

            // Passing the notifications to the layout page
            ViewBag.Notifications = notifications;

            // Fetch the most recent order with Product and Category details loaded
            var order = db.Retailerorders
               .Include(r => r.RproIdfkNavigation)
               .ThenInclude(p => p.ProcatidFkNavigation)
               .Where(r => r.RproIdfkNavigation != null && r.Rstatus != "Completed")
               .OrderByDescending(r => r.RorderId)
               .FirstOrDefault();

            if (order == null)
            {
                Console.WriteLine("Order not found");
                return NotFound();
            }
            else
            {
                Console.WriteLine("Order found: " + order.RorderId);
            }

            // Pass the order details to the view
            return View(order);
        }

        public IActionResult rpurchaselist()
        {
            ViewBag.rlist = db.Retailerorderlists.ToList();
            var userEmail = HttpContext.Session.GetString("useremail");
            var userName = HttpContext.Session.GetString("username");

            if (userEmail == null && userName == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.username = userName;
                ViewBag.useremail = userEmail;
            }

            return View();
        }
        public IActionResult ShowRetailerOrders()
        {
            var retailerOrders = db.Retailerorderlists.ToList();
            return View(retailerOrders); // No changes here
        }


        public IActionResult Delrorder(int pid)
        {
            var delord = db.Retailerorders.Find(pid);
            db.Retailerorders.Remove(delord);
            db.SaveChanges();
            return RedirectToAction("rpurchaselist");
        }

    }
}
