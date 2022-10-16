using Moq;
using DownstreamCustomers.Services;
using DownstreamCustomers.Controllers;
using Microsoft.Extensions.Logging;
using DownstreamCustomers.Models.Incoming;
using Microsoft.AspNetCore.Mvc;

namespace DownstreamCustomers.Test;

public class CustomersControllerTests
{
    [Fact]
    public void ShouldRespondWith200OK()
    {
		var mockNetworkService = new Mock<INetworkService>();
		var mockLogger = new Mock<ILogger<NetworkController>>();
		var controller = new NetworkController(
			mockLogger.Object,
			mockNetworkService.Object
		);
		var mockRequestBody = new GetDownstreamCustomersRequest
		{
			Network = new Network {
				Branches = {},
				Customers = {}
			},
			SelectedNode = 2
		};

		var result = controller.GetDownstreamCustomers(mockRequestBody);

		Assert.IsType<OkResult>(result);
    }
}