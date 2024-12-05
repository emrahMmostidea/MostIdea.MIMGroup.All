namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetOrderItemForViewDto
    {
        public OrderItemDto OrderItem { get; set; }

        public string ProductName { get; set; }

        public string OrderOrderNo { get; set; }

        public string ProductImage { get; set; }
        
        public decimal TaxRate { get; set; }

        public string TaxName { get; set; }

    }
}