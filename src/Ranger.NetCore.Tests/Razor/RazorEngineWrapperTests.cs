using Ranger.NetCore.Models;
using Ranger.NetCore.RazorHtml;
using Xunit;

namespace Ranger.NetCore.Tests.Razor
{
    public class RazorEngineWrapperTests
    {
        [Fact]
        public void Should_Generate_Html_With_Model_Provided()
        {
            var wrapper= new RazorEngineWrapper();
            var result = wrapper.Run("Hello @Model.Release", new ReleaseNoteViewModel() {Release = "1.912.4"});
            Assert.Equal(result , "Hello 1.912.4");
        }
    }
}
