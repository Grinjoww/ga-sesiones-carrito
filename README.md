# Gestión de Estado: Sesiones y Carrito de Compras

Práctica de la asignatura **Aplicaciones Web – Ingeniería de Software – UTEQ 2025-2026**.

Implementación comparativa de un carrito de compras usando sesiones de servidor en tres
plataformas del semestre:

| Plataforma | Carpeta | Mecanismo de sesión |
|---|---|---|
| PHP 8 | [`php-carrito/`](php-carrito) | `$_SESSION` |
| ASP.NET Core 8 MVC | [`aspnet-carrito/`](aspnet-carrito) | `ISession` |
| Java Spring Boot 3 | [`springboot-carrito/`](springboot-carrito) | `HttpSession` + Thymeleaf |

## Qué construye cada implementación

Un carrito de compras mínimo con tres operaciones sobre una sesión del servidor:

- **Agregar** un producto (nombre + precio) al carrito guardado en la sesión.
- **Eliminar** un producto por su índice en el carrito.
- **Limpiar** el carrito completo (vaciar o destruir la sesión).

El mismo flujo se implementa en las tres plataformas para evidenciar las diferencias de API,
configuración y seguridad de cookies (`HttpOnly`, `Secure`, `SameSite=Strict`).

## Estado de la práctica

- [x] `php-carrito/` — PHP 8 con `$_SESSION`
- [x] `aspnet-carrito/` — ASP.NET Core 8 MVC con `ISession`
- [x] `springboot-carrito/` — Java Spring Boot 3 con `HttpSession` y Thymeleaf

Las tres implementaciones están completas: agregar producto, eliminar por índice, vaciar
carrito, mostrar total, validación de catálogo/precio en el servidor y patrón
POST/Redirect/GET.

## Cómo ejecutar cada plataforma

### PHP 8

```powershell
cd php-carrito
php -S localhost:8000
```

Abrir `http://localhost:8000/index.php`. (Alternativa: copiar la carpeta a
`C:\xampp\htdocs\carrito` y usar Apache desde XAMPP → `http://localhost/carrito/index.php`).

### ASP.NET Core 8 MVC

```powershell
cd aspnet-carrito
dotnet run
```

Arranca el perfil `https` y abre `https://localhost:5001` automáticamente (redirige desde
`http://localhost:5000`). Si es la primera vez que usas certificados de desarrollo en esta
máquina: `dotnet dev-certs https --trust`.

### Java Spring Boot 3

```powershell
cd springboot-carrito
.\mvnw.cmd spring-boot:run
```

Abrir `http://localhost:8080/carrito`. (También funciona `mvn spring-boot:run` si tienes
Maven instalado globalmente).

## Tabla comparativa

| Aspecto | PHP 8 | ASP.NET Core 8 | Spring Boot 3 |
|---|---|---|---|
| Iniciar sesión | `session_start()` en `configuracion.php` | `AddSession()` + `app.UseSession()` en `Program.cs` | `HttpSession` inyectada automáticamente por Spring MVC en el controlador |
| Guardar datos | `$_SESSION['carrito'][] = [...]` | `HttpContext.Session.SetString(clave, json)` (carrito serializado a JSON) | `session.setAttribute(clave, objeto)` (directo, sin serializar a mano) |
| Leer datos | `$_SESSION['carrito'] ?? []` | `HttpContext.Session.GetString(clave)` + `JsonSerializer.Deserialize` | `(List<Producto>) session.getAttribute(clave)` con cast |
| Limpiar / destruir sesión | `$_SESSION['carrito'] = []` (vaciar) / `session_destroy()` (destruir) | `HttpContext.Session.Remove(clave)` (vaciar) / `Session.Clear()` (destruir) | `session.removeAttribute(clave)` (vaciar) / `session.invalidate()` (destruir) |
| Cookie por defecto | `PHPSESSID` | `.AspNetCore.Session` | `CARRITO_SESSION` (personalizada en `application.yml`) |
| HttpOnly | `ini_set('session.cookie_httponly', '1')` | `options.Cookie.HttpOnly = true` | `server.servlet.session.cookie.http-only: true` |
| Secure | Activado solo si se detecta HTTPS (`configuracion.php`); en producción, forzar `true` | `CookieSecurePolicy.Always` (exige HTTPS; por eso corre sobre `https://localhost:5001`) | `false` en localhost / `true` en producción sobre HTTPS (comentado en `application.yml`) |
| SameSite | `ini_set('session.cookie_samesite', 'Strict')` | `options.Cookie.SameSite = SameSiteMode.Strict` | `server.servlet.session.cookie.same-site: strict` |

## Por qué el PFC usa JWT en lugar de sesiones de servidor

Las sesiones de servidor (las de esta práctica) funcionan bien para aplicaciones **multi-página**: el servidor guarda el estado y el navegador solo envía un ID de cookie. El PFC, en cambio, es una **SPA Angular que consume una API REST** de Spring Boot: el backend debe ser *stateless* para escalar horizontalmente sin sesiones compartidas entre instancias, y una API REST no debe depender de cookies de sesión atadas a memoria del servidor. Por eso el PFC usa un **JWT guardado en cookie `HttpOnly`**: el servidor no almacena nada (cualquier instancia puede validar el token), el flag `HttpOnly` sigue protegiendo el token contra robo por XSS igual que protege el `PHPSESSID`/`.AspNetCore.Session`/`CARRITO_SESSION` en esta práctica, y la revocación se resuelve con un `accessToken` de vida corta (1h) más un `refreshToken` separado en vez de invalidar una sesión en el servidor.

## Contexto académico

- **Tipo:** GA – Práctica en clase
- **Modalidad:** Individual, presencial en laboratorio
- **Duración estimada:** 90 minutos (30 min por plataforma)
- **Prerequisito:** Temas 5, 6 y 7 (PHP, ASP.NET Core y Java/Spring Boot)
- **Objetivo:** Implementar un carrito de compras con sesiones en las tres plataformas y
  comparar su mecanismo de gestión de estado.
