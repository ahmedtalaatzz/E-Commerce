using System.Text.RegularExpressions;

namespace Infrastructure.DataSeeding.Helpers
{
    /// <summary>
    /// Helper class for language detection and translation
    /// </summary>
    public static class LanguageHelper
    {
        /// <summary>
        /// Checks if the text contains Arabic characters
        /// </summary>
        public static bool IsArabic(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            // Check if text contains Arabic characters (Unicode range: 0600–06FF)
            return Regex.IsMatch(text, @"[\u0600-\u06FF]");
        }

        /// <summary>
        /// Translates Arabic brand/partner names to English
        /// This is a simple mapping - in production, you might use a translation API
        /// </summary>
        public static string TranslateArabicToEnglish(string arabicName)
        {
            // Common brand/partner name translations
            var translations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "سامسونج", "Samsung" },
                { "أبل", "Apple" },
                { "هواوي", "Huawei" },
                { "شاومي", "Xiaomi" },
                { "نوكيا", "Nokia" },
                { "ال جي", "LG" },
                { "سوني", "Sony" },
                { "مايكروسوفت", "Microsoft" },
                { "جوجل", "Google" },
                { "أمازون", "Amazon" },
                { "نايكي", "Nike" },
                { "أديداس", "Adidas" },
                { "بوما", "Puma" },
                { "زارا", "Zara" },
                { "إتش آند إم", "H&M" },
                { "غوتشي", "Gucci" },
                { "شانيل", "Chanel" },
                { "ديور", "Dior" },
                { "برادا", "Prada" },
                { "لويس فيتون", "Louis Vuitton" },
                { "كوكاكولا", "Coca Cola" },
                { "بيبسي", "Pepsi" },
                { "ماكدونالدز", "McDonalds" },
                { "كنتاكي", "KFC" },
                { "برجر كينج", "Burger King" },
                { "ستاربكس", "Starbucks" },
                { "كارفور", "Carrefour" },
                { "ايكيا", "IKEA" },
                { "تويوتا", "Toyota" },
                { "هوندا", "Honda" },
                { "مرسيدس", "Mercedes" },
                { "بي إم دبليو", "BMW" },
                { "أودي", "Audi" },
                { "فورد", "Ford" },
                { "شيفروليه", "Chevrolet" },
                { "نيسان", "Nissan" },
                { "كيا", "Kia" },
                { "هيونداي", "Hyundai" }
            };

            // Try to find exact match
            if (translations.TryGetValue(arabicName.Trim(), out var translation))
                return translation;

            // If no match, use romanization or return as-is
            // In production, you might use a translation API here
            return RomanizeArabic(arabicName);
        }

        /// <summary>
        /// Translates English brand/partner names to Arabic
        /// This is a simple mapping - in production, you might use a translation API
        /// </summary>
        public static string TranslateEnglishToArabic(string englishName)
        {
            // Common brand/partner name translations
            var translations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Samsung", "سامسونج" },
                { "Apple", "أبل" },
                { "Huawei", "هواوي" },
                { "Xiaomi", "شاومي" },
                { "Nokia", "نوكيا" },
                { "LG", "ال جي" },
                { "Sony", "سوني" },
                { "Microsoft", "مايكروسوفت" },
                { "Google", "جوجل" },
                { "Amazon", "أمازون" },
                { "Nike", "نايكي" },
                { "Adidas", "أديداس" },
                { "Puma", "بوما" },
                { "Zara", "زارا" },
                { "H&M", "إتش آند إم" },
                { "Gucci", "غوتشي" },
                { "Chanel", "شانيل" },
                { "Dior", "ديور" },
                { "Prada", "برادا" },
                { "Louis Vuitton", "لويس فيتون" },
                { "Coca Cola", "كوكاكولا" },
                { "Pepsi", "بيبسي" },
                { "McDonalds", "ماكدونالدز" },
                { "KFC", "كنتاكي" },
                { "Burger King", "برجر كينج" },
                { "Starbucks", "ستاربكس" },
                { "Carrefour", "كارفور" },
                { "IKEA", "ايكيا" },
                { "Toyota", "تويوتا" },
                { "Honda", "هوندا" },
                { "Mercedes", "مرسيدس" },
                { "BMW", "بي إم دبليو" },
                { "Audi", "أودي" },
                { "Ford", "فورد" },
                { "Chevrolet", "شيفروليه" },
                { "Nissan", "نيسان" },
                { "Kia", "كيا" },
                { "Hyundai", "هيونداي" }
            };

            // Try to find exact match
            if (translations.TryGetValue(englishName.Trim(), out var translation))
                return translation;

            // If no match, return transliteration
            // In production, you might use a translation API here
            return englishName; // Keep as-is if no translation found
        }

        /// <summary>
        /// Simple romanization of Arabic text (fallback)
        /// </summary>
        private static string RomanizeArabic(string arabicText)
        {
            // This is a very basic romanization - in production use a proper library
            // For now, we'll just return the Arabic text as-is
            // A better approach would be to use a romanization library or API
            return arabicText;
        }
    }
}
