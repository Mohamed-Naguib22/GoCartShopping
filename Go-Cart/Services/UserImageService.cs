using Go_Cart.Interfaces;
using Go_Cart.Models;

namespace GoCart
{
    public class UserImageService : IImageService<ApplicationUser>
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public void SaveImage(IFormFile? imgFile, ApplicationUser user)
        {
            if (imgFile == null)
            {
                user.ImgUrl = "\\images\\No_Image.png";
            }
            else
            {
                string imgExtension = Path.GetExtension(imgFile.FileName);
                Guid imgGuid = Guid.NewGuid();
                string imgName = imgGuid + imgExtension;
                string imgUrl = "\\images\\" + imgName;

                string imgPath = _webHostEnvironment.WebRootPath + imgUrl;
                using (var imgStream = new FileStream(imgPath, FileMode.Create))
                {
                    imgFile.CopyTo(imgStream);
                }
                user.ImgUrl = imgUrl;
            }
        }
        public void DeleteImage(ApplicationUser user)
        {
            if (!string.IsNullOrEmpty(user.ImgUrl))
            {
                var imgOldPath = _webHostEnvironment.WebRootPath + user.ImgUrl;
                if (File.Exists(imgOldPath))
                {
                    File.Delete(imgOldPath);
                }
            }
        }
        public void SetOptionalImage(IFormFile? imgFile, int productId)
        {
        }
    }
}
