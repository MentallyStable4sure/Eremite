using Eremite.Data;

namespace Eremite.Base.Interfaces
{
    public interface IQueryBuilder
    {
        public string QueryString { get; protected set; }
        public QueryElement[] ElementsUsed { get; protected set; }

        /// <summary>
        /// Builds a query into an actual string from QueryElements, returns table-ready query
        /// </summary>
        /// <returns>sql table-ready query</returns>
        public string Build();
    }
}
