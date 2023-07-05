
namespace Eremite.Data
{
    [Serializable]
    public class OrderData
    {
        public string path = "order/create_order";
        public GenshinOrderData data;
        public string partnerOrderId = string.Empty;
    }
}
