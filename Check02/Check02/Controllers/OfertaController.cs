using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Windows;
using Check02.Context;
using Check02.Models;

namespace Check02.Controllers
{
    public class OfertaController : Controller
    {
        private Context.Context db = new Context.Context();

        // ########## LISTA PRINCIPAL ##########
        public static List<MdOferta> ListaOferta = new List<MdOferta>();
        //static List<MdOferta> ListaOferta { get; set; }

        static decimal ValorTotal = 0;
        static int IdDono;

        // GET: Oferta
        public ActionResult Index(int id)
        {

            if (db.ctDonos.Find(id) == null)
            {
                return HttpNotFound();
            }

            if (db.ctOferta.Find(id.ToString()) == null && db.ctDonos.Find(id) != null)
            {
                IdDono = (int)id;

                MdOferta mdOferta = new MdOferta();

                mdOferta.IdOferta = IdDono.ToString();
                mdOferta.IdCliente = IdDono;
                mdOferta.ValorOfertaFinal = 0;

                db.ctOferta.Add(mdOferta);
                db.SaveChanges();

                return RedirectToAction("Index/" + IdDono);
            }


            // ########## INFORMAÇÕES DO CLIENTE ##########
            List<MdDono> InformacaoCliente = db.ctDonos.ToList();
            InformacaoCliente = InformacaoCliente.Where(t => t.IdDono == id).ToList();
            ViewBag.Geral = InformacaoCliente;
            IdDono = (int)id;


            // ########## LISTA GERAL DOS PRODUTOS ##########
            List<MdServicos> ListaDosProdutos = db.ctServicos.Where(t => t.Tipo.Equals("Produto")).ToList();
            ViewBag.Servico = ListaDosProdutos;


            // ########## LISTA DO CARRINHO ##########

            if (ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault() == null)
            {
                MdOferta add = new MdOferta();
                add = db.ctOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault();

                ListaOferta.Add(add);
            }

            ViewBag.ProdutosSelecionados = ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().Carrinho;

            // ########## VALOR TOTAL ##########
            ViewBag.ValorTotal = ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().ValorOfertaFinal;

            return View(db.ctOferta.ToList());
        }



        
        public ActionResult AddProduto(int IdProduto)
        {
            MdServicos Produtos = new MdServicos();
            Produtos = db.ctServicos.Where(d => d.IdServico == IdProduto).FirstOrDefault();
            ValorTotal = Produtos.Preco;

            ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().Carrinho.Add(Produtos);
            ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().ValorOfertaFinal += ValorTotal;

            return RedirectToAction("Index/" + IdDono);
        }



        public ActionResult RemoverProduto(int IdProduto)
        {

            foreach (MdServicos item in ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().Carrinho)
            {
                if (item.IdServico == IdProduto)
                {
                    ValorTotal = item.Preco;

                    ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().Carrinho.Remove(item);
                    ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().ValorOfertaFinal -= ValorTotal;
                    break;
                }
            }

            return RedirectToAction("Index/" + IdDono);
        }

        
        public ActionResult ValidarCompra()
        {
            MdDono cliente = new MdDono();
            cliente = db.ctDonos.Where(t => t.IdDono == IdDono).FirstOrDefault();


            var ValorCompra = ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().ValorOfertaFinal;
            var ValorCredito = cliente.Credito;

            if (ValorCompra > ValorCredito)
            {
                MessageBox.Show("Você não tem crédito suficiente para esta compra !!!");
                return RedirectToAction("Index/" + IdDono);
            }
            else if (ValorCompra <= ValorCredito)
            {
                var resto = ValorCredito - ValorCompra;
                cliente.Credito = resto;

                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();


                // ########## REMOVENDO OS ITENS DO CARRINHO ##########
                ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().ValorOfertaFinal = 0;
                ListaOferta.Where(t => t.IdCliente == IdDono).FirstOrDefault().Carrinho.RemoveAll(t => t.IdServico == t.IdServico);

                MessageBox.Show("COMPRA REALIZADA COM SUCESSO !!!");
            }
            return RedirectToAction("Index/" + IdDono);
        }

/*
        // GET: Oferta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MdOferta mdOferta = db.ctOferta.Find(id);
            if (mdOferta == null)
            {
                return HttpNotFound();
            }
            return View(mdOferta);
        }


        // GET: Oferta/Create
        public ActionResult Create()
        {
            MdOferta mdOferta = new MdOferta();

            mdOferta.IdOferta = IdDono.ToString();
            mdOferta.IdCliente = IdDono;
            mdOferta.ValorOfertaFinal = 0;

            return View(mdOferta);
        }

        // POST: Oferta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdOferta,IdCliente,ValorOfertaFinal")] MdOferta mdOferta)
        {
            if (ModelState.IsValid)
            {
                mdOferta.IdOferta = IdDono.ToString();
                mdOferta.IdCliente = IdDono;
                mdOferta.ValorOfertaFinal = 0;

                db.ctOferta.Add(mdOferta);
                db.SaveChanges();
                return RedirectToAction("Index/" + IdDono);
            }

            return View(mdOferta);
        }

        // GET: Oferta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MdOferta mdOferta = db.ctOferta.Find(id);
            if (mdOferta == null)
            {
                return HttpNotFound();
            }
            return View(mdOferta);
        }

        // POST: Oferta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdOferta,IdCliente,ValorOfertaFinal")] MdOferta mdOferta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mdOferta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mdOferta);
        }


        // GET: Oferta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MdOferta mdOferta = db.ctOferta.Find(id);
            if (mdOferta == null)
            {
                return HttpNotFound();
            }
            return View(mdOferta);
        }

        // POST: Oferta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MdOferta mdOferta = db.ctOferta.Find(id);
            db.ctOferta.Remove(mdOferta);
            db.SaveChanges();
            return RedirectToAction("Index/" + IdDono);
        }
*/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public void GetImagemSol()
        {
            WebImage wbImage = new WebImage("~/Views/Shared/imagens/sol.png");
            wbImage.Resize(20, 20);
            wbImage.FileName = "quati.jpg";
            wbImage.Write();
        }

        public void GetImagemLua()
        {
            WebImage wbImage = new WebImage("~/Views/Shared/imagens/lua.png");
            wbImage.Resize(20, 20);
            wbImage.FileName = "quati.jpg";
            wbImage.Write();
        }
    }
}
