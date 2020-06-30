$(".open-submenu").off('click');
    $(".open-submenu").on("click", function (event) {
        event.stopPropagation();
        console.log('click from mobile open menu...')
    });

 $('.mobile-open-menu').off('click');
    $('.mobile-open-menu').on('click', function (event) {
        
        $('#showLayers').toggleClass('rotate');
        event.stopPropagation();
        $(".open-submenu").slideToggle("slow");
    });


console.log('main is running now...')


var scrollDuration = 300;
var paddleMargin = 40;

var leftPaddle = document.getElementsByClassName('left-paddle');
var rightPaddle = document.getElementsByClassName('right-paddle');

var menuItemsArray = $('.menu .item');
var itemsLength = 0;
var itemSize = 0;
var menuWrapperSize = 0;
var menuScrollLeft = 0;


$(document).ready(function () {

});


function initAssets() {
    
    $("#cp").autocomplete({
        source: function (request, response) {
            var param = $('#cp').val();
            

            
            
            
            
            
            
            
            
            
            
            
            
            
        },
        minLength: 3,
        select: function (event, ui) {
            cargarDireccion(ui.item.value);
        }
    });

    
    $('#cp').off('keyup');
    $('#cp').on('keyup', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    
    var modal = $(document.getElementById('myModal-custom'));
    
    var spanCloseModal = $(document.getElementsByClassName("close-modal")[0]);

    
    var acnDestacadaDetalle1 = $("#DesDeta1");
    var acnDestacadaDetalle2 = $("#DesDeta2");
    var ancDetalleItms = $("[data-action='modal-item-detail']");
    var inputCantidad = $("input[name='cantidad']");

    
    acnDestacadaDetalle1.off('click');
    acnDestacadaDetalle1.on('click', mostrarModalItems);

    acnDestacadaDetalle2.off('click');
    acnDestacadaDetalle2.on('click', mostrarModalItems);

    ancDetalleItms.off("click");
    ancDetalleItms.on("click", mostrarModalItems);

    inputCantidad.off("input");
    inputCantidad.on("input", validarSoloNumeros);


    
    spanCloseModal.off('click');
    spanCloseModal.on('click', function () {
        modal.hide();
    });

    
    window.onclick = function (event) {
        if (event.target.id == modal.attr("id")) {
            modal.hide();
        }
    }

    
    $(".finalizar").off('click');
    $(".finalizar").on('click', function () {
        showFinalizarForm();
        $(".contenedor-compras").hide();
    });

    /* menu  */
    $('.mobile-open-menu').off('click');
    $('.mobile-open-menu').on('click', function (event) {
        
        $('#showLayers').toggleClass('rotate');
        event.stopPropagation();
        $(".open-submenu").slideToggle("slow");
    });

    $(".open-submenu").off('click');
    $(".open-submenu").on("click", function (event) {
        event.stopPropagation();
        console.log('click from mobile open menu...')
    });

    $(document).on("click", function () {
        $(".open-submenu").fadeOut();
        $('#showLayers').toggleClass('rotate');
    });

    
    $('.owl-carousel').owlCarousel({
        items: 1,
        loop: false,
        center: true,
        margin: 10,
        dots: false,
        callbacks: true,
        URLhashListener: true,
        autoplayHoverPause: true,
        startPosition: 'URLHash'
    });

    
    var divD1 = $("#divDestado1");
    var divD2 = $("#divDestado2");
    var divD1Qty = $("#divD1Qty");
    var divD2Qty = $("#divD2Qty");
    var inpQtD1 = $("#inp-qty-des-1");
    var inpQtD2 = $("#inp-qty-des-2");
    var btnAddD1 = $("#btnAddDes1");
    var btnAddD2 = $("#btnAddDes2");

    var prodD1 = RecuperProductoByID(btnAddD1.attr("data-id"));
    var prodD2 = RecuperProductoByID(btnAddD2.attr("data-id"));

    if (prodD1 != null) {
        divD1.show();
        divD1Qty.attr("title", "Cantidad máxima que se puede solicitar: " + prodD1.Cantidad.toString());
        inpQtD1.attr("max", prodD1.Cantidad);
    }
    else {
        divD1.hide();
    }

    if (prodD2 != null) {
        divD2.show();
        divD2Qty.attr("title", "Cantidad máxima que se puede solicitar: " + prodD2.Cantidad.toString());
        inpQtD2.attr("max", prodD2.Cantidad);
    } else {
        divD2.hide();
    }


    
    setMenuScrollArrows();
    
    
    $(window).off('resize');
    $(window).on('resize', setMenuScrollArrows);

    
    $('.menu').off('scroll');
    $('.menu').on('scroll', setShowHideScrollArrows);

    var pPedir = getUrlParameter("pedir");
    if (pPedir === "true") {
        showFinalizarForm();
    }

    $("#numeroestablecimiento").off("input");
    $("#numeroestablecimiento").on("input", validarSoloNumeros);

    $("#finalizar").on("hidden.bs.modal", function () {
        $('.solicitud').hide();
        $("#containerAll").show();
    });

    $(".menu").scrollLeft(menuScrollLeft);
}


function setMenuScrollArrows() {

    menuItemsArray = $('.menu .item');
    itemsLength = menuItemsArray.length;
    itemSize = menuItemsArray.outerWidth(true) + 30;
    menuItemsArray.off('click');
    menuItemsArray.on('click', function () {
        menuScrollLeft = $(".menu").scrollLeft();
    });
    
    
    menuWrapperSize = $('.menu').outerWidth();

    
    var menuSize = itemsLength * itemSize;

    
    var menuInvisibleSize = menuSize - menuWrapperSize;

    if (menuInvisibleSize > 20) {
        
        $(leftPaddle).removeClass('hidden');
        $(rightPaddle).removeClass('hidden');

        setShowHideScrollArrows();

        

        $(rightPaddle).off('click');
        $(rightPaddle).on('click', function () {
            $('.menu').animate({ scrollLeft: menuInvisibleSize }, scrollDuration);
        });

        
        $(leftPaddle).off('click');
        $(leftPaddle).on('click', function () {
            $('.menu').animate({ scrollLeft: '0' }, scrollDuration);
        });
    }
    else {
        
        $(leftPaddle).addClass('hidden');
        $(rightPaddle).addClass('hidden');
    }
}

function setShowHideScrollArrows() {

    
    menuSize = itemsLength * itemSize;
    menuInvisibleSize = menuSize - menuWrapperSize;
    
    var menuPosition = $('.menu').scrollLeft();
    var menuEndOffset = menuInvisibleSize - paddleMargin;

    
    $(leftPaddle).removeClass('hidden');
    $(rightPaddle).removeClass('hidden');

    
    if (menuPosition <= paddleMargin) {
        $(leftPaddle).addClass('hidden');
        $(rightPaddle).removeClass('hidden');
    }

    if (menuPosition >= menuEndOffset) {
        $(leftPaddle).removeClass('hidden');
        $(rightPaddle).addClass('hidden');
    }
}

function pedir_click() {

    $(this).attr("disabled", true);

    $("#pedir_processing").show();
    
    var nombreestablecimiento = $('#nombreestablecimiento').val().trim();
    if (nombreestablecimiento == "") {
        alert('El campo Nombre de Establecimiento no ha sido capturado.');
        $('#nombreestablecimiento').focus();
        $("#pedir_processing").hide();
        return false;
    }

    
    var nombrecontacto = $('#nombrecontacto').val().trim();
    if (nombrecontacto == "") {
        alert('El campo Nombre de Contacto no ha sido capturado.');
        $('#nombrecontacto').focus();
        $("#pedir_processing").hide();
        return false;
    }

    
    var strTelefono = $('#telefono').val().trim();
    if (!strTelefono) {
        alert('El campo télefono no ha sido capturado.');
        $('#telefono').focus();
        $("#pedir_processing").hide();
        return false;
    }
    
    
    var strNumeroestablecimiento = $('#numeroestablecimiento').val().trim();
    if (strNumeroestablecimiento == "") {
        alert('El campo Número de Establecimiento no ha sido capturado.');
        $('#numeroestablecimiento').focus();
        $("#pedir_processing").hide();
        return false;
    }

    if (strNumeroestablecimiento.length != 10) {
        alert('El campo Número de Establecimiento debe tener 10 digitos.');
        $('#numeroestablecimiento').focus();
        $("#pedir_processing").hide();
        return false;
    }

    var strNoEstFirst3 = strNumeroestablecimiento.substr(0, 3);

    if (strNoEstFirst3 != "935" && strNoEstFirst3 != "813") {
        alert('El campo Número de Establecimiento debe iniciar con 935 o 813.');
        $('#numeroestablecimiento').focus();
        $("#pedir_processing").hide();
        return false;
    }

    
    var strCalle = $('#calle').val().trim();
    if (!strCalle) {
        alert('El campo Calle no ha sido capturado.');
        $('#calle').focus();
        $(this).attr("disabled", false);
        $("#pedir_processing").hide();
        return false;
    }

    
    var strCP = $('#cp').val().trim();
    if (!strCP) {
        alert('El campo Código postal no ha sido capturado.');
        $('#cp').focus();
        $(this).attr("disabled", false);
        $("#pedir_processing").hide();
        return false;
    }
    
    var strCd = $('#cd').val().trim();
    if (!strCd) {
        alert('El campo Ciudad no ha sido capturado.');
        $('#cd').focus();
        $(this).attr("disabled", false);
        $("#pedir_processing").hide();
        return false;
    }

    
    var strColonia = $('#colonia').val().trim();
    if (!strColonia) {
        alert('El campo Colonia no ha sido capturado');
        $('#colonia').focus();
        $(this).attr("disabled", false);
        $("#pedir_processing").hide();
        return false;
    }

    
    
    var strEmailestablecimiento = $('#emailestablecimiento').val().trim();
    if (strEmailestablecimiento == "") {
        alert('El campo Email Establecimiento no ha sido capturado.');
        $('#emailestablecimiento').focus();
        $("#pedir_processing").hide();
        return false;
    }
    
    if (!isValidEmailAddress(strEmailestablecimiento)) {
        alert('El campo Email Establecimiento no ha es válido.');
        $('#emailestablecimiento').focus();
        $(this).attr("disabled", false);
        $("#pedir_processing").hide();
        return false;

    }

    
    if (!ValidaCarrito()) {
        alert('No se han encontrado productos en tu carrito.');        
        $(this).attr("disabled", false);
        $("#pedir_processing").hide();
        return false;
    }

    return true;
}

function showModalFinalizar() {
    $("#btnPedir").attr("disabled", false);
    $("#pedir_processing").hide();
    $('#finalizar').modal("show");
    EmptyShoppCart();
}

function showFinalizarForm() {
    $('.solicitud').show();
    $("#containerAll").hide();
    $("#nombreestablecimiento").focus();
}

function hideFinalizarForm() {
    $('.solicitud').hide();
    $("#containerAll").show();
}



function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
};


function cargarDireccion(dataCP) {
    setTimeout(function () {
        document.getElementById("cp").value = "";
        var lsStr = dataCP.split(',');
        document.getElementById("colonia").value = lsStr[1];
        document.getElementById("cd").value = lsStr[2];
        document.getElementById("cp").value = lsStr[0];
    }, 100);
}