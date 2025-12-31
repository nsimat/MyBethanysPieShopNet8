using BethanysPieShopNet8.Models;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Controllers.Api
{
    /// <summary>
    /// Controller for searching pies.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        #region Fields and Constructor

        /// <summary>
        /// Pie repository for accessing pie data.
        /// </summary>
        private readonly IPieRepository _pieRepository;

        /// <summary>
        /// Logger for logging information and errors.
        /// </summary>
        private readonly ILogger<SearchController> _logger;

        /// <summary>
        /// Constructor for SearchController.
        /// </summary>
        /// <param name="pieRepository">Repository for accessing pie data</param>
        /// <param name="logger">Logger for logging information and errors</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SearchController(IPieRepository pieRepository, ILogger<SearchController> logger)
        {
            _pieRepository = pieRepository ?? throw new ArgumentNullException(nameof(pieRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Endpoints operations

        // GET: /Search/
        /// <summary>
        /// Retrieves all the pies from the database.   
        /// </summary>
        /// <returns>All pies</returns>
        /// <response code="200">Returns all pies</response>
        /// <response code="204">No pies found</response>
        /// <response code="500">Internal server error</response>
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
        /// <summary>
        /// Retrieves a pie by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the pie to retrieve</param>
        /// <returns>The pie with the specified ID</returns>
        /// <response code="200">Returns the pie</response>
        /// <response code="404">Pie not found</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="500">Internal server error</response>
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

        // POST: /Search/
        /// <summary>
        /// Searches for pies by name or description.
        /// </summary>
        /// <param name="searchQuery">The search query to filter pies</param>
        /// <returns>An array of pies matching the search criteria</returns>
        /// <response code="200">Returns the matching pies</response>
        /// <response code="204">No pies found</response>
        /// <response code="500">Internal server error</response>
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
        #endregion
    }
}
