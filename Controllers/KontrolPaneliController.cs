using DsiSporKayitSistemi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DsiSporKayitSistemi.Controllers
{
    public class KontrolPaneliController : Controller
    {
        SporKulubuDBEntities1 db = new SporKulubuDBEntities1();
        

        // GET: KontrolPaneli
        public ActionResult Index()
        {
            

            return View();
        }
        
        public ActionResult AntrenorEkle(Antrenorler antrenor)
        {
            db.Antrenorler.Add(antrenor);
            db.SaveChanges();
            return RedirectToAction("AntrenorListesi", "Home");

        }


        public ActionResult SporDaliEkle(SporDali spor_ismi)
        {
            db.SporDali.Add(spor_ismi);
            db.SaveChanges();
            return RedirectToAction("SporListesi","Home");
        }

        public ActionResult OgrenciEkle(Ogrenciler ogrenciler, Kayit kayit)
        {
           
            db.Ogrenciler.Add(ogrenciler);
            db.SaveChanges(); 

           
            kayit.OgrenciID = ogrenciler.OgrenciID;

         
            db.Kayit.Add(kayit);
            db.SaveChanges(); 

            int sporId = kayit.SporID.Value;
            ViewBag.KayitID = kayit.KayitID;

            return RedirectToAction("KursSec", "Home", new { sp_id = sporId, kyt_id = kayit.KayitID });
        }



        public ActionResult KursSil(int id)
        {
            var silinecekKurs = db.KurKayit.FirstOrDefault(m => m.KurID == id);
            db.KurKayit.Remove(silinecekKurs);
            db.SaveChanges();
            return RedirectToAction("KursListesi", "Home");

        }
        public ActionResult OgrenciSil(int id)
        {
            var silinecekOgrenci = db.Ogrenciler.FirstOrDefault(m => m.OgrenciID == id);
            db.Ogrenciler.Remove(silinecekOgrenci);
            db.SaveChanges();
            return RedirectToAction("SporcuListesi", "Home");
        }

        public ActionResult AntrenorSil(int id) {
            var silinecekAntrenor = db.Antrenorler.FirstOrDefault(m => m.AntrenorID == id);
            db.Antrenorler.Remove(silinecekAntrenor);
            db.SaveChanges();
            return RedirectToAction("AntrenorListesi", "Home");
        }
        public ActionResult SporDaliSil(int id) { 
            var silinecekSporDali = db.SporDali.FirstOrDefault(m => m.SporID == id);
            db.SporDali.Remove(silinecekSporDali);
            db.SaveChanges();
            return RedirectToAction("SporListesi", "Home");
        }
        public ActionResult AntrenorDuzenle(Antrenorler antrenor)
        {
            db.Entry(antrenor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AntrenorListesi","Home");



        }
        public ActionResult SporDaliDuzenle(SporDali spordali)
        {
            db.Entry(spordali).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("SporListesi", "Home");
        }

        public ActionResult OgrenciDuzenle(Ogrenciler ogrenciler,Kayit kayit)
        {
            db.Entry(ogrenciler).State = System.Data.Entity.EntityState.Modified;
            db.Entry(kayit).State = System.Data.Entity.EntityState.Modified;
            kayit.OgrenciID = ogrenciler.OgrenciID;
            db.SaveChanges();
            return RedirectToAction("SporcuListesi", "Home");

        }
        public ActionResult KursDuzenle(KurKayit kurkayit)
        {
            db.Entry(kurkayit).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            //int SonKurId = kurkayit.KurID;
            return RedirectToAction("KursListesi", "Home");

            
        }
       




    }
}