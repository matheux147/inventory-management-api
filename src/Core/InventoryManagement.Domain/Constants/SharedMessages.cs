namespace InventoryManagement.Domain.Constants;

public static class SharedMessages
{
    public const string AmountCannotBeNegative = "Amount cannot be negative.";
    public const string CurrencyIsRequired = "Currency is required.";
    
    public const string CountryCodeIsRequired = "Country code is required.";
    public const string InvalidCountryCodeFormat = "Country code must be a valid ISO-3166 alpha-2 style code with 2 letters.";
    
    public const string CurrencyCodeIsRequired = "Currency code is required.";
    public const string InvalidCurrencyCodeFormat = "Currency code must be a valid ISO-4217 style code with 3 letters.";

    public const string CategoryShortcodeIsRequired = "Category shortcode is required.";
    public const string InvalidCategoryShortcodeFormat = "Category shortcode must contain only letters, numbers, underscore or hyphen.";

    public const string EmailIsRequired = "Email is required.";
    public const string InvalidEmailFormat = "Email is invalid.";
}
