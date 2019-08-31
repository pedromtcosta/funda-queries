namespace FundaQueries.Services.ResponseModels
{
    public class QueryResponse
    {
        public ListedObject[] Objects { get; set; }
        public Paging Paging { get; set; }
    }
}
