using FluentAssertions;
using Notes.Application.Notes.Commands;
using Notes.Application.Notes.Queries.Dtos;
using Notes.IntegrationTests.Base;
using Notes.IntegrationTests.Extensions;
using System.Net;

namespace Notes.IntegrationTests.Notes;

public class NotesControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string RequestUri = @"/api/notes";
    private readonly HttpClient _autherizedClient;
    private readonly HttpClient _unautherizedClient;

    public NotesControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _autherizedClient = factory.CreateClient();
        _unautherizedClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllNotesWithoutTokenReturnsUnautherized()
    {
        //RemoveBearerTokenAsync(_unautherizedClient);

        var response = await _unautherizedClient.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAllNotesWithTokenReturnOkResponseAsync()
    {
        await AuthenticateAsync(_autherizedClient);

        var response = await _autherizedClient.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllNotesWithTokenReturnsAllCreatedNoteAsync()
    {
        await AuthenticateAsync(_autherizedClient);

        await CreateNoteAsync();

        var response = await _autherizedClient.GetAsync(RequestUri);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        (await response.ReadAsAsync<List<NotesDto>>()).Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateNoteWithTokenReturnsOkResponseAsync()
    {
        await AuthenticateAsync(_autherizedClient);

        var response = await CreateNoteAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

    }

    [Fact]
    public async Task UpdateNoteWithTokenReturnsOkResponseAsync()
    {
        await AuthenticateAsync(_autherizedClient);
        var response = await CreateNoteAsync();
        int.TryParse((await response.Content.ReadAsStringAsync()), out var id);
        var randomNote = GetRandomObjectAs<SaveNoteCommand>();
        randomNote.Id = id;
        var httpResponse = await _autherizedClient.PostAsJsonAsync(RequestUri, randomNote);

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task<HttpResponseMessage> CreateNoteAsync()
    {
        var request = GetRandomObjectAs<SaveNoteCommand>();
        request.Id = 0;
        request.UserId = _userId;

        var response = await _autherizedClient.PostAsJsonAsync(RequestUri, request);
        return response;
    }
}
