using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http.Results;

namespace API.Controllers
{
    public class ValidarFirmaController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ValidarFirma
        public IQueryable<validar> GetvalidarFirma()
        {
            return db.validar;
        }

        // GET: api/ValidarFirma/5
        [ResponseType(typeof(validar))]
        public IHttpActionResult GetValidarFirma(int id)
        {
            validar validarFirma = db.validar.Find(id);
            if (validarFirma == null)
            {
                return NotFound();
            }

            return Ok(validarFirma);
        }

        // PUT: api/ValidarFirma/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutValidarFirma(int id, validar validarFirma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != validarFirma.Id)
            {
                return BadRequest();
            }

            db.Entry(validarFirma).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValidarFirmaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ValidarFirma
        [ResponseType(typeof(validar))]
        public IHttpActionResult PostValidarFirma(validar validarFirma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(validarFirma==null || validarFirma.Hash==""|| validarFirma.Mensaje == "")
            return new BadRequestResult(this);

            SHA256 mySHA256 = SHA256Managed.Create();

            SHA256Managed crypt = new SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(validarFirma.Mensaje), 0, Encoding.UTF8.GetByteCount(validarFirma.Mensaje));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }


            if (string.Equals(validarFirma.Hash, hash.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return Json("{ validado: true, mensaje: " + validarFirma.Mensaje + "}");
            }
            else
            {
                return Json("{ validado: false, mensaje: " + validarFirma.Mensaje + "}");
            }


        }

        // DELETE: api/ValidarFirma/5
        [ResponseType(typeof(validar))]
        public IHttpActionResult DeleteValidarFirma(int id)
        {
            validar validarFirma = db.validar.Find(id);
            if (validarFirma == null)
            {
                return NotFound();
            }

            db.validar.Remove(validarFirma);
            db.SaveChanges();

            return Ok(validarFirma);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ValidarFirmaExists(int id)
        {
            return db.validar.Count(e => e.Id == id) > 0;
        }
    }
}