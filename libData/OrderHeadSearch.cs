namespace libData
{
    public class OrderHeadSearch
    {
        public int IDOrderHead;
        public string OrderNumber;
        public string OrderCustomer;

        public OrderHeadSearch(int idOrderHead, string orderNumber, string OrderCustomer)
        {
            this.IDOrderHead = idOrderHead;
            this.OrderNumber = orderNumber;
            this.OrderCustomer = OrderCustomer;
        }
    }
}
