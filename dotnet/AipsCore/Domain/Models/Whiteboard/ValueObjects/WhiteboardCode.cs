using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Abstract.ValueObject;
using AipsCore.Domain.Common.Validation.Rules;
using AipsCore.Domain.Models.Whiteboard.External;
using AipsCore.Domain.Models.Whiteboard.Validation;

namespace AipsCore.Domain.Models.Whiteboard.ValueObjects;

public record WhiteboardCode : AbstractValueObject
{
    public string CodeValue { get; init; }
    
    public WhiteboardCode(string CodeValue)
    {
        this.CodeValue = CodeValue;
        Validate();
    }

    private const int CodeLength = 8;

    protected override ICollection<IRule> GetValidationRules()
    {
        return
        [
            new ExactLength(CodeValue, CodeLength),
            new WhiteboardCodeCharsetRule(CodeValue)
        ];
    }

    public async static Task<WhiteboardCode> GenerateUniqueAsync(IWhiteboardRepository whiteboardRepository)
    {
        WhiteboardCode whiteboardCode;
        bool codeExists;

        do
        {
            whiteboardCode = Generate();

            codeExists = await whiteboardRepository.WhiteboardCodeExists(whiteboardCode);
        } while (codeExists);

        return whiteboardCode;
    }

    public static WhiteboardCode Generate()
    {
        var rng = new Random();
        char[] result = new char[8];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = (char)('0' + rng.Next(0, 10));
        }
        
        return new WhiteboardCode(new string(result));
    }
} 