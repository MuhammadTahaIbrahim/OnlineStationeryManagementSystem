using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StationaryApp.Data;
using StationaryApp.Models;

namespace StationaryApp.Controllers
{
    public class WholesalerController : Controller
    {
        private readonly ILogger<WholesalerController> _logger;
        private readonly StationaryAppContext db;

        public WholesalerController(ILogger<WholesalerController> logger, StationaryAppContext db)
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
                ViewBag.username =  userName;
                ViewBag.useremail = userEmail;
            }

            var products = db.ProDetails.ToList();
            var notifications = db.Notifications
                                    .OrderByDescending(n => n.Timestamp)
                                    .Take(5) // Example: Get the latest 5 notifications
                                    .ToList();

            // Passing the notifications to the layout page
            ViewBag.Notifications = notifications;
            return View(products);
        }

        // POST: Clear All notifications
        [HttpPost]
        public IActionResult ClearNoti()
        {
            var userEmail = HttpContext.Session.GetString("useremail");

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Delete all notifications for the user
                var notificationsToDelete = db.Notifications.Where(n => n.WholesalerEmail == userEmail).ToList();
                db.Notifications.RemoveRange(notificationsToDelete);
                db.SaveChanges();
            }

            // After clearing, redirect back to the notifications page (Index)
            return RedirectToAction("Index");
        }
    
    ////------------login portal work
    //public IActionResult Selectrole()
    //{
    //    return View();
    //}
    //[HttpPost]
    //public IActionResult Selectrole(string selectedRole)
    //{
    //    if (!string.IsNullOrEmpty(selectedRole))
    //    {
    //        // Store the selected role in the session
    //        HttpContext.Session.SetString("role", selectedRole);

    //        // Redirect to the corresponding login page based on the role
    //        if (selectedRole == "Admin")
    //        {
    //            return RedirectToAction("Signin", "Home"); // Admin login page
    //        }
    //        else if (selectedRole == "Wholesaler")
    //        {
    //            return RedirectToAction("Signin", "Wholesaler"); // Wholesaler login page
    //        }
    //        else if (selectedRole == "Retailer")
    //        {
    //            return RedirectToAction("Signin", "Retailer"); // Retailer login page
    //        }
    //    }

    //    // If no role selected, stay on the same page and show the message
    //    ViewBag.msg = "Please select a role!";
    //    return View();
    //}

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
            var wholesaler = db.Regwholesalers
                .FirstOrDefault(w => w.UserName == userName && w.UserPass == userPass);

           // var existingWholesaler = db.Regwholesalers.FirstOrDefault(w => w.UserName == ru.UserName);

            if (wholesaler != null)
            {
                if (wholesaler.UserStatus == "Deactive")
                {
                    // Account is deactivated, show the deactivation message
                    ViewBag.msg1 = "Your account has been deactivated by the admin. Please contact admin.";
                    return View(); // Return the signup view with the message
                }
                else
                {
                    HttpContext.Session.SetString("username", wholesaler.UserName);
                    HttpContext.Session.SetString("useremail", wholesaler.UserEmail);
                    
                    return RedirectToAction("Index", "Wholesaler");
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
        public IActionResult Signup(Regwholesaler ru)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                ViewBag.msg1 = "All fields are required and must be valid.";
                return View(ru);
            }

            // Check if the email already exists in the database
            var existingWholesaler = db.Regwholesalers.Any(w => w.UserEmail == ru.UserEmail);
            if (existingWholesaler)
            {
                // If the email already exists, add an error message and return the view with the model
                ModelState.AddModelError("useremail", "This email has already been taken.");

                return View(ru);  // Return the same view to show the error message
            }

            // If email is unique, save the wholesaler details to the database
            db.Regwholesalers.Add(ru);
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
            var wholesaler = db.Regwholesalers.FirstOrDefault(w => w.UserEmail == userEmail && w.UserName == userName);

            // If wholesaler is found, pass it to the view
            if (wholesaler != null)
            {
                ViewBag.username = wholesaler.UserName;
                ViewBag.useremail = wholesaler.UserEmail;
                return View(wholesaler);  // Pass the specific wholesaler object to the view
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
            var wholesaler = db.Regwholesalers.FirstOrDefault(w => w.UserEmail == userEmail && w.UserName == userName);

            if (wholesaler != null)
            {
                // If a new password is provided, update it
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    wholesaler.UserPass = NewPassword;  // In a real app, hash the password before saving
                    db.SaveChanges();
                    TempData["msg1"] = "Password updated successfully.";
                }

                // Fetch the updated wholesaler from the database to pass to the view
                var updatedWholesaler = db.Regwholesalers.FirstOrDefault(w => w.UserEmail == userEmail && w.UserName == userName);
                
                // Pass the updated wholesaler to the view
                return RedirectToAction("Profile", new { userEmail = updatedWholesaler.UserEmail, userName = updatedWholesaler.UserName });
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
            var wholesaler = db.Regwholesalers.FirstOrDefault(w => w.UserEmail == email);

            if (wholesaler != null)
            {
                // If the email is valid, display the password in a popup
                TempData["PasswordPopup"] = $"Your password is: {wholesaler.UserPass}";


            }
            else
            {
                // If the email is incorrect, show an error message
                TempData["Error"] = "Your email is incorrect, unable to retrieve your password.";
            }

            return RedirectToAction("FPassword");
        }
       
        public IActionResult wsaleslist()
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
            ViewBag.pro = db.ProDetails.ToList();
            return View();
        }
        public IActionResult wshowsaleslist(int id)
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
            var orderDetails = db.Whorders
                                     .Where(wo => wo.WproIdfk == id)
                                     .Select(wo => new { wo.OrderDate })
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
        public IActionResult wshowsaleslist(int proid, int pqty, string totalprice, string catename, Whorder wo)
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
            var existingOrder = db.Whorders
                                   .Where(w => w.WproIdfk == proid && w.Whsaleremail == userEmail)
                                   .OrderByDescending(w => w.OrderDate) // Assuming we have OrderDate to sort by the most recent order
                                   .FirstOrDefault();

            if (existingOrder != null)
            {
                // Check the status of the existing order
                if (existingOrder.Wstatus.Equals("Delivered") || existingOrder.Wstatus.Equals("Approved") || existingOrder.Wstatus.Equals("Rejected"))
                {
                    // Create a new order if the previous order is delivered or approved
                    wo.WproIdfk = proid;
                    wo.Catename = catename;
                    wo.Wproqty = pqty;
                    wo.Wtotalprice = totalprice;
                    wo.Whsaleremail = userEmail;
                    wo.Wstatus = "Pending"; // Set the status of the new order to Disapproved initially
                    wo.OrderDate = DateTime.Now; // Make sure you have an OrderDate field to track when orders were placed

                    db.Whorders.Add(wo);
                    db.SaveChanges();
                    // Add notification for admin about the new product request
                    var adminNotification = new AdminNotification
                    {
                        Message = $"Wholesaler ({userEmail}) placed an order for product: {product.ProName}",
                        WholesalerEmail = userEmail,
                        Timestamp = DateTime.Now
                    };

                    db.AdminNotifications.Add(adminNotification);  // Save notification for admin
                    db.SaveChanges();
                    return RedirectToAction("worderslip");

                   
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
                wo.WproIdfk = proid;
                wo.Catename = catename;
                wo.Wproqty = pqty;
                wo.Wtotalprice = totalprice;
                wo.Whsaleremail = userEmail;
                wo.Wstatus = "Pending"; // Default status for new orders
                wo.OrderDate = DateTime.Now;

                db.Whorders.Add(wo);
                db.SaveChanges();

                // Add notification for admin about the new product request
                var adminNotification = new AdminNotification
                {
                    Message = $"Wholesaler ({userEmail}) placed an order for product: {product.ProName}",
                    WholesalerEmail = userEmail,
                    Timestamp = DateTime.Now
                };

                db.AdminNotifications.Add(adminNotification);  // Save notification for admin
                db.SaveChanges();
                return RedirectToAction("worderslip");

               
            }
        }
            public IActionResult worderslip()
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
            var order = db.Whorders
               .Include(w => w.WproIdfkNavigation)
               .ThenInclude(p => p.ProcatidFkNavigation)
               .Where(w => w.WproIdfkNavigation != null && w.Wstatus != "Completed")
               .OrderByDescending(w => w.WorderId)
               .FirstOrDefault();

            if (order == null)
            {
                Console.WriteLine("Order not found");
                return NotFound();
            }
            else
            {
                Console.WriteLine("Order found: " + order.WorderId);
            }

            // Pass the order details to the view
            return View(order);
        }
        public IActionResult wpurchaselist()
        {
            ViewBag.wolist = db.Whsalerorderlists.ToList();
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
            return View();
        }

        public IActionResult Delorder(int pid)
        {
            var delord = db.Whorders.Find(pid);
            db.Whorders.Remove(delord);
            db.SaveChanges();
            return RedirectToAction("wpurchaselist");
        }

        public IActionResult retailerorderlist()
        {

            ViewBag.rolist = db.Retailerorderlists.ToList();

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
            return View();
        }

        public IActionResult uprstatus(int id)
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
            var retailer = db.Retailerorders.Find(id);

            if (retailer == null)
            {
                // Handle the case where the wholesaler is not found
                return NotFound();
            }

            // Pass the wholesaler object to the view
            return View(retailer);
        }

        [HttpPost]
        public IActionResult uprstatus(int id, Retailerorder upd)
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

            // Find the wholesaler in the database, including the related Product
            var retailer = db.Retailerorders
                                .Include(r => r.RproIdfkNavigation)  // Include the related Product information
                                .FirstOrDefault(r => r.RorderId == id);

            if (retailer != null)
            {
                // Update the status based on the form submission
                retailer.Rstatus = upd.Rstatus;
                db.SaveChanges();  // Save changes to the database

                // Use the product name (ProName) in the message, or default text if not available
                //string message = $"Your request for this {(retailer.RproIdfkNavigation?.ProName ?? "product information is not available")} has been {retailer.Rstatus} by the manager.";

                //// Create a new notification object for the retailer
                //var notification = new RetailerNotification
                //{
                //    RetailerEmail = retailer.Retaileremail,  // Using the retailer's email
                //    Message = message,
                //    Timestamp = DateTime.Now,
                //    IsRead = false  // New notifications are unread by default
                //};

                // Save the notification to the RetailerNotifications table
                //db.RetailerNotifications.Add(notification);
                db.SaveChanges();
            }

            // Redirect to another action or return the updated view
            return RedirectToAction("retailerorderlist");
        }

        public IActionResult regrs()
        {
            ViewBag.wslist = db.Regretailers.ToList();

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


            return View();
        }
        [HttpGet]
        public IActionResult uprs(int id)
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

            // Fetch the retailer from the database using the provided id
            var retailer = db.Regretailers.Find(id);

            if (retailer == null)
            {
                // Handle the case where the retailer is not found
                return NotFound();
            }

            // Pass the retailer object to the view
            return View(retailer);
        }

        [HttpPost]
        public IActionResult uprs(int id, Regretailer upd)
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

            // Find the wholesaler in the database
            var retailer = db.Regretailers.Find(id);

            if (retailer != null)
            {
                // Update the status based on the form submission
                retailer.UserStatus = upd.UserStatus;
                db.SaveChanges();  // Save changes to the database
            }

            // Redirect to another action or return the updated view
            return RedirectToAction("regrs");
        }
        //delete wholersaler
        public IActionResult delrs(int id)
        {
            // Wholesaler ko find karna
            var retailer = db.Regretailers.FirstOrDefault(w => w.UserId == id);
            if (retailer != null)
            {
                // Wholesaler ko delete karna
                db.Regretailers.Remove(retailer);
                db.SaveChanges(); // Changes ko save karna
            }

            // Wholesalers list page par redirect karna
            return RedirectToAction("regrs");
        }

    }
}
