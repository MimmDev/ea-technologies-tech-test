using Moq;
using DownstreamCustomers.Services;
using Microsoft.Extensions.Logging;
using DownstreamCustomers.Models.Incoming;
using Microsoft.AspNetCore.Http;

namespace DownstreamCustomers.Test;

public class NetworkServiceTests
{
	private Network _network;
	private Mock<ILogger<NetworkService>> _networkLogger;

	public NetworkServiceTests()
	{
		_networkLogger = new Mock<ILogger<NetworkService>>();
		_network = new Network {
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
		};
	}

	[Fact]
	public void ShouldReturnCorrectNumberOfCustomers()
	{
		var service = new NetworkService(_networkLogger.Object);

		var result = service.GetDownstreamCustomers(_network, 50);

		Assert.Equal(10, result);
	}

	[Fact]
	public void ShouldThrowBadRequestExceptionIfStartNodeInvalid()
	{
		var service = new NetworkService(_networkLogger.Object);

		Assert.Throws<BadHttpRequestException>(() => service.GetDownstreamCustomers(_network, 150));
	}

	[Fact]
	public void CreateNodeLookup_ShouldReturnValidDictionary()
	{
		var service = new NetworkService(_networkLogger.Object);

		var result = service.CreateNodeLookup(_network.Branches);

		Assert.True(
			result[10][0] == 20 &&
			result[50][0] == 60 &&
			result[50][1] == 90
		);
	}

	[Fact]
	public void CreateCustomerLookup_ShouldReturnValidDictionary()
	{
		var service = new NetworkService(_networkLogger.Object);

		var result = service.CreateCustomerLookup(_network.Customers);

		Assert.True(
			result[40] == 3 &&
			result[80] == 3 &&
			result[90] == 5
		);
	}

	[Fact]
	public void GetDownstreamNodes_ShouldReturnAllDownstreamNodes()
	{
		var service = new NetworkService(_networkLogger.Object);

		var nodeLookup = new Dictionary<int, List<int>> {
			{10, new List<int> { 20, 30 }},
			{20, new List<int> { 40, 50 }},
			{50, new List<int> { 60 }}
		};
		var result = service.GetDownstreamNodes(nodeLookup, 20);

		Assert.Equal(3, result.Count);
		Assert.True(
			result[0] == 40 &&
			result[1] == 50 &&
			result[2] == 60
		);
	}
}