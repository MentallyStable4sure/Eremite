
using Eremite.Data;
using Eremite.Data.DiscordData;
using System.Text;

namespace Eremite.Services
{
    /// <summary>
    /// Class used to build update queries to update only needed elements instead of all tables/rows
    /// </summary>
    internal class QueryBuilder
    {
        public string QueryString { get; protected set; }
        public QueryElement[] ElementsUsed { get; protected set; }

        public string UserIdUsed { get; protected set; }

        public QueryBuilder(UserData user, params QueryElement[] elements)
        {
            ElementsUsed = elements;
            UserIdUsed = user.UserId;

            if(ElementsUsed.Contains(QueryElement.All))
            {
                //no need to go down below since we already know query will include all modules to update
                QueryString = QueryElement.All.GetCorrespondingQuery(user);
                return;
            }

            StringBuilder queryBuilder = new StringBuilder("");

            for (int i = 0; i < ElementsUsed.Length; i++)
            {
                queryBuilder.Append(ElementsUsed[i].GetCorrespondingQuery(user));
                if (i < ElementsUsed.Length - 1) queryBuilder.Append(",");
            }

            QueryString = queryBuilder.ToString();
        }

        public string BuildUpdateQuery() => $"UPDATE `users` SET {QueryString} WHERE `userid`='{UserIdUsed}'";
    }
}
