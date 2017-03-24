using Xunit;

namespace Utility.CommandLine.Tests
{
    [Collection("ArgumentAttribute")]
    public class ArgumentAttribute
    {
        public void Constructor()
        {
            CommandLine.ArgumentAttribute test = new CommandLine.ArgumentAttribute("name");

            Assert.Equal("name", test.Name);
        }
    }
}