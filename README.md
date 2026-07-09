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

- [ ] `php-carrito/` — PHP 8 con `$_SESSION`
- [ ] `aspnet-carrito/` — ASP.NET Core 8 MVC con `ISession`
- [ ] `springboot-carrito/` — Java Spring Boot 3 con `HttpSession` y Thymeleaf

Cada plataforma se implementa por separado; ver el README dentro de cada carpeta una vez
creado para instrucciones de ejecución específicas.

## Contexto académico

- **Tipo:** GA – Práctica en clase
- **Modalidad:** Individual, presencial en laboratorio
- **Duración estimada:** 90 minutos (30 min por plataforma)
- **Prerequisito:** Temas 5, 6 y 7 (PHP, ASP.NET Core y Java/Spring Boot)
- **Objetivo:** Implementar un carrito de compras con sesiones en las tres plataformas y
  comparar su mecanismo de gestión de estado.
