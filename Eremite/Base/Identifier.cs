
namespace Eremite.Base
{
    [Serializable]
    public class Identifier
    {
        public string identifier;
        public int content;

        public Identifier(string identifier, int content)
        {
            this.identifier = identifier;
            this.content = content;
        } 
    }
}
