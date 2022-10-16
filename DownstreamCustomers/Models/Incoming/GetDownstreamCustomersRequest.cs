namespace DownstreamCustomers.Models.Incoming;

public class GetDownstreamCustomersRequest
{
	public Network Network = new Network();
	public int SelectedNode; 
}

public class Branch
{
	public int StartNode { get; set; }
	public int EndNode { get; set; }
}

public class Customer
{
	public int Node { get; set; }
	public int NumberOfCustomers { get; set; }
}

public class Network
{
	public List<Branch> Branches { get; set; } = new List<Branch>();
	public List<Customer> Customers { get; set; } = new List<Customer>();
}