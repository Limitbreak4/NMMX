
//
    $('.desk').off('click');
    $('.desk').on('click', function (event) {
        event.stopPropagation();
        console.log('desk click')
        $(".contenedor-compras").slideToggle("slow");
    });

    $(".contenedor-compras").on("click", function (event) {
        event.stopPropagation();
    });

    $(document).on("click", function () {
        $(".contenedor-compras").fadeOut();
    });

    
    $("[data-action='ADD_TO_CART']").off("click");
    $("[data-action='ADD_TO_CART']").on("click", function () {
        AddItmToCart(this);
    });

/*** 

This is the code for a shopping cart to amex-pop
JAN 2017
Carlos Martinez

**/


var jsonProductos = { Productos: [] };
var shoppingCart = { items:[] };
var strCookieShoopingCar = "shopingCartItms";
var cartItemTemplate = "<div class='cart__item  h-100'><div class='row align-items-center h-100'><div class='col-3 mx-auto'><img class='product__image cart__item__image img-fluid' src='@@itmImgUrl' alt='@@itmName' /></div>" +
    "<div class='col-5  mx-auto product__name cart__item__name' data-id='@@itmID'>@@itmName<div class='btn btn--danger btn--small delete' data-action='REMOVE_ITEM' data-id='@@itmID' >Eliminar</div></div>" +
    "<div class='col-4 mx-auto product__number cart__item__number'><input class='number' name='cantidad'  data-action='CHANGE_ITEM' data-id='@@itmID' type='number' min='1' max='@@maxQty' value='@@itmQty' ></div></div></div>";


$(document).ready(function () {

    
    RecuperarProductos();

});
function ValidaCarrito()
{
    if(shoppingCart.items.length > 0)
    {
        return true;
    }
    else
    {
        return false;
    }

}

function initShoppingCart()
{
    
    var cookieVal = Cookies.get(strCookieShoopingCar);

    if (cookieVal != null) {
        shoppingCart = JSON.parse(Cookies.get(strCookieShoopingCar));
        BuiltshoppingCart();
    }
    else
    {
        UpdateCookie();
    }

    $('.desk').off('click');
    $('.desk').on('click', function (event) {
        event.stopPropagation();
        $(".contenedor-compras").slideToggle("slow");
    });

    $(".contenedor-compras").on("click", function (event) {
        event.stopPropagation();
    });

    $(document).on("click", function () {
        $(".contenedor-compras").fadeOut();
    });

    $("[data-action='ADD_TO_CART']").off("click");
    $("[data-action='ADD_TO_CART']").on("click", function () {
        AddItmToCart(this);
    });
};

function AddItmToCart(itmElmt)
{   
    var prodID = itmElmt.attributes["data-id"].value;
    var prodObj = RecuperProductoByID(prodID);
    var inpQty =  $(itmElmt).parent().parent().find("input")[0];


    var prodQty = 0;
    prodQty = parseInt(inpQty.value);
    if (isNaN(prodQty) ||
        (prodQty == 0)) {
        return;
    }

    if (prodQty > prodObj.Cantidad) {
        prodQty = prodObj.Cantidad;
        inpQty.value = prodQty;
    }

    if (IsItemInshoppingCart(prodObj.IdProducto)) {
        DeleteItemInshoppingCart(prodObj.IdProducto);
    }
    var strNewItm = "{\"name\":\"" + prodObj.Nombre + "\",\"imgurl\":\"" + prodObj.Img1 + "\",\"quantity\":\"" + prodQty + "\",\"id\":\"" + prodObj.IdProducto + "\"}";
    shoppingCart.items.push(jQuery.parseJSON(strNewItm));

    UpdateCookie();

    BuiltshoppingCart();
    itmElmt.innerText = "Agregado";
}

function DeleteItmFromCart() {
    var prodID = $(this).attr("data-id");

    DeleteItemInshoppingCart(prodID);
    UpdateCookie();

    BuiltshoppingCart();

    if (shoppingCart.items.length == 0)
    {
        hideFinalizarForm();
    }
}

function EmptyShoppCart() {
    shoppingCart = { items: [] };
    UpdateCookie();
    BuiltshoppingCart();
}

function UpdateItemQty()
{
    var inpHtmlItm = $(this);
    var attrID = inpHtmlItm.attr("data-id");
    var attrQty = inpHtmlItm.val();

    if (attrQty == "0") {
        DeleteItemInshoppingCart(attrID);
    }
    else
    {
        for (var i = 0; i < shoppingCart.items.length; i++) {
            var vItm = shoppingCart.items[i];
            if (attrID == vItm.id) {
                shoppingCart.items[i].quantity = attrQty;
                i = shoppingCart.items.length;
            }
        }
    }
    UpdateCookie();

    BuiltshoppingCart();
}


function IsItemInshoppingCart(prodID)
{
    var itmFound = false;

    for (var i = 0; i < shoppingCart.items.length; i++) {
        var vItm = shoppingCart.items[i];
        if (prodID == vItm.id) {
            itmFound = true;
            i = shoppingCart.items.length;
        }
    }

    return itmFound;
}


function DeleteItemInshoppingCart(prodID)
{
    for (var i = 0; i < shoppingCart.items.length; i++)
    {
        if (prodID == shoppingCart.items[i].id)
        {
            shoppingCart.items.splice(i, 1);
            return true;
        }
    }

    return false;
}

function BuiltshoppingCart()
{
    
    var divNoItms = $("span.numb-cart");
    var divTotalItms = $("#total");
    var divItmsDetail = $("#div-items-carrito");
    var htmlItems = "";
    var NoItms = 0;

    for (var i = 0; i < shoppingCart.items.length; i++)
    {
        NoItms = parseInt(NoItms) + parseInt(shoppingCart.items[i].quantity);

        var prodObj = RecuperProductoByID(shoppingCart.items[i].id);

        var itmHtml = replaceEverything(cartItemTemplate, "@@itmName", shoppingCart.items[i].name);
        itmHtml = replaceEverything(itmHtml, "@@itmImgUrl", shoppingCart.items[i].imgurl);
        itmHtml = replaceEverything(itmHtml, "@@itmQty", shoppingCart.items[i].quantity);
        itmHtml = replaceEverything(itmHtml, "@@itmID", shoppingCart.items[i].id);
        itmHtml = replaceEverything(itmHtml, "@@maxQty", prodObj.Cantidad);

        htmlItems += itmHtml;
    }

    if (shoppingCart.items.length > 0)
    {
        $(".item-add-indicator").show();
        $(".empty-car").hide();
        $("#div-footer-carrito").show();
    }
    else
    {
        $(".item-add-indicator").hide();
        $(".empty-car").show();
        $("#div-footer-carrito").hide();
    }   

    divNoItms.html(NoItms.toString());
    divTotalItms.html(NoItms.toString());
    divItmsDetail.html(htmlItems);

    var deleteBtns = $("[data-action='REMOVE_ITEM']");
    for (var i = 0; i < deleteBtns.length; i++) {
        $(deleteBtns[i]).off('click');
    }
    for (var i = 0; i < deleteBtns.length; i++) {
        $(deleteBtns[i]).on('click', DeleteItmFromCart);
    }
    $("[data-action='CHANGE_ITEM']").off("input");
    $("[data-action='CHANGE_ITEM']").on("input", validarSoloNumeros);

    $("[data-action='CHANGE_ITEM']").off("blur");
    $("[data-action='CHANGE_ITEM']").on("blur", UpdateItemQty);
    $(".number").on('paste', function (e) {
        e.preventDefault();
       
    })
}


function UpdateCookie()
{
    Cookies.set(strCookieShoopingCar, JSON.stringify( shoppingCart) );    
    var strShpC = "";

    for(var i = 0; i < shoppingCart.items.length;i++)
    {
        if (i == 0)
        {
            strShpC = strShpC + shoppingCart.items[i].id.toString() + "|" + shoppingCart.items[i].quantity.toString();
        }
        else
        {
            strShpC = strShpC + "|" + shoppingCart.items[i].id.toString() + "|" + shoppingCart.items[i].quantity.toString();
        }
    }

    $("#hdCarrito").val(strShpC);    
}

function replaceEverything(strOriginal, strToReplace, strNewStr)
{
    while (strOriginal.indexOf(strToReplace) > -1) {
        strOriginal = strOriginal.replace(strToReplace, strNewStr);
    }

    return strOriginal;
}

function RecuperarProductos() {

}


function RecuperProductoByID(idPrdt) {
    var prdObj = null;

    for (var i = 0; i < jsonProductos.length; i++) {
        if (jsonProductos[i].IdProducto == idPrdt) {
            prdObj = jsonProductos[i];
            i = jsonProductos.length;
        }
    }

    return prdObj;
}



function mostrarModalItems() {
    var ancDetalle = $(this);

    
    var prodId = ancDetalle.attr("data-id");

    
    var prodObj = RecuperProductoByID(prodId);
    if (prodId != null) {

        $('#txtModalItmName').html('<p> ' + prodObj.Nombre + '</p>');
        $('#txtModalItmNombreMob').html('<p> ' + prodObj.Nombre + '</p>');
        $('#txtModalItmCantidad').html('<p>Cantidad: ' + prodObj.Cantidad + '</p>');
        $('#txtModalItmMedidas').html('<p>Medidas: <br>' + prodObj.Medidas + '</p>');
        $('#txtModalItmDescripcion').html('<p> ' + prodObj.Descripcion + '</p>');


        
        if (!prodObj.Img1) {   
            $("#imgModalItm2").hide();
            $("#imgModalItm2").attr("src", "");
            $("#imgModalItm2M").hide();
            $("#imgModalItm2M").attr("src", "");
        }
        else {
            $("#imgModalItm2").show();
            $("#imgModalItm2").attr("src", prodObj.Img1);
            $("#imgModalItm2M").show();
            $("#imgModalItm2M").attr("src", prodObj.Img1);
        }

        if (!prodObj.Img2) { 
            $("#imgModalItm3").hide();
            $("#imgModalItm3").attr("src", "");
            $("#imgModalItm3M").hide();
            $("#imgModalItm3M").attr("src", "");
        }
        else {
            $("#imgModalItm3").show();
            $("#imgModalItm3").attr("src", prodObj.Img2);
            $("#imgModalItm3M").show();
            $("#imgModalItm3M").attr("src", prodObj.Img2);
        }

        if (!prodObj.Img3) { 
            $("#imgModalItm4").hide();
            $("#imgModalItm4").attr("src", "");
            $("#imgModalItm4M").hide();
            $("#imgModalItm4M").attr("src", "");
        }
        else {
            $("#imgModalItm4").show();
            $("#imgModalItm4").attr("src", prodObj.Img3);
            $("#imgModalItm4M").show();
            $("#imgModalItm4M").attr("src", prodObj.Img3);
        }


        
        var ancDownload = $("#ancModalItemDownload");
        var divToolTip = $("#divModalItemProdQty");
        var inpQty = $("#divModalItemProdQty input");
        var divAddToCart = $("#divModalItemAddCart");

        if (prodObj.Ecommerce == "Ecommerce") {
            ancDownload.show();
            inpQty.hide();
            divAddToCart.hide();
            ancDownload.attr("href", "downloader.aspx?product_id=" + prodObj.IdProducto);
            $('#txtModalItmFormato').html('<p>Formato: <br>' + prodObj.Formato + '</p>');
        }
        else {
            ancDownload.hide();
            inpQty.show();
            divAddToCart.show();

            divToolTip.attr("title", "Cantidad mÃ¡xima que se puede solicitar: " + prodObj.Cantidad.toString());
            inpQty.attr("max", prodObj.Cantidad.toString());
            divAddToCart.attr("data-id", prodObj.IdProducto.toString());
            $('#txtModalItmFormato').html('<p>Tipo de material: <br>' + prodObj.Material + '</p>');
        }

        
        $("#exampleModal").modal('show');
    }
}




function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};


function isValidOnlyLettersAndSpace(strToTst) {
    var pattern = /^[a-zA-Z\s]*$/;
    return pattern.test(strToTst);
};


function validarSoloNumeros() {
    this.value = this.value.replace(/[^0-9]/g, '');
    var inpCant = $(this);
    var prodMax = inpCant.attr("max");
    var prodMin = inpCant.attr("min");
    
    if ((inpCant.val() == '') ||
        (inpCant.val() == "0")) {
        inpCant.val(prodMin);
    } 
    
    var intVal = parseInt(inpCant.val());
    var intProdMax = parseInt(prodMax);

    if (intVal > intProdMax) {
        inpCant.val(prodMax);
    }
}