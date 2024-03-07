using Microsoft.AspNetCore.Mvc;
using MvcCoreSessionCubos.Extensions;
using MvcCoreSessionCubos.Models;
using MvcCoreSessionCubos.Repositories;
using System.Diagnostics;

namespace MvcCoreSessionCubos.Controllers
{
    public class CubosController : Controller
    {
        private RepositoryCubos repo;

        public CubosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Cubo> cubos = await this.repo.GetCubosAsync();
            return View(cubos);
        }

        public IActionResult AnyadirCuboCarrito(int? idcubo)
        {
            if (idcubo != null)
            {
                List<int> carrito;
                if (HttpContext.Session.GetString("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                }
                carrito.Add(idcubo.Value);
                HttpContext.Session.SetObject("CARRITO", carrito);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Carrito()
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito != null)
            {
                List<Cubo> cubos = await this.repo.GetCubosSessionAsync(carrito);
                return View(cubos);
            }
            return View();
        }

        public async Task<IActionResult> EliminarCuboCarrito(int? idcubo)
        {
            if (idcubo != null)
            {
                List<int> carrito =
                    HttpContext.Session.GetObject<List<int>>("CARRITO");
                carrito.Remove(idcubo.Value);
                if (carrito.Count() == 0)
                {
                    HttpContext.Session.Remove("CARRITO");
                }
                else
                {
                    HttpContext.Session.SetObject("CARRITO", carrito);
                }
            }
            return RedirectToAction("Carrito");
        }

        public async Task<IActionResult> RealizarCompra()
        {
            List<int> carrito =
                    HttpContext.Session.GetObject<List<int>>("CARRITO");
            List<Compra> compras =
                await this.repo.CreateCompraAsync(carrito);
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("Index");
        }
    }
}
