//Bootsshop-----------------------//
$(document).ready(function () {
    GetCategories();

	/* carousel of home page animation */
    $('#myCarousel').carousel({
        interval: 4000
    });
    $('#featured').carousel({
        interval: 4000
    });
	$(function() {
		$('#gallery a').lightBox();
	});
	
	$(document.body).on("click", ".subMenu > a", function (e)
	{
		e.preventDefault();
		var subMenu = $(this).siblings('ul');
		var li = $(this).parents('li');
		var subMenus = $('#sidebar li.subMenu ul');
		var subMenus_parents = $('#sidebar li.subMenu');
		if(li.hasClass('open'))
		{
			if(($(window).width() > 768) || ($(window).width() < 479)) {
				subMenu.slideUp();
			} else {
				subMenu.fadeOut(250);
			}
			li.removeClass('open');
		} else 
		{
			if(($(window).width() > 768) || ($(window).width() < 479)) {
				subMenus.slideUp();			
				subMenu.slideDown();
			} else {
				subMenus.fadeOut(250);			
				subMenu.fadeIn(250);
			}
			subMenus_parents.removeClass('open');		
			li.addClass('open');	
		}
	});
	var ul = $('#sidebar > ul');
	$(document.body).on("click", "#sidebar > a", function (e)
	{
		e.preventDefault();
		var sidebar = $('#sidebar');
		if(sidebar.hasClass('open'))
		{
			sidebar.removeClass('open');
			ul.slideUp(250);
		} else 
		{
			sidebar.addClass('open');
			ul.slideDown(250);
		}
	});

});


function GetCategories() {

    $.ajax({
        url: '/Home/GetCategories',
        type: 'POST',
        data: {},
        dataType: "json",
        success: function (data) {
            var menus = document.getElementById("sideManu");
            var Shown = false;
            if (data.main.length != 0) {
                var html = "";
                for (var i = 0; i < data.main.length; i++) {
                    html += "<li class=\"subMenu";

                    if (i == 0) {
                        html += " open";
                    }

                    html += "\"> <a>" + data.main[i].Name + "</a>";

                    if (Shown) {
                        html += "<ul style = \"display: none\" >";
                    }
                    else {
                        html += "<ul>";
                    }

                    for (var j = 0; j < data.Sub.length; j++) {
                        if (data.Sub[j].ParentID != data.main[i].ID)
                            continue;

                        html += "<li><a ";

                        if (!Shown) {
                            html += "class=\" active \" ";
                            Shown = true;
                        }

                        html += "onclick=\" ShowCategory(" + data.Sub[j].ID + ") \"  onmouseover=\"\" style=\"cursor: pointer;\" ><i class=\"icon-chevron-right\"></i> ";

                        html += data.Sub[j].Name + "</a></li> ";

                        data.Sub.splice(j, 1);
                        j--;
                    }

                    html += "</ul></li>";
                }

                menus.innerHTML = html;
                UpdateCart();
            }

        }
    });
}

function ShowCategory(e)
{
    location.href = "/Products/ProductsDisplay/"+e;
        
}
