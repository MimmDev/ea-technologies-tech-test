using DownstreamCustomers.Models.Incoming;
using DownstreamCustomers.Services;
using Microsoft.AspNetCore.Mvc;

namespace DownstreamCustomers.Controllers;

[ApiController]
[Route("customers")]
public class NetworkController : ControllerBase
{
    private readonly ILogger<NetworkController> _logger;
	private readonly INetworkService _networkService;

    public NetworkController(
		ILogger<NetworkController> logger, 
		INetworkService networkService)
    {
        _logger = logger;
		_networkService = networkService;
    }

    [HttpPost("/customers")]
    public ActionResult GetDownstreamCustomers([FromBody]GetDownstreamCustomersRequest request)
    {
		return Ok();
    }
}
