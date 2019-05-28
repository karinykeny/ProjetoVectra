using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ProjetoVectra.Models;

namespace ProjetoVectra.Controllers
{
    public class PessoasController : Controller
    {
        private Banco db = new Banco();

        // GET: pessoas
        public ActionResult Index(string Pesquisa = "")
        {
            var pes = db.pessoa.AsQueryable();
            if (!string.IsNullOrEmpty(Pesquisa))
            {
                
                pes = pes.Where(p => p.Cpf.Contains(Pesquisa));
              
            }

            return View(pes.ToList());
        }

        // GET: pessoas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.pessoa.Find(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // GET: pessoas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: pessoas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cpf,Nome,Idade,Peso,Altura,Imc")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                if (ValidarCpf(pessoa.Cpf))
                {
                    var people = (from item in db.pessoa where item.Cpf.Equals(pessoa.Cpf) select item).ToArray();

                    if (people.Length == 0)
                    {
                        pessoa.Imc = Math.Round((pessoa.Peso / (pessoa.Altura * pessoa.Altura) * 10000), 1);
                        db.pessoa.Add(pessoa);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "CPF já cadastrado!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "CPF Inválido!");
                }
            }
            return View(pessoa);
        }

        // GET: pessoas/Edit/5
        public ActionResult Edit(int? id)
        {          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.pessoa.Find(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // POST: pessoas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cpf,Nome,Idade,Peso,Altura,Imc")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                if (ValidarCpf(pessoa.Cpf))
                {
                    pessoa.Imc = Math.Round((pessoa.Peso / (pessoa.Altura * pessoa.Altura) * 10000), 1);
                    db.Entry(pessoa).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "CPF Inválido!");
                }
            }
            return View(pessoa);
        }

        // GET: pessoas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.pessoa.Find(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // POST: pessoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pessoa pessoa = db.pessoa.Find(id);
            db.pessoa.Remove(pessoa);
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

        public bool ValidarCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
            {
                return false;
            }
            tempCpf = cpf.Substring(0, 9);

            soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }
            resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }
            resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
