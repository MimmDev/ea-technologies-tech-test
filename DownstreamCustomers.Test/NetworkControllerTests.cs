using Moq;
using DownstreamCustomers.Services;
using DownstreamCustomers.Controllers;
using Microsoft.Extensions.Logging;
using DownstreamCustomers.Models.Incoming;
using Microsoft.AspNetCore.Mvc;
using DownstreamCustomers.Models.Outgoing;

namespace DownstreamCustomers.Test;

public class CustomersControllerTests
{
	private GetDownstreamCustomersRequest _requestBody;
	private Mock<INetworkService> _networkService;
	private Mock<ILogger<NetworkController>> _networkLogger;

	public CustomersControllerTests()
	{
		_networkService = new Mock<INetworkService>();
		_networkLogger = new Mock<ILogger<NetworkController>>();
		_requestBody = new GetDownstreamCustomersRequest {
			Network = new Network {
				Branches = new List<Branch> {
					new Branch { StartNode = 10, EndNode = 20 },
					new Branch { StartNode = 20, EndNode = 30 },
					new Branch { StartNode = 30, EndNode = 50 },
					new Branch { StartNode = 50, EndNode = 60 },
					new Branch { StartNode = 50, EndNode = 90 },
					new Branch { StartNode = 60, EndNode = 40 },
					new Branch { StartNode = 70, EndNode = 80 }
				},
				Customers = new List<Customer> {
					new Customer { Node = 30, NumberOfCustomers = 8 },
					new Customer { Node = 40, NumberOfCustomers = 3 },
					new Customer { Node = 60, NumberOfCustomers = 2 },
					new Customer { Node = 70, NumberOfCustomers = 1 },
					new Customer { Node = 80, NumberOfCustomers = 3 },
					new Customer { Node = 90, NumberOfCustomers = 5 }
				}
			},
			SelectedNode = 50
		};
	}

    [Fact]
    public async Task ShouldRespondWith200OK()
    {
		var controller = new NetworkController(
			_networkLogger.Object,
			_networkService.Object
		);
		var mockRequestBody = new GetDownstreamCustomersRequest
		{
			Network = new Network {
				Branches = {},
				Customers = {}
			},
			SelectedNode = 2
		};

		var result = await controller.GetDownstreamCustomers(mockRequestBody);

		Assert.IsType<OkObjectResult>(result);
    }

	[Fact]
	public async Task ShouldReturnCorrectNumberOfCustomers()
	{
		var controller = new NetworkController(
			_networkLogger.Object,
			_networkService.Object
		);
		_networkService.Setup(m => m.GetDownstreamCustomers(_requestBody.Network, _requestBody.SelectedNode))
			.Returns(10);

		var result = await controller.GetDownstreamCustomers(_requestBody);

		var okResult = Assert.IsType<OkObjectResult>(result);
		var responseBody = Assert.IsType<GetDownstreamCustomersResponse>(okResult.Value);
		Assert.Equal(responseBody.NumberOfCustomers, 10);
	}

	[Fact]
	public async Task ShouldCallNetworkService()
	{
		var controller = new NetworkController(
			_networkLogger.Object,
			_networkService.Object
		);

		await controller.GetDownstreamCustomers(_requestBody);

		_networkService.Verify(mock => mock.GetDownstreamCustomers(It.IsAny<Network>(), It.IsAny<int>()), Times.Once);
	}
}