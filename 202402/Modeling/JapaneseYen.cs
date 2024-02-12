using System.ComponentModel.DataAnnotations;
namespace _202402.Modeling;

public record JapaneseYen(
    [property: Range(0, 99999999)]
    int Value)
{
    public static JapaneseYen Empty => new(0);
    public static (JapaneseYen, string) Parse(string arg1)
    {
        var errorMessage = string.Empty;
        if (!int.TryParse(arg1, out var x))
        {
            errorMessage += "所持金が数値ではありません。";
        }
        var validationResults = new List<ValidationResult>();
        var yen = new JapaneseYen(x);
        Validator.TryValidateObject(yen, new ValidationContext(yen), validationResults, true);
        if (validationResults.Any())
        {
            errorMessage += string.Join(Environment.NewLine, validationResults.Select(v => v.ErrorMessage));
        }
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return (Empty, errorMessage);
        }
        return (new JapaneseYen(x), string.Empty);
    }
}
