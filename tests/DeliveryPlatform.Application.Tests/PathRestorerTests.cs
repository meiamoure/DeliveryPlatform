/*using DeliveryPlatform.Application.Routing.ShortestPath;
using FluentAssertions;

namespace DeliveryPlatform.Application.Tests;

public class PathRestorerTests
{
    [Fact]
    public void RestorePath_ShouldBuildCorrectPath()
    {
        // Arrange
        var A = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var B = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var C = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        var parents = new Dictionary<Guid, Guid?>
        {
            [A] = null,
            [B] = A,
            [C] = B
        };

        // Act
        var path = PathRestorer.RestorePath(C, parents);

        // Assert
        path.Should().Equal(A, B, C);
    }
}*/
