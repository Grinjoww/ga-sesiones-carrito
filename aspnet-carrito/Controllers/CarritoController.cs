using CarritoASP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CarritoASP.Controllers;

public class CarritoController : Controller
{
    // Clave de la sesión
    private const string ClaveCarrito = "carrito";

    // Catálogo de precios válidos (servidor, nunca del cliente)
    private static readonly Dictionary<string, decimal> Precios = new()
    {
        ["Laptop"] = 450m,
        ["Mouse"] = 25m,
        ["Teclado"] = 45m,
        ["Monitor"] = 180m,
    };

    // Helper: leer el carrito de la sesión
    private List<Producto> ObtenerCarrito()
    {
        var json = HttpContext.Session.GetString(ClaveCarrito);
        return json is null
            ? new List<Producto>()
            : JsonSerializer.Deserialize<List<Producto>>(json) ?? new List<Producto>();
    }

    // Helper: guardar el carrito en la sesión
    private void GuardarCarrito(List<Producto> carrito)
    {
        HttpContext.Session.SetString(ClaveCarrito, JsonSerializer.Serialize(carrito));
    }

    // GET /Carrito/Index (ruta por defecto: "/") -- mostrar catálogo y carrito
    public IActionResult Index()
    {
        ViewBag.Carrito = ObtenerCarrito();
        ViewBag.Catalogo = Precios.Keys.ToList();
        ViewBag.SessionId = HttpContext.Session.Id;
        return View();
    }

    // POST /Carrito/Agregar -- agregar producto
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Agregar(string nombre)
    {
        if (Precios.TryGetValue(nombre, out var precio))
        {
            var carrito = ObtenerCarrito();
            carrito.Add(new Producto(nombre, precio));
            GuardarCarrito(carrito);
        }
        return RedirectToAction(nameof(Index)); // PRG pattern
    }

    // POST /Carrito/Eliminar -- eliminar por índice
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Eliminar(int indice)
    {
        var carrito = ObtenerCarrito();
        if (indice >= 0 && indice < carrito.Count)
        {
            carrito.RemoveAt(indice);
        }
        GuardarCarrito(carrito);
        return RedirectToAction(nameof(Index));
    }

    // POST /Carrito/Limpiar -- vaciar el carrito
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Limpiar()
    {
        HttpContext.Session.Remove(ClaveCarrito);
        // Para destruir toda la sesión:
        // HttpContext.Session.Clear();
        return RedirectToAction(nameof(Index));
    }
}
