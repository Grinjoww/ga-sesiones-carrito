<?php

require 'configuracion.php';

$nombre = filter_input(INPUT_POST, 'nombre', FILTER_SANITIZE_SPECIAL_CHARS);

// Validar que el producto exista en el catálogo del servidor.
// El precio NUNCA se toma del cliente: se busca aquí con el nombre.
if ($nombre !== null && array_key_exists($nombre, CATALOGO)) {
    if (!isset($_SESSION['carrito'])) {
        $_SESSION['carrito'] = [];
    }

    $_SESSION['carrito'][] = [
        'nombre' => $nombre,
        'precio' => CATALOGO[$nombre],
    ];
}

// Patrón Post/Redirect/Get: evitar reenvío del formulario
header('Location: index.php');
exit;
