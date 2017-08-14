/********************************************/
/********************************************/
/*********                          *********/
/*********   Context Menu Manager   *********/
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
function contextMenuToBeExecutedOnDocumentReady() {
    $body.on('contextmenu', function (e) {
        e.stopPropagation();
        e.preventDefault();
        //if (!$(e.target).hasClass('.tiny')) {
            // The target element is not a .tiny div
        //}
    });

}

/****************************/
/*    Global Functions      */
/****************************/

//Method to register an element to the context menu
function registerToContextMenu(elementID){
    
}