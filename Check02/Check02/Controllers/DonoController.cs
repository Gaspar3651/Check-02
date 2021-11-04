using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using Check02.Context;
using Check02.Models;

namespace Check02.Controllers
{
    public class DonoController : Controller
    {
        private Context.Context db = new Context.Context();
        private MdDono dono = new MdDono();

        // GET: Dono
        public ActionResult Index()
        {
            return View(db.ctDonos.ToList());
        }

        public static bool ValidarTelefone(string num)
        {
            Regex validation = new Regex(@"^\(?[1-9]{2}\)? ?(?:[2-8]|9[1-9])[0-9]{3}\-?[0-9]{4}$");

            if (!validation.IsMatch(num))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

       /*public JsonResult Teste(string num)
        {
            MdDono c = db.ctDonos.SingleOrDefault(s => s.Telefone == num);
            bool retorno = false;

            if (ValidarTelefone(num.ToString()))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
        }*/

        // GET: Dono/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MdDono mdDono = db.ctDonos.Find(id);
            if (mdDono == null)
            {
                return HttpNotFound();
            }
            return View(mdDono);
        }

        // GET: Dono/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Dono/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDono,NmDono,Telefone,Nascimento")] MdDono mdDono)
        {
            bool verificationTel = ValidarTelefone(mdDono.Telefone.ToString());
            if (!verificationTel)
            {
                MessageBox.Show("Telefone inválido");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.ctDonos.Add(mdDono);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(mdDono);
        }

        // GET: Dono/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MdDono mdDono = db.ctDonos.Find(id);
            if (mdDono == null)
            {
                return HttpNotFound();
            }
            return View(mdDono);
        }

        // POST: Dono/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDono,NmDono,Telefone,Nascimento")] MdDono mdDono)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mdDono).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mdDono);
        }

        // GET: Dono/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MdDono mdDono = db.ctDonos.Find(id);
            if (mdDono == null)
            {
                return HttpNotFound();
            }
            return View(mdDono);
        }

        // POST: Dono/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MdDono mdDono = db.ctDonos.Find(id);
            db.ctDonos.Remove(mdDono);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
