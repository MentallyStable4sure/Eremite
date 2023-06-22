
namespace Eremite.Base
{
    [Serializable]
    public class BaseIdentifier
    {
        public string identifier;
        public object content;

        public BaseIdentifier(string identifier, object content)
        {
            this.identifier = identifier;
            this.content = content;
        }
    }
}
