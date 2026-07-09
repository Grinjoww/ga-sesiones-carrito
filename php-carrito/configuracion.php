<?php

/**
 * Configuración de sesión segura. Debe incluirse (require) al inicio de
 * cada script antes de leer o escribir en $_SESSION.
 */

// Detectar HTTPS para decidir el flag Secure de la cookie de sesión.
// En localhost sobre HTTP esto da 'false' y la cookie se envía igual;
// en producción, servir SIEMPRE sobre HTTPS para que este flag sea true.
// (Si se despliega detrás de un proxy/balanceador, dejar el flag Secure
// forzado a '1' en la configuración del servidor en vez de esta detección.)
$httpsDetectado = (!empty($_SERVER['HTTPS']) && $_SERVER['HTTPS'] !== 'off')
    || (($_SERVER['SERVER_PORT'] ?? null) == 443)
    || (($_SERVER['HTTP_X_FORWARDED_PROTO'] ?? '') === 'https');

// Configurar ANTES de session_start()
ini_set('session.cookie_httponly', '1');                       // HttpOnly
ini_set('session.cookie_secure', $httpsDetectado ? '1' : '0');  // Secure solo si hay HTTPS
ini_set('session.cookie_samesite', 'Strict');                   // Anti-CSRF
ini_set('session.use_strict_mode', '1');                        // IDs regenerados
ini_set('session.gc_maxlifetime', '1800');                      // 30 min timeout

session_start();

// Regenerar ID al iniciar sesión (previene Session Fixation)
if (!isset($_SESSION['iniciada'])) {
    session_regenerate_id(true);
    $_SESSION['iniciada'] = true;
}

// Catálogo de productos válidos: única fuente de verdad en el servidor.
// El cliente nunca envía el precio; solo el nombre, que se valida aquí.
const CATALOGO = [
    'Laptop' => 450,
    'Mouse' => 25,
    'Teclado' => 45,
    'Monitor' => 180,
];
