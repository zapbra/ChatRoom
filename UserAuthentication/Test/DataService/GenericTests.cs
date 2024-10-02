using FluentAssertions;
using Xunit;

namespace UserAuthentication.Test.DataService
{
    public class GenericTests
    {

        [Fact]
        public void Run_Generic_Test()
        {
            int x = 5;

            x.Should().Be(5);
        }
    }
}
