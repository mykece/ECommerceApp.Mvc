namespace ETicaret.UI.Models
{
    public class PrivacyViewModel
    {
        public int Height { get; set; }   // Boy (cm)
        public int Weight { get; set; }   // Kilo (kg)
        public string ClothingType { get; set; }   // Seçilen Ürün Türü (Gömlek, Tişört, Pantolon)
        public string ResponseMessage { get; set; }   // ChatGPT'den dönen mesaj
    }
}
