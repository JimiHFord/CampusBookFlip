using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.Domain.Entities;
using FileHelpers;
using Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using WebMatrix.WebData;

namespace CampusBookFlip.WebUI.Infrastructure
{
    public class Constants
    {
        public static string DEFAULT_PASSWORD { get { return "password"; } }
        public const string ADMIN = "Administrator";
        public static string jimi { get { return "jford"; } }
        public static string wes { get { return "wjones"; } }
        public static string EMAIL_NO_REPLY { get { return "no-reply@campusbookflip.com"; } }
        public static string COMPLETE_REGISTRATION_PROCESS { get { return "Complete Registration Process"; } }
        public static string CHANGE_EMAIL { get { return "Change Email"; } }
        public static string FORGOT_PASSWORD { get { return "Password Reset"; } }

        public static bool MakeGoogleRequest
        {
            get
            {
                var val = WebConfigurationManager.AppSettings["MakeGoogleRequest"];
                if (val == null) { val = ""; }
                return val.ToLower().Equals("true");
            }
        }

        public static bool GrowDB
        {
            get
            {
                var val = WebConfigurationManager.AppSettings["GrowDB"];
                if (val == null) { val = ""; }
                return val.ToLower().Equals("true");
            }
        }

        public static string InstitutionFile
        {
            get
            {
                var val = WebConfigurationManager.AppSettings["InstitutionFile"];
                if (val == null) { val = ""; }
                return val;
            }
        }

        public static string CampusFile
        {
            get
            {
                var val = WebConfigurationManager.AppSettings["CampusFile"];
                if (val == null) { val = ""; }
                return val;
            }
        }

        public static int ParticipatingCollegesItemsPerPage
        {
            get
            {
                int def = 25;
                string val = WebConfigurationManager.AppSettings["ParticipatingCollegesItemsPerPage"];
                if (val == null)
                {
                    val = def.ToString();
                }
                try { def = int.Parse(val); }
                catch { }
                return def <= 0 ? 25 : def;
            }
        }

        public static void UpdateCollegeData(IRepository repo = null)
        {
            if (repo == null) { repo = new CampusBookFlip.Domain.Concrete.EFRepository(); }
            string campus_path = VirtualPathUtility.ToAbsolute(CampusFile);
            string institution_path = VirtualPathUtility.ToAbsolute(InstitutionFile);
            FileHelperEngine institution_engine = new FileHelperEngine(typeof(CampusBookFlip.Domain.Entities.InstitutionFile));
            FileHelperEngine campus_engine = new FileHelperEngine(typeof(CampusBookFlip.Domain.Entities.CampusFile));
            IEnumerable<CampusBookFlip.Domain.Entities.CampusFile> campusList = campus_engine.ReadFile(campus_path) as IEnumerable<CampusBookFlip.Domain.Entities.CampusFile>;
            IEnumerable<CampusBookFlip.Domain.Entities.InstitutionFile> institutionList = institution_engine.ReadFile(institution_path) as IEnumerable<CampusBookFlip.Domain.Entities.InstitutionFile>;
            ICollection<int> visited_institutions = new List<int>();
            ICollection<Pair> visited_campus = new List<Pair>();
            foreach (var i in institutionList)
            {
                if (!visited_institutions.Contains(i.Id))
                {
                    visited_institutions.Add(i.Id);
                    repo.SaveInstitution(new Institution
                    {
                        Address = i.Address.Replace("\"", "").Replace("\\", ""),
                        Activated = false,
                        WebAddress = i.WebAddress.Replace("\"", "").Replace("\\", ""),
                        Zip = i.Zip.Replace("\"", "").Replace("\\", ""),
                        State = i.State.Replace("\"", "").Replace("\\", ""),
                        Phone = i.Phone.Replace("\"", "").Replace("\\", ""),
                        Name = i.Name.Replace("\"", "").Replace("\\", ""),
                        InstitutionId = i.Id,
                        City = i.City.Replace("\"", "").Replace("\\", ""),
                    });
                }
            }

            foreach (var c in campusList)
            {
                if (!visited_campus.Contains(new Pair(c.InstitutionId, c.Name)))
                {
                    visited_campus.Add(new Pair(c.InstitutionId, c.Name));
                    int inst = repo.XInstitution.FirstOrDefault(i => i.InstitutionId == c.InstitutionId).Id;
                    repo.SaveCampus(new Campus
                    {
                        Activated = false,
                        Address = c.Address.Replace("\"", "").Replace("\\", ""),
                        City = c.City.Replace("\"", "").Replace("\\", ""),
                        ZipCode = c.ZipCode.Replace("\"", "").Replace("\\", ""),
                        Name = c.Name.Replace("\"", "").Replace("\\", ""),
                        State = c.State.Replace("\"", "").Replace("\\", ""),
                        InstitutionId = inst,
                    });
                }
            }

        }

        public static void FixPublishers(IRepository repo)
        {
            var all_ids = repo.Publisher.Select(a => a.Id).ToList();
            for (int all_i = 0; all_i < all_ids.Count(); all_i++)
            {
                int current_publisher_id = all_ids.FirstOrDefault();
                var current_publisher = repo.Publisher.FirstOrDefault(p => p.Id == current_publisher_id);
                all_ids.RemoveAt(0);
                if (current_publisher != null)
                {
                    var duplicates = repo.Publisher.Where(p => p.Name == current_publisher.Name && p.Id != current_publisher.Id);

                    var book_ids = repo.Book.Where(b => duplicates.Select(p => p.Id).Contains(b.PublisherId)).Select(b => b.Id).ToList();
                    for (int book_i = 0; book_i < book_ids.Count(); book_i++)
                    {
                        int current_book_id = book_ids.FirstOrDefault();
                        var current_book = repo.Book.FirstOrDefault(b => b.Id == current_book_id);
                        book_ids.RemoveAt(0);
                        current_book.PublisherId = current_publisher.Id;
                        repo.SaveBook(current_book);
                    }
                    while (repo.Publisher.Where(p => p.Name == current_publisher.Name && p.Id != current_publisher.Id).Count() > 0)
                    {
                        repo.DeletePublisher(repo.Publisher.Where(p => p.Name == current_publisher.Name && p.Id != current_publisher.Id).FirstOrDefault().Id);
                    }
                }
            }

        }

        internal static void InitializeWebSecurity()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("EFDbContext", "CBFUser", "Id", "AppUserName", autoCreateTables: true);
            }
        }

        internal static void SeedAdmins()
        {
            Constants.InitializeWebSecurity();

            if (!Roles.RoleExists(Constants.ADMIN))
            {
                Roles.CreateRole(Constants.ADMIN);
            }

            if (!WebSecurity.UserExists(Constants.jimi))
            {
                WebSecurity.CreateUserAndAccount(Constants.jimi,
                    Constants.DEFAULT_PASSWORD, new
                    {
                        FirstName = "Jimi",
                        LastName = "Ford",
                        Paid = false
                    });
            }

            if (!WebSecurity.UserExists(Constants.wes))
            {
                WebSecurity.CreateUserAndAccount(Constants.wes,
                    Constants.DEFAULT_PASSWORD, new
                    {
                        FirstName = "Wes",
                        LastName = "Jones",
                        Paid = false
                    });
            }

            if (!Roles.IsUserInRole(Constants.jimi, Constants.ADMIN))
            {
                Roles.AddUserToRole(Constants.jimi, Constants.ADMIN);
            }

            if (!Roles.IsUserInRole(Constants.wes, Constants.ADMIN))
            {
                Roles.AddUserToRole(Constants.wes, Constants.ADMIN);
            }
        }
    }

}