using AipsCore.Domain.Abstract.Rule;
using AipsCore.Domain.Common.Validation;

namespace AipsTestsUtility;

public static class AssertUtility
{
    public static void AssertHasBrokenExactRule<TRule>(ValidationException exception)
        where TRule : IRuleMetadata
    {
        var expectedCode = TRule.ErrorCodeString;

        var receivedCodes = exception.ValidationErrors.Select(e => e.Code).ToList();
        var receivedCodesString = string.Join(", ", receivedCodes.Select(c => $"'{c}'"));
        
        Assert.Multiple(() =>
        {
            Assert.That(receivedCodes.Count, Is.EqualTo(1), 
                $"Expected exactly 1 validation error, but found {receivedCodes.Count}. " +
                $"Received codes: [{receivedCodesString}]");

            Assert.That(receivedCodes, Contains.Item(expectedCode), 
                $"Expected validation error code '{expectedCode}' was not found. " +
                $"Received codes: [{receivedCodesString}]");
        });
    }
    
    public static void AssertHasBrokenRule<TRule>(ValidationException exception)
        where TRule : IRuleMetadata
    {
        var expectedCode = TRule.ErrorCodeString;

        var receivedCodes = exception.ValidationErrors.Select(e => e.Code).ToList();
        var receivedCodesString = string.Join(", ", receivedCodes.Select(c => $"'{c}'"));
        
        Assert.Multiple(() =>
        {
            Assert.That(receivedCodes, Contains.Item(expectedCode), 
                $"Expected validation error code '{expectedCode}' was not found. " +
                $"Received codes: [{receivedCodesString}]");
        });
    }
}