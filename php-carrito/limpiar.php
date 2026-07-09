<?php

require 'configuracion.php';

// Opción A: solo vaciar el carrito (mantiene la sesión) — la usada aquí
$_SESSION['carrito'] = [];

// Opción B: destruir toda la sesión (cerrar sesión completo)
// session_unset();
// session_destroy();
// setcookie(session_name(), '', time() - 3600, '/');

header('Location: index.php');
exit;
