namespace Tradelens.Unit.Tests.Services;

/// <summary>
/// Things to test to verify max pain and cash value calculations are working properly:
/// That there are a total of three objects in the response? Call Sums, Put Sums, and Max Pain strike.
/// That the length of the Call Sums object is equal to the Put Sums.
/// That the Max Pain strike is within the range of the list of expirations.
/// That there is only one Max Pain value. (is it necessary if Max Pain value is defined as double type?)
/// And more...
/// </summary>
/// 
/// <remarks>
/// Currently, the business logic exposes multiple methods. Should there only be one, and the rest be
/// private methods (implementation details)? According to the Microsoft docs:
/// https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
/// we may be better served concerning the end result and how that should be sent to the outer layers.
///
/// Is there a possible scenario where an options chain can yield call and put cash sum values such that there
/// is more than one max pain strike? Consider creating a valid options chain that mocks the response shape where
/// the cash values work out to this sort of result. If possible, then it needs to be handled as an edge case
/// in the business logic.
/// </remarks>
public class MaxPainCalculatorTests
{
    /// <summary>
    /// Make sure the length of the computed call cash values is equal to the computed put cash values for an options chain.
    /// In an options chain there is always a put strike price for each call so cash values should reflect that.
    /// </summary>
    [Fact]
    public void CallAndPutCashValues_OptionsChainOne_ShouldReturnSameLength()
    {
        // Arrange
        // get and store call cash sums and put cash sums
        
        // Act
        // compare their lengths
        
        // Assert
        // that they are the same
    }
}