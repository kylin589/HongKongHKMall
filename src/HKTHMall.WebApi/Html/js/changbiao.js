 $(function () {
            var ccs = $(document.body).width();
            var ccd = ccs/420;
            $("#c_zoom").css("zoom",ccd);
			
			
			$(window).resize(function() {
			   wid=$(document.body).width();
			   ccw=wid/420;
			   $("#c_zoom").css("zoom",ccw);
			});
  })
 
 
  $(function () {
            var c2s = $(document.body).width();
            var c2d = c2s/640;
            $("#c2_zoom").css("zoom",c2d);
		
			$(window).resize(function() {
			   w2d=$(document.body).width();
			   c2w=w2d/640;
	
			   $("#c2_zoom").css("zoom",c2w);
			});
  })
  
  
  