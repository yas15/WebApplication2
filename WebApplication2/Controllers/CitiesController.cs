using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using System.Linq;

namespace WebApplication2.Controllers
{
    // The ASP.NET MVC framework maps URLs to classes and methods, 
    // these classes are referred to as controllers and methods are referred to as actions.
    // Define a new Controller (i.e a class in MVC) called CitiesController, 
    // This Controller inherits from the AspNetCore.Mvc Controller class, 
    // The  Controller class gives us access to methods that respond to HTTP requests.

    // [Route("apo/cities")] here we are using attribute based routing i.e. 
    // With attribute-based routing we specify the URL pattern in an attribute.
    // To reach this controller i.e class the first part of the url must contain the string "api/cities".
    [Route("api/cities")]
    public class CitiesController : Controller
    {

        // Most actions (i.e. methods in ASP.Net MVC) return an instance of a class that derives from ActionResult.
        // Define an new action (a method in ASP.NET MVC) that returns an object that implements the IActionResult interface
        // IActionResult methods typically take a model and returns a view.
        // JsonResult derives from the ActionResult class, and is used to retun data formated as JSON.
        // The HttpGetAttribute specifies that an action supports a GET HTTP method only.
        // to get to this route you need to navigate to the URL http://localhost:54673/api/cities
        [HttpGet]
        public IActionResult GetCities()
        {
            // Using the Current Auto-Property Initializer of the class CitiesDataStore
            // Get the value of the variable Cities i.e. List<CityDto>
            // the constructor for JsonResult class i.e. JsonResult() takes  the data and transforms it into JSON
            // The JsonResult class has a property StatusCode, this property allows us to set
            // the HTTP status code for this method.
            //var temp = new JsonResult(CitiesDataStore.Current.Cities);
            //temp.StatusCode = 200;
            //return temp;


            // return an object of class OkObjectResult which is an object that produces 
            // a Http Microsoft.AspNetCore.Http.StatusCodes.Status200OK response.
            // i.e. a HTTP response message with the status code set to 200 and body of the message containing the data.
            return Ok(CitiesDataStore.Current.Cities);

        }



        // to get to this route you need to navigate to the URL http://localhost:54673/api/cities/id
        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            // find city
            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

            if (cityToReturn == null)
            {
                // return an object of class NotFoundResult
                // i.e.an object that produces a Http Microsoft.AspNetCore.Mvc.NotFoundResult response.
                return NotFound();
            }


            // return an object of class OkObjectResult which is an object that produces 
            // a Http Microsoft.AspNetCore.Http.StatusCodes.Status200OK response.
            // i.e. a HTTP response message with the status code set to 200 and body of the message containing the data.
            return Ok(cityToReturn);

        }

    }
}
