using Microsoft.AspNetCore.Mvc;
using Praca_dyplomowa.Entities;
using Praca_dyplomowa.Helpers;
using Praca_dyplomowa.Models;
using Praca_dyplomowa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Praca_dyplomowa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivitiesController : ControllerBase
    {
        public User CurrentUser => (User)HttpContext.Items["User"];
        private IActivityService _activityService;
        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet("{page}/{number}")]
        public IActionResult GetTrainings(int page, int number)
        {
            PageJSON pageJSON = new PageJSON { Page = page, Number = number};
            var trainings = _activityService.GetTrainings(CurrentUser, pageJSON);
            return Ok(trainings);
        }

        [HttpGet("details")]
        public IActionResult GetTrainingDetails([FromBody] DetailsIdJSON detailsId)
        {
            var trainingDetails = _activityService.GetTrainingDetails(CurrentUser, detailsId);
            if (trainingDetails != null)
                return Ok(trainingDetails);
            return BadRequest();
        }

        [HttpPost("add")]
        public IActionResult AddRegion([FromBody] NewTrainingJSON newTraining)
        {
            var addResult = _activityService.AddTraining(CurrentUser, newTraining);
            if (addResult)
                return Ok();
            return BadRequest();
        }

        [HttpPut("edit")]
        public IActionResult EditRegion([FromBody] EditTrainingJSON editTraining)
        {
            var editResult = _activityService.EditTraining(CurrentUser, editTraining);
            if (editResult)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("remove")]
        public IActionResult DeleteRegion([FromBody] RemoveIdJSON removeId)
        {
            var deleteResult = _activityService.DeleteTraining(CurrentUser, removeId);
            if (deleteResult)
                return Ok();
            return BadRequest();
        }
    }
}
