using DownstreamCustomers.Models.Incoming;

namespace DownstreamCustomers.Services;

public interface INetworkService
{
	int GetDownstreamCustomers(Network network, int startNode);
}

public class NetworkService : INetworkService
{
	private readonly ILogger<NetworkService> _logger;

	public NetworkService(ILogger<NetworkService> logger)
	{
		_logger = logger;
	}

	public int GetDownstreamCustomers(Network network, int startNode)
	{
		var nodeLookup = CreateNodeLookup(network.Branches);

		if (!nodeLookup.ContainsKey(startNode))
		{
			throw new BadHttpRequestException("Provided start node does not exist in the network.");
		}

		var downstreamNodes = GetDownstreamNodes(nodeLookup, startNode);

		var customerLookup = CreateCustomerLookup(network.Customers);
		var totalCustomers = downstreamNodes.Aggregate(0, (a, b) => 
			customerLookup.GetValueOrDefault(b, 0) + a);

		return totalCustomers;
	}

	public Dictionary<int, List<int>> CreateNodeLookup(List<Branch> branches)
	{
		var nodeLookup = new Dictionary<int, List<int>>();
		foreach (Branch branch in branches)
		{
			var key = branch.StartNode;
			var value = branch.EndNode;

			if (nodeLookup.ContainsKey(key))
			{
				nodeLookup[key].Add(value);
			}
			else 
			{
				nodeLookup[key] = new List<int> { value };
			}
		}
		return nodeLookup;
	}

	public Dictionary<int, int> CreateCustomerLookup(List<Customer> customers)
	{
		var customerLookup = new Dictionary<int, int>();
		foreach (Customer customer in customers)
		{
			customerLookup[customer.Node] = customer.NumberOfCustomers;
		}
		return customerLookup;
	}

	public List<int> GetDownstreamNodes(Dictionary<int, List<int>> nodeLookup, int startNode)
	{
		if (nodeLookup.ContainsKey(startNode))
		{
			var newList = new List<int>();
			foreach (int node in nodeLookup[startNode])
			{
				if (nodeLookup.ContainsKey(node))
				{
					newList.Add(node);
				}
				newList.AddRange(GetDownstreamNodes(nodeLookup, node));
			}
			return newList;
		}
		else
		{
			return new List<int> { startNode };
		}
	}
}