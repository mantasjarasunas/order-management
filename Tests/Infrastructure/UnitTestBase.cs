using FluentAssertions;
using Moq;

namespace Tests.Infrastructure
{
    public class UnitTestBase
    {
        protected static T IsDeep<T>(T expected)
        {
            bool Validate(T actual)
            {
                actual.Should().BeEquivalentTo(expected);
                return true;
            }

            return Match.Create<T>(Validate);
        }
    }
}