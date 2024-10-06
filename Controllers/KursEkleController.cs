using DsiSporKayitSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DsiSporKayitSistemi.Controllers
{
    public class KursEkleController : Controller
    {
        
        private SporKulubuDBEntities1 db = new SporKulubuDBEntities1();
        // GET: KursEkle
        public ActionResult Step1()
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



            
            return View();
        }

        public ActionResult KursRegister(KurKayit kurkayit)
        {

            db.KurKayit.Add(kurkayit);
            db.SaveChanges();
            int SonKurId = kurkayit.KurID;
            
            
            
            return RedirectToAction("Step2", "KursEkle", new {id = SonKurId});
        }
        public ActionResult Step2(int id)
        {
            var sonKur = db.KurKayit.FirstOrDefault(m => m.KurID == id);

            var antrenorList = db.Antrenorler.Where(m=> m.BransID == sonKur.SporDaliID).ToList();

            List<SelectListItem> Antrenorler = antrenorList.Select(i => new SelectListItem
            {
                Text = $"{i.AntrenorAdi} {i.AntrenorSoyadi}",
                Value = i.AntrenorID.ToString()
            }).ToList();
            ViewBag.SonKur = id;
            ViewBag.Antrenor = Antrenorler;
            return View(sonKur);
        }

        public ActionResult KursRegister2(KurKayit kurkayit)
        {
            db.Entry(kurkayit).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("KursListesi", "Home");


        }

    }
}