using Go_Cart.Data;
using Go_Cart.Interfaces;
using Go_Cart.Models;

namespace GoCart
{
    public class ProductImageService : IImageService<Product>
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        public ProductImageService(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }
        public void SaveImage(IFormFile? imgFile, Product product)
        {
            //if (imgFile == null)
            //{
            //    product.ImgUrl = "\\images\\No_Image.png";
            //}
            //else
            //{
            //    string imgExtension = Path.GetExtension(imgFile.FileName);
            //    Guid imgGuid = Guid.NewGuid();
            //    string imgName = imgGuid + imgExtension;
            //    string imgUrl = "\\images\\" + imgName;

            //    product.ImgUrl = imgUrl;

            //    string imgPath = _webHostEnvironment.WebRootPath + imgUrl;
            //    using (var imgStream = new FileStream(imgPath, FileMode.Create))
            //    {
            //        imgFile.CopyTo(imgStream);
            //    }
            //}
        }
        public void SetOptionalImage(IFormFile? imgFile, int productId)
        {
            string imgExtension = Path.GetExtension(imgFile.FileName);
            Guid imgGuid = Guid.NewGuid();
            string imgName = imgGuid + imgExtension;
            string imgUrl = "\\images\\" + imgName;

            var productImage = new ProductImage
            {
                ImgUrl = imgUrl,
                ProductId = productId,
            };

            _context.ProductImages.Add(productImage);
            _context.SaveChanges();

            string imgPath = _webHostEnvironment.WebRootPath + imgUrl;
            using var imgStream = new FileStream(imgPath, FileMode.Create);
            imgFile.CopyTo(imgStream);
        }
        public void DeleteImage(Product product)
        {
            //if (!string.IsNullOrEmpty(product.ImgUrl))
            //{
            //    var imgOldPath = _webHostEnvironment.WebRootPath + product.ImgUrl;
            //    if (File.Exists(imgOldPath))
            //    {
            //        File.Delete(imgOldPath);
            //    }
            //}
        }
    }
}
