using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StationaryApp.Data;
using StationaryApp.Models;

using System.Diagnostics;
using System.Security.Cryptography;

namespace StationaryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly StationaryAppContext db;

		public HomeController(ILogger<HomeController> logger, StationaryAppContext db)
		{
			this.db = db;
			_logger = logger;
		}

		public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            var products = db.ProDetails.ToList();
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            return View(products);
        }
        // POST: Clear all Admin notifications
        [HttpPost]
        public IActionResult ClearAdminNoti()
        {
            // Check if the user is an admin by checking the session for the admin's username
            var username = HttpContext.Session.GetString("username");

            // Check if the username is valid and matches the admin credentials
            if (string.IsNullOrEmpty(username) || username != "admin")
            {
                // Handle if the user is not logged in as admin
                return RedirectToAction("Selectrole", "Home");
            }

            // Get all admin notifications and delete them
            var notificationsToDelete = db.AdminNotifications.ToList();

            if (notificationsToDelete.Any())
            {
                db.AdminNotifications.RemoveRange(notificationsToDelete);
                db.SaveChanges();
                Console.WriteLine("Admin notifications cleared successfully.");
            }
            else
            {
                Console.WriteLine("No admin notifications to clear.");
            }

            // After clearing, redirect back to the Index page
            return RedirectToAction("Index");
        }


        //-----------------Login portal work
        public IActionResult Selectrole()
        {
            return View();
        }
        [HttpPost]
        // After role selection, redirect to the respective login page
        public IActionResult Selectrole(string selectedRole)
        {
            if (!string.IsNullOrEmpty(selectedRole))
            {
                // Store the selected role in the session
                HttpContext.Session.SetString("role", selectedRole);

                // Redirect to the corresponding login page based on the role
                if (selectedRole == "Admin")
                {
                    return RedirectToAction("Signin", "Home"); // Admin login page
                }
                else if (selectedRole == "Wholesaler")
                {
                    return RedirectToAction("Signin", "Wholesaler"); // Wholesaler login page
                }
                else if (selectedRole == "Retailer")
                {
                    return RedirectToAction("Signin", "Retailer"); // Retailer login page
                }
            }

            // If no role selected, stay on the same page and show the message
            //ViewBag.msg = "Please select a role!";
            return View();
        }

        //-----------------Signin work
        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signin(string un, string pass)
        {

            // Check if username or password is empty
            if (string.IsNullOrEmpty(un) || string.IsNullOrEmpty(pass))
            {
                // Set message in ViewBag for missing fields
                ViewBag.msg = "Username and Password must be required";
                return View();
            }

            if (un.Equals("admin") && pass.Equals("admin"))
            {
                HttpContext.Session.SetString("username", un);
                return RedirectToAction("Index");
            }
            else
            {
                // If credentials do not match, set error message
                ViewBag.msg = "Wrong Credentials";
                return View();
            }
        }
        //-----------------logout work
        public IActionResult Signout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Selectrole");
        }
        public IActionResult regWS()
        {
            ViewBag.wslist = db.Regwholesalers.ToList();

            if (HttpContext.Session.GetString("username") == null)
			{
				return RedirectToAction("Selectrole", "Home");
			}
			else
			{
				ViewBag.user = HttpContext.Session.GetString("username");
			}

            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;

            return View();
		}
        [HttpGet]
        public IActionResult upws(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }

            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;

            // Fetch the wholesaler from the database using the provided id
            var wholesaler = db.Regwholesalers.Find(id);

            if (wholesaler == null)
            {
                // Handle the case where the wholesaler is not found
                return NotFound();
            }

            // Pass the wholesaler object to the view
            return View(wholesaler);
        }
       
        [HttpPost]
        public IActionResult upws(int id, Regwholesaler upd)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;

            // Find the wholesaler in the database
            var wholesaler = db.Regwholesalers.Find(id);

            if (wholesaler != null)
            {
                // Update the status based on the form submission
                wholesaler.UserStatus = upd.UserStatus;
                db.SaveChanges();  // Save changes to the database
            }

            // Redirect to another action or return the updated view
            return RedirectToAction("regWS");
        }
        //delete wholersaler
        public IActionResult delws(int id)
        {
            // Wholesaler ko find karna
            var wholesaler = db.Regwholesalers.FirstOrDefault(w => w.UserId == id);
            if (wholesaler != null)
            {
                // Wholesaler ko delete karna
                db.Regwholesalers.Remove(wholesaler);
                db.SaveChanges(); // Changes ko save karna
            }

            // Wholesalers list page par redirect karna
            return RedirectToAction("regWS");
        }
        //add category----
        [HttpGet]
        public IActionResult addcategory()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }

            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            return View();

        }
        [HttpPost]
        public IActionResult addcategory(Addcategory cat)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            db.Addcategories.Add(cat);
            db.SaveChanges();
            return RedirectToAction("showcategory");
        }
        public IActionResult showcategory()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            var cat_list = db.Addcategories.ToList();
            return View(cat_list);
        }
        public IActionResult DelCat(int did)
        {
            var delid = db.Addcategories.Find(did);
            db.Addcategories.Remove(delid);
            db.SaveChanges();
            return RedirectToAction("showcategory");
            
        }
        [HttpGet]
        public IActionResult UpdCat(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            var show = db.Addcategories.FirstOrDefault(option => option.CatId == id);
            return View(show);
        }
        [HttpPost]
        public IActionResult UpdCat(int id, Addcategory cat)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }

            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            var show = db.Addcategories.Find(id);

            if (show != null)
            {
                show.CatName = cat.CatName;
                show.CatDesc = cat.CatDesc;
                //show.ProPrice = pro.ProPrice;
                //show.ProCatidfk = pro.ProCatidfk;
                db.SaveChanges();
                return RedirectToAction("showcategory");
            }
            return View();
        }

        public IActionResult addpro()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }

            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            ViewBag.catdata = db.Addcategories.ToList();
            //db.SaveChanges();
            return View();
        }
        [HttpPost]
        public IActionResult addpro(Product pro, IFormFile ProImg)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }

            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            ViewBag.catdata = db.Addcategories.ToList();

            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upl_images");
            string imgpath = Path.Combine(folder, ProImg.FileName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var strem = new FileStream(imgpath, FileMode.Create);

            ProImg.CopyTo(strem);

            string? imgname = ProImg.FileName;
            pro.ProImg = imgname;

            //--------now inserting data into db
            db.Entry(pro).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("showpro");
        }

        public IActionResult showpro()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            ViewBag.pro = db.ProDetails.ToList();
            return View();
        }

        //delete product------------
        public IActionResult DelPro(int pid)
        {
            var delpro = db.Products.Find(pid);
            db.Products.Remove(delpro);
            db.SaveChanges();
            return RedirectToAction("showpro");
        }

        //Update product---------------

        [HttpGet]
        public IActionResult uppro(int id)
        {

            ViewBag.catdata = db.Addcategories.ToList();
            var show = db.Products.FirstOrDefault(option => option.ProId == id);
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            return View(show);
        }

        [HttpPost]
        public IActionResult uppro(int id, Product pro, IFormFile ProImg)
        {

            ViewBag.catdata = db.Addcategories.ToList();
            //---------old
            var show = db.Products.Find(id);

            if (show != null)
            {
                show.ProName = pro.ProName;
                show.ProDesc = pro.ProDesc;
                show.ProPrice = pro.ProPrice;
                show.ProcatidFk = pro.ProcatidFk;
                show.Availability = pro.Availability;

                // Handle the image upload
                if (ProImg != null && ProImg.Length > 0)
                {
                    // Define a path to store the image
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upl_images", ProImg.FileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ProImg.CopyTo(stream);
                    }

                    // Update the image path in the database (assuming ProImg is a string that stores the file path)
                    show.ProImg = "/" + ProImg.FileName;
                }

                db.SaveChanges();
                return RedirectToAction("showpro");
            }



            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            return NotFound();
        }
        public IActionResult orderlist()
        {

            ViewBag.wolist = db.Whsalerorderlists.ToList();

            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult upwstatus(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }
            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;
            // Fetch the wholesaler from the database using the provided id
            var wholesaler = db.Whorders.Find(id);

            if (wholesaler == null)
            {
                // Handle the case where the wholesaler is not found
                return NotFound();
            }

            // Pass the wholesaler object to the view
            return View(wholesaler);
        }
   
        [HttpPost]
        public IActionResult upwstatus(int id, Whorder upd)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Selectrole", "Home");
            }
            else
            {
                ViewBag.user = HttpContext.Session.GetString("username");
            }

            // Fetch the latest admin notifications
            var adminNotifications = db.AdminNotifications
                                       .OrderByDescending(n => n.Timestamp)
                                       .Take(5) // Get the latest 5 notifications
                                       .ToList();

            // Pass notifications to the view
            ViewBag.AdminNotifications = adminNotifications;

            // Find the wholesaler in the database, including the related Product
            var wholesaler = db.Whorders
                                .Include(w => w.WproIdfkNavigation)  // Include the related Product information
                                .FirstOrDefault(w => w.WorderId == id);

            if (wholesaler != null)
            {
                // Update the status based on the form submission
                wholesaler.Wstatus = upd.Wstatus;
                db.SaveChanges();  // Save changes to the database

                // Use the product name (ProName) in the message, or "product information is not available" if null
                string message = $"Your request for this {(wholesaler.WproIdfkNavigation?.ProName ?? "product information is not available")} has been {wholesaler.Wstatus} by the admin.";

                // Create the notification
                var notification = new Notification
                {
                    WholesalerEmail = wholesaler.Whsaleremail,  // Using the email from Whorder
                    Message = message,
                    Timestamp = DateTime.Now
                };

                // Save the notification to the database
                db.Notifications.Add(notification);
                db.SaveChanges();
        }

            // Redirect to another action or return the updated view
            return RedirectToAction("orderlist");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}