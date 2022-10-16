namespace DownstreamCustomers.Models.Incoming;

public class GetDownstreamCustomersRequest
{
	public Network Network { get; set; }
	public int SelectedNode { get; set; } 
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
	public List<Branch> Branches { get; set; }
	public List<Customer> Customers { get; set; }
}