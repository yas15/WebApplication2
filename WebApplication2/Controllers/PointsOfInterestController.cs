using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {

        private ILogger<PointsOfInterestController> _logger;


        // This is the constructor for the class PointsOfInterestController.
        // The constructor requires a parameter of type ILogger<T>
        // The ASP.NET Core built-in DI container already has a service registered for interface of type ILogger<T>,
        // So the DI container will create an instance of this concrete type for us.

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
        {
            _logger = logger;
        }




        // Most actions (i.e. methods in ASP.Net MVC) return an instance of a class that derives from ActionResult.
        // Define an new action (a method in ASP.NET MVC) that returns an object that implements the IActionResult interface
        // IActionResult methods typically take a model and returns a view.
        // The HttpGetAttribute specifies that an action supports a GET HTTP method only.
        // to get to this route you need to navigate to the URL: - 
        // http://localhost:54673/api/cities/{cityId}/pointsofinterest
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            // we ALWAYS want to log exceptions
            try
            {
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }


        // http://localhost:54673/api/cities/{cityId}/pointsofinterest/{id}
        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }




        // Most actions (i.e. methods in ASP.Net MVC) return an instance of a class that derives from ActionResult.
        // Define an new action (a method in ASP.NET MVC) that returns an object that implements the IActionResult interface
        // IActionResult methods typically take a model and returns a view.
        // The HttpPostAttribute specifies that an action handles HTTP POST requests only.
        // Remember there are 2 types of HTTP messages a HTTP request message and a HTTP reponse message.
        // A HTTP POST request method, is a method we send in a HTTP request message, 
        // that requests that a web server accepts the data enclosed in the body of the request message.
        // The Action (i.e. method in ASP.Net) has 2 parameters a cityId, which it will get from the URL
        // and also a pointOfInterest, this will come from the body of the Http Request Message, 
        // The [FromBody] attribute specifies that the (PointOfInterestForCreationDto) parameter 
        // comes only from the body part of the incoming HttpRequestMessage.
        // Since we have specifies the [FromBody] attribute the ASP.Net runtime will attempt 
        // to deserialize the body of the Http Request Message into a .Net object of type PointOfInterestForCreationDto.
        // To get to this route you need to navigate to the URL: - 
        // http://localhost:54673/api/cities/{cityId}/pointsofinterest/
        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            // if the body of the http request message can not be deserialized into the .Net type PointOfInterestForCreationDto
            // i.e. pointOfInterest is null, then return a Status 400 BadRequest
            if (pointOfInterest == null)
            {
                // BadRequest() Creates an BadRequestResult that produces a Http.StatusCodes.Status400BadRequest
                return BadRequest();
            }

            // If the 'Description' and 'Name' parameters the user input are the same then...
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }


            // ModelState is a property of a Controller, and can be accessed in classes that inherit from System.Web.Mvc.Controller.
            // The ModelState represents a collection of name and value pairs that were submitted to the 
            // server during a POST. It also contains a collection of error messages for each value submitted.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // From the CitiesDataStore return the city such that c.id equals the cityId we input 
            // as a parameter of this method.
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                // NotFound() Creates an Microsoft.AspNetCore.Mvc.NotFoundObjectResult 
                // that produces a Http.StatusCodes.Status404NotFound
                return NotFound();
            }


            // demo purposes - to be improved
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                             c => c.PointsOfInterest).Max(p => p.Id);


            // calculate an Id for the new pointofinterest
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            // add new pointofinterest to the list PointsOfInterest.
            city.PointsOfInterest.Add(finalPointOfInterest);

            // the CreatedAtRoute() method returns an object of type CreatedAtActionResult
            // and a Http.StatusCodes.Status201Created response, with a HTTP location response header.
            // The Location response header indicates the URL to redirect a page to.
            // i.e. the Location response header will contain the URI of where the newly created URI can be found.
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalPointOfInterest.Id },
                finalPointOfInterest);
        }



        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        // The HTTP methods PATCH can be used to update partial resources. For instance, when you only 
        // need to update one field of the resource, PUTting a complete resource representation might 
        // be cumbersome and utilizes more bandwidth.
        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
             [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                   new PointOfInterestForUpdateDto()
                   {
                       Name = pointOfInterestFromStore.Name,
                       Description = pointOfInterestFromStore.Description
                   };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }



        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            return NoContent();
        }

    }
}