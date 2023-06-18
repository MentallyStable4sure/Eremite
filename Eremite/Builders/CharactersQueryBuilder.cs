using Eremite.Base.Interfaces;
using Eremite.Data;
using System.Text;

namespace Eremite.Builders
{
    internal class CharactersQueryBuilder : IQueryBuilder
    {
        public string QueryString { get; set; }
        public QueryElement[] ElementsUsed { get; set; }

        public const string CharactersTable = "characters";

        /// <summary>
        /// Constructor for getting characters from the table as a query string (dont forget <see cref="Build"/> after constructor)
        /// </summary>
        /// <param name="charactersId">Put 0 to get all the characters, or put ID's of characters u want to get from the table</param>
        public CharactersQueryBuilder(params int[] charactersId)
        {
            ElementsUsed = new QueryElement[1] { QueryElement.Characters };
            QueryString = GetAllCharacters();

            if (charactersId.Contains(0)) return;

            StringBuilder queryBuilder = new StringBuilder($"{QueryString} WHERE `CharacterId`=");
            for (int i = 0; i < charactersId.Length; i++)
            {
                queryBuilder.Append(charactersId[i]);
                if (i < charactersId.Length - 1) queryBuilder.Append(",");
            }

            QueryString = queryBuilder.ToString();
        }

        private string GetAllCharacters() => $"SELECT * FROM `{CharactersTable}`";

        public string Build() => QueryString;
    }
}
