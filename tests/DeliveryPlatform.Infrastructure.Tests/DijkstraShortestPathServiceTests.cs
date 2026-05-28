/*using DeliveryPlatform.Infrastructure.Routing.Graph;
using DeliveryPlatform.Infrastructure.Routing.ShortestPath;
using FluentAssertions;

namespace DeliveryPlatform.Infrastructure.Tests;

public class DijkstraShortestPathServiceTests
{
    private readonly Guid A = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private readonly Guid B = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private readonly Guid C = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

    [Fact]
    public async Task RunFrom_ShouldCalculateShortestDistances()
    {
        // Arrange
        var provider = new TestRoadGraphProvider();
        var service = new DijkstraShortestPathService(provider);

        // Act
        var result = await service.RunFromAsync(A, CancellationToken.None);

        // Assert
        result.Distances[A].Should().Be(0);
        result.Distances[B].Should().Be(5);
        result.Distances[C].Should().Be(10); // через B, а не напрямую
    }

    [Fact]
    public async Task RunFrom_ShouldSetCorrectParents()
    {
        // Arrange
        var provider = new TestRoadGraphProvider();
        var service = new DijkstraShortestPathService(provider);

        // Act
        var result = await service.RunFromAsync(A, CancellationToken.None);

        // Assert
        result.Parents[B].Should().Be(A);
        result.Parents[C].Should().Be(B);
    }
}*/

