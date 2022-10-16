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

		Assert.IsType<OkObjectResult>(result);
    }

	[Fact]
	public void ShouldReturnCorrectNumberOfCustomers()
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

		var result = controller.GetDownstreamCustomers(mockRequestBody);

		var okResult = Assert.IsType<OkObjectResult>(result);
		var responseBody = Assert.IsType<GetDownstreamCustomersResponse>(okResult.Value);
		Assert.Equal(responseBody.NumberOfCustomers, 10);
	}
}