using TodoAppTakeHome.Api.DTOs;
using TodoAppTakeHome.Api.Validators;
using Xunit;

namespace TodoAppTakeHome.Tests.Validators;

public class ValidatorsTests
{
    [Fact]
    public void CreateTaskValidator_Should_Fail_When_Title_Is_Empty()
    {
        var validator = new CreateTaskRequestValidator();
        var request = new CreateTaskRequest { Title = "" };
        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateTaskValidator_Should_Fail_When_Title_Invalid(string? title)
    {
        var validator = new CreateTaskRequestValidator();
        var request = new CreateTaskRequest { Title = title! };
        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void CreateTaskValidator_Should_Pass_With_Valid_Title()
    {
        var validator = new CreateTaskRequestValidator();
        var request = new CreateTaskRequest { Title = "Valid Task" };
        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void UpdateTaskValidator_Should_Fail_When_Title_Too_Long()
    {
        var validator = new UpdateTaskRequestValidator();
        var request = new UpdateTaskRequest { Title = new string('A', 201) };
        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void UpdateTaskValidator_Should_Pass_When_Title_Is_Valid()
    {
        var validator = new UpdateTaskRequestValidator();
        var request = new UpdateTaskRequest { Title = "Valid Title" };
        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }
}
