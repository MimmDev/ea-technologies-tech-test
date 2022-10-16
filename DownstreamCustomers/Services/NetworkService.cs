using DownstreamCustomers.Models.Incoming;

namespace DownstreamCustomers.Services;

public interface INetworkService
{
	Task<int> GetDownstreamCustomers(Network network, int startNode);
}

public class NetworkService : INetworkService
{
	public Task<int> GetDownstreamCustomers(Network network, int startNode)
	{
		throw new NotImplementedException();
	}
}