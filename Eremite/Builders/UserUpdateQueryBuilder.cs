using Eremite.Data.DiscordData;
using Eremite.Data;
using System.Text;
using Eremite.Base.Interfaces;

namespace Eremite.Builders
{
    /// <summary>
    /// Class used to build update queries to update only needed elements instead of all tables/rows
    /// </summary>
    internal class UserUpdateQueryBuilder : IQueryBuilder
    {
        public string QueryString { get; set; }
        public QueryElement[] ElementsUsed { get; set; }

        public string UserIdUsed { get; protected set; }

        public UserUpdateQueryBuilder(UserData user, params QueryElement[] elements)
        {
            ElementsUsed = elements;
            UserIdUsed = user.UserId;

            if (ElementsUsed.Contains(QueryElement.All))
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

            QueryString = $"UPDATE `users` SET {queryBuilder.ToString()} WHERE `userid`='{UserIdUsed}'";
        }

        public string Build() => QueryString;
    }
}
