using BethanysPieShopNet8.Models;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IPieRepository _pieRepository;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IPieRepository pieRepository, ILogger<SearchController> logger)
        {
            _pieRepository = pieRepository ?? throw new ArgumentNullException(nameof(pieRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: /Search/
        [HttpGet]
        [EndpointSummary("Get all the pies from database")]
        [EndpointDescription("Retrieves all the pies from the database. Returns a 204 No Content status if no pies are found, or a 500 Internal Server Error if an unexpected error occurs.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Pie>))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public IActionResult GetAllPies()
        {
            _logger.LogInformation("Retrieving all the pies...");

            try
            {
                var allPies = _pieRepository.AllPies;

                if (!allPies.Any())
                    return NoContent();

                return Ok(allPies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all the pies...");

                return Problem(
                    detail: "An unexpected error occurred while processing your request!",
                    instance: HttpContext.TraceIdentifier,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error"
                );
            }
        }

        // GET: /Search/1
        [HttpGet("{id}")]
        [EndpointSummary("Get a pie by ID from database")]
        [EndpointDescription("Retrieves a pie by its ID from the database. Returns a 404 Not Found status if the pie does not exist, or a 500 Internal Server Error if an unexpected error occurs.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pie))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public IActionResult GetAPie(int id)
        {
            string msg = $"Retrieving pie with ID {id}...";

            _logger.LogInformation(msg);

            if (id <= 0)
            {
                return Problem(
                    detail: "Invalid pie ID provided.",
                    instance: HttpContext.TraceIdentifier,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Bad Request"
                );
            }

            try
            {
                var pie = _pieRepository.GetPieById(id);

                if (pie == null)
                {
                    return Problem(
                        detail: $"Pie with ID {id} was not found!",
                        instance: HttpContext.TraceIdentifier,
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Pie not found."
                    );
                }

                return Ok(pie);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access");

                return Problem(
                    detail: ex.Message,
                    instance: HttpContext.TraceIdentifier,
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized access"
                );
            }
            catch (Exception ex)
            {
                // Log the exception details
                string errorMsg = $"An unexpected error occurred while retrieving pie with id {id}.";

                _logger.LogError(ex, errorMsg);

                return Problem(
                    detail: "An unexpected error occurred while processing your request.",
                    instance: HttpContext.TraceIdentifier,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error"
                );
            }
        }

        [HttpPost]
        [EndpointSummary("Search for pies by name or description")]
        [EndpointDescription("Searches for pies by name or description. Returns an array of pies or empty arrayr.")]
        public IActionResult SearchPies([FromBody] string searchQuery)
        {
            string msg = $"Launching a search process for {searchQuery}...";

            _logger.LogInformation(msg);

            IEnumerable<Pie> pies = new List<Pie>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                pies = _pieRepository.SearchPies(searchQuery);
            }

            return new JsonResult(pies);
        }
    }
}
