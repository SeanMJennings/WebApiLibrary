namespace Testing;

[TestFixture]
public partial class ExceptionHandlerShould
{
    [Test]
    public void allow_extending_with_custom_exception_mappings()
    {
        Given(an_api_with_custom_exception_handler);
        When(calling_the_api_that_throws_not_found_exception);
        Then(not_found_response_is_returned);
    }

    [Test]
    public void still_use_base_handlers_for_validation_exceptions()
    {
        Given(an_api_with_custom_exception_handler);
        When(calling_the_api_that_throws_validation_exception);
        Then(bad_request_response_is_returned_for_validation);
    }

    [Test]
    public void still_use_base_handlers_for_serialisation_exceptions()
    {
        Given(an_api_with_custom_exception_handler);
        When(calling_the_api_that_throws_serialisation_exception);
        Then(bad_request_response_is_returned_for_serialisation);
    }

    [Test]
    public void still_return_internal_server_error_for_unknown_exceptions()
    {
        Given(an_api_with_custom_exception_handler);
        When(calling_the_api_that_throws_unknown_exception);
        Then(internal_server_error_response_is_returned);
    }
}
