using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{

    // DTO (Data Transfer objects) is a data container for moving data between layers.
    // A DTO is only used to pass data and does not contain any business logic. 
    // i.e. a DTO is a class that contains properties ONLY and no methods. 
    public class PointOfInterestForCreationDto
    {
        // Return an error message if this required parameter is not provided.
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

    }
}
 