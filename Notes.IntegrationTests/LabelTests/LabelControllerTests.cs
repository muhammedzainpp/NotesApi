using FluentAssertions;
using Notes.Application.Labels.Commands;
using Notes.Application.Labels.Queries.Dtos;
using Notes.IntegrationTests.Base;
using Notes.IntegrationTests.Extensions;
using System.Net;

namespace Notes.IntegrationTests.LabelTests;

public class LabelControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string RequestUri = @"/api/label";
    private readonly HttpClient _autherizedClient;
    private readonly HttpClient _unautherizedClient;

    public LabelControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _autherizedClient = factory.CreateClient();
        _unautherizedClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllLabelsWithoutTokenReturnsUnautherized()
    {
        var response = await _unautherizedClient.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAllLabelsWithTokenReturnOkResponseAsync()
    {
        await AuthenticateAsync(_autherizedClient);

        var response = await _autherizedClient.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllLabelsReturnsAllCreatedLabelAsync()
    {
        await AuthenticateAsync(_autherizedClient);

        await CreateLabelAsync();

        var response = await _autherizedClient.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        List<GetLabelsQueryDto>? labels = (await response.ReadAsAsync<List<GetLabelsQueryDto>>());
        labels.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ShouldUpdateLabelWhenExistingLabelModified()
    {
        await AuthenticateAsync(_autherizedClient);
        var response = await CreateLabelAsync();
        _ = int.TryParse(await response.Content.ReadAsStringAsync(), out var id);
        var randomLabel = GetRandomObjectAs<SaveLabelCommand>();
        randomLabel.Id = id;
        var httpResponse = await _autherizedClient.PostAsJsonAsync(RequestUri, randomLabel);

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ShouldBeAbleToCreateLabelAsync()
    {
        await AuthenticateAsync(_autherizedClient);

        var response = await CreateLabelAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteLabelWithTokenReturnsOkResponseAsync()
    {
        await AuthenticateAsync(_autherizedClient);

        var response = await CreateLabelAsync();
        int.TryParse(await response.Content.ReadAsStringAsync(), out var id);
        var deletedLabelResponse = await _autherizedClient.DeleteAsync($"{RequestUri}/{id}");
        deletedLabelResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }



    private async Task<HttpResponseMessage> CreateLabelAsync()
    {
        var request = GetRandomObjectAs<SaveLabelCommand>();
        request.Id = 0;
        request.UserId = _userId;

        var response = await _autherizedClient.PostAsJsonAsync(RequestUri, request);
        return response;
    }
}
