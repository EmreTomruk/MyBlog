/*!
    * Start Bootstrap - SB Admin v6.0.1 (https://startbootstrap.com/templates/sb-admin)
    * Copyright 2013-2020 Start Bootstrap
    * Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-sb-admin/blob/master/LICENSE)
    */
    (function($) {
    "use strict";

    // Add active state to sidbar nav links
        var path = window.location.href; // because the 'href' property of the DOM element is the absolute path

        $("#layoutSidenav_nav .sb-sidenav a.nav-link").each(function() {
            if (this.href === path) {
                $(this).addClass("active");
            }
        });

    // Toggle the side navigation
        $("#sidebarToggle").on("click", function (e) {

            e.preventDefault();
            $("body").toggleClass("sb-sidenav-toggled");
        });
    })(jQuery);

<<<<<<< HEAD
    //Get the button
=======
//    /*Scroll Bar*/
//$(document).ready(function () {
//    $(window).scroll(function () {
//        if ($(this).scrollTop() > 50) {
//            $('#back-to-top').fadeIn();
//        }
//        else {
//            $('#back-to-top').fadeOut();
//        }
//    });
//    // scroll body to 0px on click

//    $('#back-to-top').click(function () {
//        $('body, html').animate(
//            {
//                scrollTop: 0
//            }, 400);
//        return false;
//    });
//});


//Get the button
>>>>>>> parent of 9f96e26 (Last Changes)
    var mybutton = document.getElementById("myBtn");

    // When the user scrolls down 20px from the top of the document, show the button
    window.onscroll = function() {scrollFunction()};

    function scrollFunction()
    {
        if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20)
        {
            mybutton.style.display = "block";
        }
        else
        {
            mybutton.style.display = "none";
        }
    }

    // When the user clicks on the button, scroll to the top of the document
    function topFunction()
    {
        document.body.scrollTop = 0;
        document.documentElement.scrollTop = 0;
    }
