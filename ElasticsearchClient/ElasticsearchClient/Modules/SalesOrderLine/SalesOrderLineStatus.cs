namespace ElasticsearchClient.Modules.SalesOrderLine;

public enum SalesOrderLineStatus
{
    Exception = 0,
    Pending = 2,
    Vpa = 1,
    NeedsApproval = 3,
    AwaitingApproval = 4,
    Approved = 5,
    Rejected = 6,
    NotApplicable
}