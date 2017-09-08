/********************************************/
/********************************************/
/*********                          *********/
/********* Grid Extensions Manager  *********/
/*********                          *********/
/********************************************/
/********************************************/

/****************************/
/*    Global Variables      */
/****************************/

//Add all global variables for your partial view here

/****************************/
/*    Document Ready        */
/****************************/

//Method to be called on the document ready and contains all the pertinent code for a partial view
function gridExtensionsToBeExecutedOnDocumentReady() {
    //CODE
}

/****************************/
/*    Global Functions      */
/****************************/

//Add all your global functions for your partial view here


function CustomisaGrid(elemento) {
    $(".k-grid-header-wrap  ", elemento).after("<div class='ScrollUp'><div class='DivScrollSup'>&nbsp;</div></div>");
    $(".DivScrollSup", elemento).css("width", $('table[role="grid"]').width())
    $(".k-grid-content", elemento).css("overflow", "hidden");

    $(".k-grid-header", elemento).before("<div id='sticky-anchor' class='Css_sticky-anchor'></div>");
    $(".k-grid-header", elemento).before("<div id='sticky-anchor' class='Css_sticky-anchor'></div>");
    $(".k-grid-header", elemento).before("<ul id='contextMenu' class='dropdown-menu' role='menu' style='display:none; width:150px; min-width:150px; z-index:2000'><li class='Ajustar_texto'>Ajustar texto</li></ul>");

    //Scroll en en la parte Superior
    $(".k-grid-content", elemento).scroll(function () {
        $(".ScrollUp", elemento)
            .scrollLeft($(".k-grid-content", elemento).scrollLeft());
    });
    $(".ScrollUp", elemento).scroll(function () {
        $(".k-grid-content", elemento)
            .scrollLeft($(".ScrollUp", elemento).scrollLeft());
    });

    $(window).scroll(function () { sticky_relocate(elemento) });

    //textoAjustado
    $.fn.contextMenu = function (settings) {

        return this.each(function () {

            // Open context menu
            $(this).on("contextmenu", function (e) {
                // return native menu if pressing control
                if (e.ctrlKey) return;

                //open menu
                var $menu = $(settings.menuSelector)
                    .data("invokedOn", $(e.target))
                    .show()
                    .css({
                        position: "absolute",
                        left: e.clientX - 100,
                        top: 25,
                    })
                    .off('click')
                    .on('click', 'li', function (e) {
                        $menu.hide();

                        var $invokedOn = $menu.data("invokedOn");
                        var $selectedMenu = $(e.target);

                        settings.menuSelected.call(this, $invokedOn, $selectedMenu);
                    });

                return false;
            });

            //make sure menu closes on any click
            $('body').click(function () {
                $(settings.menuSelector).hide();
            });
        });
    };
    $(".k-grid-header", elemento).contextMenu({
        menuSelector: "#contextMenu",
        menuSelected: function (invokedOn, selectedMenu) {
            if (selectedMenu.hasClass("Ajustar_texto")) {
                if ($(".k-grid-content td").css("white-space") == "nowrap") {
                    $(".k-grid-content td").css("white-space", "normal");
                }
                else {
                    $(".k-grid-content td").css("white-space", "nowrap");

                }
            }
        }
    });


}
//encabezado fijo
function sticky_relocate(elemento) {
    var window_top = $(window).scrollTop();
    var offset = $('#sticky-anchor', elemento).offset().top;
    if (window_top > offset) {
        $('.k-grid-header', elemento).addClass('stick');
        $(".k-grid-header", elemento).css("width", $('.k-grid-content', elemento).width())
        $(window).resize(function () {
            $(".k-grid-header", elemento).css("width", $('.k-grid-content', elemento).width())
        });
    } else {
        $('.k-grid-header', elemento).removeClass('stick');
        $(".k-grid-header", elemento).css("width", "inherit")
    }
}