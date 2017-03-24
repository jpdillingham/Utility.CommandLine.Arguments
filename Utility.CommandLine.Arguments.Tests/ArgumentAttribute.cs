using Xunit;

namespace Utility.CommandLine.Tests
{
    [Collection("ArgumentAttribute")]
    public class ArgumentAttribute
    {
        [Fact]
        public void Constructor()
        {
            CommandLine.ArgumentAttribute test = new CommandLine.ArgumentAttribute("name");

            Assert.Equal("name", test.Name);
        }
    }
}