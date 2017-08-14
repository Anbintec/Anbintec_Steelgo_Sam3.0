/********************************************/
/********************************************/
/*********                          *********/
/*********    Menu Manager          *********/
/*********                          *********/
/********************************************/
/********************************************/

/****************************/
/*    Global Variables      */
/****************************/

//Add all global variables for your partial view here
var $currentPageOnMenu = false;
var $currentUrl = window.location.href.toString().split(window.location.host)[1].toString().split('?')[0];
var $MenuData = {};

/****************************/
/*    Document Ready        */
/****************************/

//Method to be called on the document ready and contains all the pertinent code for a partial view
function menuManagerToBeExecutedOnDocumentReady() {
    //Open side menu on hover
    /*$("#sidebar").hover(
        function () { $(this).removeClass("minified"); $(".content-container").removeClass("expanded"); },
        function () { $(this).addClass("minified"); $(".content-container").addClass("expanded"); }
    );*/

    //Toggle for main menu's submenus           
    /*Open menus on hover*/
    //$(document).on("mouseenter", "ul.main-menu > li:not('.shortcut')", function () {
    //    if ($(this).hasClass('active') && !$("#sidebar").hasClass("minified")) {
    //        $("ul.sub-menu2").removeClass("open");
    //        $("li.shortcut").removeClass("open");
    //        $("ul.sub-menu > li").removeClass("active expand");
    //        $(this).children("ul").toggleClass("open");
    //        $("i.arrow").toggleClass("down");
    //    }
    //    else if (!$("#sidebar").hasClass("minified")) {
    //        $("#sidebar li").removeClass('active');
    //        $("#sidebar ul").removeClass('open');
    //        $("li.shortcut").removeClass("open");
    //        $(this).closest("li").has("ul").addClass('active').children("ul").addClass("open");
    //        $("i.arrow").addClass("down");
    //    }
    //    else {
    //        $("#sidebar li").removeClass('active');
    //        $("#sidebar ul").removeClass('open');
    //        $("li.shortcut").removeClass("open");
    //        $(this).addClass('active');
    //    }
    //}).on("mouseleave", "ul.main-menu > li:not('.shortcut')", function () {
    //    $("#sidebar li").removeClass('active');
    //    $("#sidebar ul").removeClass('open');
    //    $("li.shortcut").removeClass("open");
    //    $(this).removeClass('active');
    //});

    /*Open menus on click*/
    $(document).on("click", "ul.main-menu > li:not('.shortcut') > a", function () {
        if ($(this).closest("li").hasClass('active') && !$("#sidebar").hasClass("minified")) {
            $("ul.sub-menu2").removeClass("open");
            $("li.shortcut").removeClass("open");
            $("ul.sub-menu > li").removeClass("active expand");
            $(this).closest("li").children("ul").toggleClass("open");
            $(this).children("i.arrow").toggleClass("down");
        }
        else if (!$("#sidebar").hasClass("minified")) {
            $("#sidebar li").removeClass('active');
            $("#sidebar ul").removeClass('open');
            $("li.shortcut").removeClass("open");
            $(this).closest("li").addClass('active').children("ul").addClass("open");
            $(this).children("i.arrow").addClass("down");
        }
        else {
            $("#sidebar li").removeClass('active');
            $("#sidebar ul").removeClass('open');
            $("li.shortcut").removeClass("open");
            $(this).closest("li").addClass('active');
        }
    });

    /*Open sidebar when clicking the icons when minified*/
    $(document).on("click", "#sidebar ul.main-menu > li", function () {
        if( $("#sidebar").hasClass("minified")) {
            $("#sidebar").removeClass("minified");
            $(".content-container").toggleClass("expanded");
            $(this).children("ul").addClass("open");
        }
    });

    //Toggle for second submenu
    /*Open menus on hover*/
    //$(document).on("mouseenter", "ul.sub-menu > li", function () {
    //    if ($(this).children("ul.sub-menu2").hasClass('open')) {
    //        $(this).removeClass("active expand");
    //        $(this).children("ul.sub-menu2").removeClass("open");
    //        $("li.shortcut").removeClass("open");
    //    }        
    //    else {
    //        $("ul.sub-menu2").removeClass("open");
    //        $("ul.sub-menu > li").removeClass("active expand");
    //        $(this).addClass("active");
    //        $(this).has("ul").addClass("expand");
    //        $(this).children("ul.sub-menu2").addClass("open");
    //        $("li.shortcut").addClass("open");
    //    }

    //    if ($(this).children("ul.sub-menu2").is(':empty')) {
    //        $("li.shortcut").removeClass("open");
    //        $(this).removeClass("expand");

    //    }
    //});

    /*Open menus on click*/
    $(document).on("click", "ul.sub-menu > li > a", function () {
        if ($(this).parent().children("ul.sub-menu2").hasClass('open')) {
            $(this).parent("li").removeClass("active expand");
            $(this).parent().children("ul.sub-menu2").removeClass("open");
            $("li.shortcut").removeClass("open");
        }
        else if ($(this).parent().children("ul.sub-menu2").is(':empty')) {
            $("li.shortcut").removeClass("open");
        }
        else {
            $("ul.sub-menu2").removeClass("open");
            $("ul.sub-menu > li").removeClass("active expand");
            $(this).parent("li").addClass("active");
            $(this).parent("li").has("ul").addClass("expand");
            $(this).parent().children("ul.sub-menu2").addClass("open");
            $("li.shortcut").addClass("open");
        }
    });

    //Toggle for last submenu's links         
    $(document).on("click", "ul.sub-menu2 > li > a", function () {
        $("ul.sub-menu2 li").removeClass("active");
        $("ul.sub-menu3").removeClass("open");
    });

    //Toggle for last submenu's links         
    $(document).on("click", "ul.sub-menu3 > li > a", function () {
        $("ul.sub-menu2 li").removeClass("active");
        $("ul.sub-menu3").removeClass("open");
    });

    //Toggle shorcut section size 
    $(document).on("mouseover", "li.shortcut", function () {
        $("li.shortcut").removeClass("open");
    });

    $(document).on("click", "li.shortcut", function () {
        $("li.shortcut").removeClass("open");
    });

    //Toggle sidebar's size
    $("i.minify").click(function () {
        $("ul").removeClass("open");
        $("i.arrow").removeClass("down");
        $("#sidebar").toggleClass("minified").removeClass("open");
        $(".content-container").toggleClass("expanded");
    });
}

/****************************/
/*    Global Functions      */
/****************************/
//Update cookies for QuickLinks
function updateCookiesQuickLinks() {
    var url = window.location.href.toString().split(window.location.host)[1].toString().split('?')[0];
    //console.log(url);
    if ($('a[href="' + url + '"]').children("span").attr("id") != undefined) {
        var label = "quickLabel9002";
        if (url != "/Busqueda/Busqueda") {
            label = $('a[href="' + url + '"]').children("span").attr("id").toString().replace("menu", "quick");
        }

        //Validate if the page it's already on the quicklinks
        if (Cookies.get("QL1route") == url) {
            //console.log("Remover las cookies 1");
            //Remover las cookies 1
            Cookies.remove("QL1route", { path: '/' });
            Cookies.remove("QL1label", { path: '/' });
        }
        if (Cookies.get("QL1route") == Cookies.get("QL2route")) {
            //console.log("Remover las cookies 2");
            //Remover las cookies 2
            Cookies.remove("QL2route", { path: '/' });
            Cookies.remove("QL2label", { path: '/' });
        }
        if (Cookies.get("QL1route") == Cookies.get("QL3route")) {
            //console.log("Remover las cookies 3");
            //Remover las cookies 3
            Cookies.remove("QL3route", { path: '/' });
            Cookies.remove("QL3label", { path: '/' });
        }

        //Check if Cookies 1 not exist
        if (Cookies.get("QL1route") == null || Cookies.get("QL1route") == "" || Cookies.get("QL1label") == null || Cookies.get("QL1label") == "") {
            //console.log("Check if Cookies 1 not exist");
            //Generar las cookies 1 con mis datos
            Cookies.set("QL1route", url, { path: '/' });
            Cookies.set("QL1label", label, { path: '/' });
            //Remover las cookies 2
            Cookies.remove("QL2route", { path: '/' });
            Cookies.remove("QL2label", { path: '/' });
            //Remover las cookies 3
            Cookies.remove("QL3route", { path: '/' });
            Cookies.remove("QL3label", { path: '/' });
        } else {
            //Check if Cookies 1 exist and 2 not exist
            if (Cookies.get("QL2route") == null || Cookies.get("QL2route") == "" || Cookies.get("QL2label") == null || Cookies.get("QL2label") == "") {
                //console.log("Check if Cookies 1 exist and 2 not exist");
                //Creo las cookies 2 con los datos de las cookies 1
                Cookies.set("QL2route", Cookies.get("QL1route"), { path: '/' });
                Cookies.set("QL2label", Cookies.get("QL1label"), { path: '/' });
                //Guardo mis datos en las cookies 1
                Cookies.set("QL1route", url, { path: '/' });
                Cookies.set("QL1label", label, { path: '/' });
                //Remover las cookies 3
                Cookies.remove("QL3route", { path: '/' });
                Cookies.remove("QL3label", { path: '/' });
            } else {
                //console.log("else");
                //Creo las cookies 3 con los datos de las cookies 2
                Cookies.set("QL3route", Cookies.get("QL2route"), { path: '/' });
                Cookies.set("QL3label", Cookies.get("QL2label"), { path: '/' });
                //Guardo los datos de las cookies 1 en las cookies 2
                Cookies.set("QL2route", Cookies.get("QL1route"), { path: '/' });
                Cookies.set("QL2label", Cookies.get("QL1label"), { path: '/' });
                //Guardo mis datos en las cookies 1
                Cookies.set("QL1route", url, { path: '/' });
                Cookies.set("QL1label", label, { path: '/' });
            }
        }
    }
}

//Create the QuickLinks
function generateQuickLinks(data) {
    //console.log(data);
    //Update cookies for QuickLinks
    //updateCookiesQuickLinks();

    //Basic Structure for QuickLinks
    var htmlToAppend = "<li class='shortcut'><div class='quicklinks'><i class='icn links'></i><span id='layoutLabel0014'></span><i class='icn right gear'></i></div><ul class='sub-menu'>";

    //Validate the first quick link if not able create cookies and assign the logout element
    //if (Cookies.get("QL1route") != null && Cookies.get("QL1route") != "" && Cookies.get("QL1label") != null && Cookies.get("QL1label") != "") {
    //    htmlToAppend = htmlToAppend + "<li><a href='" + Cookies.get("QL1route") + "' onclick='redirectToLanguage(event,this)'><span id='" + Cookies.get("QL1label") + "'>QuickLink1</span></a></li>";
    //} else {
    //    htmlToAppend = htmlToAppend + "<li><a href='/Home/Landing' onclick='redirectToLanguage(event,this)'><span id='quickLabel9001'>QuickLink1</span></a></li>";
    //}

    //Validate the first quick link if not able create cookies and assign the logout element
    //if (Cookies.get("QL2route") != null && Cookies.get("QL2route") != "" && Cookies.get("QL2label") != null && Cookies.get("QL2label") != "") {
    //    htmlToAppend = htmlToAppend + "<li><a href='" + Cookies.get("QL2route") + "' onclick='redirectToLanguage(event,this)'><span id='" + Cookies.get("QL2label") + "'>QuickLink1</span></a></li>";
    //} else {
    //    htmlToAppend = htmlToAppend + "<li><a href='/Busqueda/Busqueda' onclick='redirectToLanguage(event,this)'><span id='quickLabel9002'>QuickLink1</span></a></li>";
    //}

    //Validate the first quick link if not able create cookies and assign the logout element
    //if (Cookies.get("QL3route") != null && Cookies.get("QL3route") != "" && Cookies.get("QL3label") != null && Cookies.get("QL3label") != "") {
    //    htmlToAppend = htmlToAppend + "<li><a href='" + Cookies.get("QL3route") + "' onclick='redirectToLanguage(event,this)'><span id='" + Cookies.get("QL3label") + "'>QuickLink1</span></a></li>";
    //} else {
    //    htmlToAppend = htmlToAppend + "<li><a href='#' onclick='removeUserSession()'><span id='quickLabel9003'>QuickLink1</span></a></li>";
    //}
    data.forEach(function (d) {
        htmlToAppend += "<li><a href='"+window.location.origin + d.liga + "' onclick='redirectToLanguage(event,this)'><span id='" + d.texto + "'></span></a></li>";
    })
    //Add closure elements of the basic structure
    htmlToAppend = htmlToAppend + "</ul></li>";

    //var htmlToAppend = "<li class='shortcut'><div class='quicklinks'><i class='icn links'></i><span id='layoutLabel0014'></span><i class='icn right gear'></i></div><ul class='sub-menu'>    <li>        <a href='#'>            <span>Llegada de Material</span>        </a>    </li>    <li>        <a href='#'><span>Generación Pase Salida</span></a></li><li><a href='#'><span>Despacho de Tubos</span></a></li></ul></li>";
    $(".main-menu").append(htmlToAppend);
}

//Create Level 0 DOM Elements for SideMenu
function generateSideMenuDOMElementsLevel0(elementOfMenu, appendingTarget) {
    
    //var htmlToAppend = "<li class='active'><a href='" + elementOfMenu.liga + "'>";
    var htmlToAppend = $("<li><a href='" + elementOfMenu.liga + "' onclick='redirectToLanguage(event,this)'><i class='" + elementOfMenu.icono + "'></i><span id='" + elementOfMenu.texto + "'></span><i class='icn right arrow'></i></a></li>");
    var htmlSubMenu = $("<ul class='sub-menu'></ul>");
    htmlToAppend.append(htmlSubMenu);
    appendingTarget.append(htmlToAppend);
    return htmlSubMenu;
}

//Create Level 1 DOM Elements for SideMenu
function generateSideMenuDOMElementsLevel1(elementOfMenu, appendingTarget) {
    
    var htmlToAppend = $("<li><a href='" + elementOfMenu.liga + "' onclick='redirectToLanguage(event,this)'><span id='" + elementOfMenu.texto + "'></span></a></li>");
    var htmlSubMenu2 = $("<ul class='sub-menu2'></ul>");
    htmlToAppend.append(htmlSubMenu2);
    appendingTarget.append(htmlToAppend);
    return htmlSubMenu2;
}

//Create Level 2 DOM Elements for SideMenu
function generateSideMenuDOMElementsLevel2(elementOfMenu, appendingTarget) {
    
    var htmlToAppend = $("<li><a href='" + elementOfMenu.liga + "' onclick='redirectToLanguage(event,this)'><span id='" + elementOfMenu.texto + "'></span></a></li>");
    var htmlSubMenu3 = $("<ul class='sub-menu3'></ul>");
    htmlToAppend.append(htmlSubMenu3);
    appendingTarget.append(htmlToAppend);
    return htmlSubMenu3;
}

//Create Level 3 DOM Elements for SideMenu
function generateSideMenuDOMElementsLevel3(elementOfMenu, appendingTarget) {
    var htmlToAppend = $("<li><a href='" + elementOfMenu.liga + "' onclick='redirectToLanguage(event,this)'><span id='" + elementOfMenu.texto + "'></span></a></li>");
    appendingTarget.append(htmlToAppend);
    return htmlToAppend;
}

//Recursive Method to generate the side menu
function generateSideMenuDOMElements(idPadre, nivel, appendingTarget) {
    if ($sideMenuLayout[nivel] != undefined && $sideMenuLayout[nivel][idPadre] != undefined) {
        for (key in $sideMenuLayout[nivel][idPadre]) {
            var newAppendingTarget;
            switch (nivel) {
                case 0: newAppendingTarget = generateSideMenuDOMElementsLevel0($sideMenuLayout[nivel][idPadre][key], appendingTarget); break;
                case 1: newAppendingTarget = generateSideMenuDOMElementsLevel1($sideMenuLayout[nivel][idPadre][key], appendingTarget); break;
                case 2: newAppendingTarget = generateSideMenuDOMElementsLevel2($sideMenuLayout[nivel][idPadre][key], appendingTarget); break;
                case 3: newAppendingTarget = generateSideMenuDOMElementsLevel3($sideMenuLayout[nivel][idPadre][key], appendingTarget); break;
            }
            generateSideMenuDOMElements(key, nivel + 1, newAppendingTarget);
        }
    }
}
//Method to generate the Side Menu
function generateSideMenu(data) {
    $sideMenu = {};
    $sideMenuLayout = {};

    if (typeof (data.layout) != null && typeof (data.layout.navigation) != null && data.layout.navigation[0].type == "sidemenu") {
        for (key in data.layout.navigation[0].elements) {
            var element = data.layout.navigation[0].elements[key];

            $MenuData[element.liga.split("?")[0].replace("/", "").replace("\"", "").replace("\"", "").replace("\n", "").replace("\r", "").toLowerCase()] = 1;

            if ($sideMenuLayout[element.nivel] == undefined) {
                $sideMenuLayout[element.nivel] = {};
            }

            if ($sideMenuLayout[element.nivel][element.idPadre] == undefined) {
                $sideMenuLayout[element.nivel][element.idPadre] = {};
            }

            var elementIdentifier = element.elemetId;
            if (element.acomodo != 0) {
                elementIdentifier = element.acomodo;
            }

            if ($sideMenuLayout[element.nivel][element.idPadre][elementIdentifier] == undefined) {
                $sideMenuLayout[element.nivel][element.idPadre][elementIdentifier] = {};
            }

            $sideMenuLayout[element.nivel][element.idPadre][elementIdentifier].texto = element.texto;
            $sideMenuLayout[element.nivel][element.idPadre][elementIdentifier].liga = element.liga;
            $sideMenuLayout[element.nivel][element.idPadre][elementIdentifier].icono = element.icono;
        }
    }
}