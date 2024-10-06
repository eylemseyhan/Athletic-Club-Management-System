using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DsiSporKayitSistemi.Models;

namespace DsiSporKayitSistemi.Controllers
{
    public class OgrenciDetayController : Controller
    {
        private SporKulubuDBEntities1 db = new SporKulubuDBEntities1();
        // GET: OgrenciDetay
        public ActionResult Bilgi(int id)
        {
            var ogrenciDetay = db.Ogrenciler.Find(id);
            

            return View(ogrenciDetay);
        }
    }
}