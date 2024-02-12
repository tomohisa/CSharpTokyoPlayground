using System.ComponentModel.DataAnnotations;
namespace _202402.Modeling;

public record ItemAmount(
    [property: Range(1, 100)]
    int Value)
{
    public static ItemAmount Empty => new(0);
    public static (ItemAmount, string) Parse(string arg1)
    {
        var errorMessage = string.Empty;
        if (!int.TryParse(arg1, out var x))
        {
            errorMessage += "個数が数値ではありません。";
        }
        var validationResults = new List<ValidationResult>();
        var amount = new ItemAmount(x);
        Validator.TryValidateObject(amount, new ValidationContext(amount), validationResults, true);
        if (validationResults.Any())
        {
            errorMessage += string.Join(Environment.NewLine, validationResults.Select(v => v.ErrorMessage));
        }
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return (Empty, errorMessage);
        }
        return (amount, string.Empty);
    }
}
