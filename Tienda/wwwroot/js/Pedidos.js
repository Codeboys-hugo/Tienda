

function CambioCantidad(id) {
    var precio = $("#" + id + "-Precio").val();
    var cantidad = $("#" + id + "-Cantidad").val();
    $("#" + id + "-Total").val(precio * cantidad);

    var suma = 0;
    var productos = 0;
    $('.total').each(function () {
        suma += parseFloat($(this).val());
    });
   
    $("#TotalTotal").val(suma);
    $('.cantidad').each(function () {
        productos += parseFloat($(this).val());
    });
    $("#CantidadTotal").val(productos);
}