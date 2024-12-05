namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetProductForViewDto
    {
        public ProductDto Product { get; set; }

        public string ProductCategoryName { get; set; }

        public string TaxRateName { get; set; }

        public ProductCategoryDto Category { get; set; }

        public BrandDto Brand { get; set; }
    }
}