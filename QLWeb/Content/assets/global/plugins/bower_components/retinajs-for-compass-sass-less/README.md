# retina.js for Sass
### JavaScript and SASS or LESS helpers for rendering high-resolution image variants

retina.js makes it easy to serve high-resolution images to devices with retina displays


## How it works

When your users load a page, retina.js checks each image on the page to see if there is a high-resolution version of that image on your server. If a high-resolution variant exists, the script will swap in that image in-place.

The script assumes you use Apple's prescribed high-resolution modifier (@2x) to denote high-resolution image variants on your server.

For example, if you have an image on your page that looks like this:
	
		<img src="/images/my_image.png" />

The script will check your server to see if an alternative image exists at this path:
	
		"/images/my_image@2x.png"


##How to use

###JavaScript

The JavaScript helper script automatically replaces images on your page with high-resolution variants (if they exist). To use it, download the script and include it at the bottom of your page.

1. Place the retina.js file on your server
2. Include the script on your page (put it at the bottom of your template, before your closing \</body> tag)

		<script type="text/javascript" src="/scripts/retina.js"></script> 

###SASS
The Sass mixin helps for applying high-resolution background images in your stylesheet. You provide it with an image path and the dimensions of the original-resolution image. The mixin creates a media query spefically for Retina displays, changes the background image for the selector elements to use the high-resolution (@2x) variant and applies a background-size of the original image in order to maintain proper dimensions. To use it, download the mixin, import or include it in your SASS stylesheet, and apply it to elements of your choice.

*Syntax:*

		@mixin at2x($image_name, $w: auto, $h: auto, $extention: '.png') 
		
		The extention defaults to PNG. To change this - define $extention when calling (ie jpg);
		
*Steps:*

		1.	Add the .at2x() mixin from retina.scss to your compass stylesheet (or reference it in an @import statement)
		2.	In your stylesheet, call the .at2x() mixin anywhere instead of using background-image 

				#logo {
				  @include at2x('example', 200px, 100px, .jpg);
				} 

		Will compile to: 

				#logo {
				  background-image: url('/images/example.jpg');
				}

				@media all and (-webkit-min-device-pixel-ratio: 1.5) {
				  #logo {
				    background-image: url('/images/exampe@2x.jpg');
				    background-size: 200px 100px;
				  }
				}
			


###LESS

The LESS CSS mixin is a helper for applying high-resolution background images in your stylesheet. You provide it with an image path and the dimensions of the original-resolution image. The mixin creates a media query spefically for Retina displays, changes the background image for the selector elements to use the high-resolution (@2x) variant and applies a background-size of the original image in order to maintain proper dimensions. To use it, download the mixin, import or include it in your LESS stylesheet, and apply it to elements of your choice.

*Syntax:*

		.at2x(@path, [optional] @width: auto, [optional] @height: auto);

*Steps:*

1.	Add the .at2x() mixin from retina.less to your LESS stylesheet (or reference it in an @import statement)
2.	In your stylesheet, call the .at2x() mixin anywhere instead of using background-image 

		#logo {
		  .at2x('/images/my_image.png', 200px, 100px);
		} 

Will compile to: 

		#logo {
		  background-image: url('/images/my_image.png');
		}

		@media all and (-webkit-min-device-pixel-ratio: 1.5) {
		  #logo {
		    background-image: url('/images/my_image@2x.png');
		    background-size: 200px 100px;
		  }
		}