using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DsiSporKayitSistemi.Models;
using System.Data.Entity;
using DsiSporKayitSistemi.Models.ViewModel;
using System.Net;
using Newtonsoft.Json;

namespace DsiSporKayitSistemi.Controllers
{
    public class HomeController : Controller
    {
        private SporKulubuDBEntities1 db = new SporKulubuDBEntities1();
        
        
        public ActionResult Index()
        {
            int toplamKursSayisi = db.SporDali.Count();
            ViewBag.toplamKursSayisi = toplamKursSayisi;

            int kayitliOgrenciSayisi = db.Ogrenciler.Count();
            ViewBag.kayitliOgrenciSayisi = kayitliOgrenciSayisi;

            int antrenorSayisi = db.Antrenorler.Count();
            ViewBag.antrenorSayisi = antrenorSayisi;

            int devamEdenKursSayisi = db.KurKayit.Count(m => m.BitisTatihi > DateTime.Now);
            ViewBag.DevamEdenKurs = devamEdenKursSayisi;

            return View();
        }
        public ActionResult SporcuRegister()
        {
            OgrenciVeKayitVM ogrencivekayitvm = new OgrenciVeKayitVM(); //view model nesnesi oluşturdu

            ogrencivekayitvm.kayit = new Kayit(); //kayıt nesnesi
            ogrencivekayitvm.ogrenciler = new Ogrenciler(); //öğrenci nesnesi





            List<SelectListItem> sporDali = (from i in db.SporDali
                                             select new SelectListItem
                                             {
                                                 Text = i.SporAdi,
                                                 Value = i.SporID.ToString()
                                             }).ToList();
            ViewBag.Brans = sporDali;

            return View(ogrencivekayitvm);  //view'a modeli gönderdik 
        }

        public ActionResult SporcuListesi()

        {
            //var antrenorListesi = db.Antrenorler.Include(x => x.SporDali).ToList();
            var sporcuListesi = db.View_Sporcu_info.ToList();

            return View(sporcuListesi);
        }

        public ActionResult AntrenorRegister() {
            List<SelectListItem> sporDali = (from i in db.SporDali
                                             select new SelectListItem
                                             {
                                                 Text = i.SporAdi,
                                                 Value = i.SporID.ToString()
                                             }).ToList();
            ViewBag.Brans = sporDali;
            return View();
        }

        public ActionResult AntrenorListesi() {

            var antrenorListesi = db.Antrenorler.Include(x => x.SporDali).ToList(); //Eager loading, SQL Server'a 1 kere sorgu gönderecek. Lazy loadingte antrenör başına hepsine sorgu gönderecek.
            return View(antrenorListesi);
        }

        public ActionResult SporRegister()
        {
            return View();
        }

        public ActionResult SporListesi()
        {
            var sporDallari = db.SporDali.ToList();
            return View(sporDallari);
        }

        public ActionResult EtkinlikEkle()
        {
            return View();
        }

        public ActionResult EtkinlikListele()
        {
            return View();
        }

        public ActionResult KursListesi()

        {
            var kurslistesi = db.KurKayit.Include(x => x.Antrenorler).ToList();
            return View(kurslistesi);

        }

        public ActionResult AntrenorUpdateForm(int id)

        {
            List<SelectListItem> sporDali = (from i in db.SporDali
                                             select new SelectListItem
                                             {
                                                 Text = i.SporAdi,
                                                 Value = i.SporID.ToString()
                                             }).ToList();
            ViewBag.Brans = sporDali;

            var duzenlenecekAntrenor = db.Antrenorler.FirstOrDefault(m => m.AntrenorID == id);
            return View(duzenlenecekAntrenor);
        }
        
        public ActionResult SporDaliUpdateForm(int id)
        {
            var duzenlenecekSpor = db.SporDali.FirstOrDefault(m => m.SporID == id);
            return View(duzenlenecekSpor);
        }
        public ActionResult OgrenciUpdateForm(int id)
        {
            var duzenlenecekOgrenci = db.Ogrenciler.FirstOrDefault(m => m.OgrenciID == id);
            var duzenlenecekKayit = db.Kayit.FirstOrDefault(m => m.OgrenciID == id);

            OgrenciVeKayitVM ogrencivekayit = new OgrenciVeKayitVM(); //view model nesnesi
            ogrencivekayit.ogrenciler = duzenlenecekOgrenci; 
            ogrencivekayit.kayit = duzenlenecekKayit;

           
            List<SelectListItem> sporDali = (from i in db.SporDali
                                             select new SelectListItem
                                             {
                                                 Text = i.SporAdi,
                                                 Value = i.SporID.ToString()
                                             }).ToList();
            ViewBag.Brans = sporDali;

            return View(ogrencivekayit);
        }
        public ActionResult KursUpdateForm(int id)

        {
            List<SelectListItem> SporDallari = (from i in db.SporDali
                                                select new SelectListItem
                                                {
                                                    Text = i.SporAdi,
                                                    Value = i.SporID.ToString()
                                                }).ToList();

            ViewBag.sporDallari = SporDallari;



            List<SelectListItem> Donemler = (from i in db.Donem
                                             select new SelectListItem
                                             {
                                                 Text = i.DonemAdi,
                                                 Value = i.DonemID.ToString()
                                             }).ToList();
            ViewBag.DonemSecim = Donemler;

            var duzenlenecekKurs = db.KurKayit.FirstOrDefault(m => m.KurID == id);

            var antrenorList = db.Antrenorler.Where(m => m.BransID == duzenlenecekKurs.SporDaliID ).ToList();
            List<SelectListItem> Antrenorler = antrenorList.Select(i => new SelectListItem
            {
                Text = $"{i.AntrenorAdi} {i.AntrenorSoyadi}",
                Value = i.AntrenorID.ToString()
            }).ToList();
            
            ViewBag.Antrenor = Antrenorler;

            
            return View(duzenlenecekKurs);


        }

        public ActionResult KursSec(int sp_id, int kyt_id)
        {
            var kursList = db.KurKayit
                .Where(x => x.SporDaliID == sp_id && x.BitisTatihi > DateTime.Now)
                .ToList();
            ViewBag.KayitID = kyt_id;
            ViewBag.SporID = sp_id;

            return View(kursList);
        }


        public ActionResult OgrenciKurKayitAdd(int KayitID, int KurID)
        {
            
            var kayit = db.Kayit.FirstOrDefault(x => x.KayitID == KayitID);
            kayit.KursID = KurID;
            db.SaveChanges();
            return RedirectToAction("SporcuListesi", "Home");
        }
        public ActionResult EkKursAdd()
        {
            //öğrenci listesini getir
            var sporcuListesi = db.View_Sporcu_info.ToList();


            return View(sporcuListesi); 
        }

        public ActionResult OgrenciEkKursSec(int OgrenciID)
        {

            return RedirectToAction("EkKursAdd2", new {  ogrenciID = OgrenciID });
        }
        [HttpPost]
        public JsonResult GetKurslar(int sporDaliID)
        {
            var kurslar = db.KurKayit.Where(k => k.SporDaliID == sporDaliID).ToList();

            return Json(kurslar, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EkKursAdd2( int ogrenciID)
        {
            
            var sporDallari = db.SporDali.ToList();
            var kurslar = db.KurKayit.ToList();

            ViewBag.OgrenciID = ogrenciID;
            ViewBag.SporDallari = sporDallari;
            ViewBag.Kurslar = kurslar;  

            return View();
        }
        [HttpPost]
        public ActionResult EkKursKayitSon(int ogrenciID, int sporDali, int kursDali)
        {
            var kayit = db.Kayit.FirstOrDefault(x => x.OgrenciID == ogrenciID && x.SporID == sporDali);
            if (kayit != null)
            {
                kayit.KursID = kursDali;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }


    }

}