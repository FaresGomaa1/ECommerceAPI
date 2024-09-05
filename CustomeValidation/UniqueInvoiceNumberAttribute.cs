using System.ComponentModel.DataAnnotations;
using System.Linq;

public class UniqueInvoiceNumberAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var invoiceNumber = (string)value;  // Cast to string

        // Resolve the DbContext or service from the service provider
        var context = (ECommerceContext)validationContext.GetService(typeof(ECommerceContext));

        // Check for uniqueness in your data source
        bool exists = context.Orders.Any(o => o.InvoiceNumber == invoiceNumber);

        if (exists)
        {
            return new ValidationResult("The Invoice Number must be unique.");
        }

        return ValidationResult.Success;
    }
}
