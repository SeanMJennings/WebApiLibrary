namespace Testing;

[TestFixture]
public partial class WebApiShould
{
    [Test]
    public void map_controllers_by_default()
    {
        Given(an_api);
        When(calling_the_api_test_controller);
        Then(the_controller_was_mapped);
    }

    [Test]
    public void allow_registering_of_services()
    {
        Given(an_api);
        When(calling_the_api_test_controller_that_relies_on_a_service);
        Then(the_service_is_available_for_injection);
    }
    
    [Test]
    public void provide_default_serialisation()
    {
        Given(an_api);
        When(calling_the_api_test_controller);
        Then(default_serialisation_is_evident_in_the_response);
    }

    [Test]
    public void allow_registering_custom_converters()
    {
        Given(an_api);
        When(calling_the_api_custom_serialisation_controller);
        Then(custom_serialisation_is_evident_in_the_response);
    }
    
    [Test]
    public void ensure_default_exception_handling()
    {
        Given(an_api);
        When(calling_the_api_that_should_error_unexpectedly);
        Then(default_exception_handling_is_evident_in_the_response);
        And(the_error_is_logged);
    }
    
    [Test]
    public void ensure_default_error_handling_for_validation_exceptions()
    {
        Given(an_api);
        When(calling_the_api_that_should_error_in_validation);
        Then(default_error_handling_for_validation_exceptions_is_evident_in_the_response);
    }    
    
    [Test]
    public void ensure_default_error_handling_for_serialisation_exceptions()
    {
        Given(an_api);
        When(calling_the_api_that_should_error_in_serialisation);
        Then(default_error_handling_for_serialisation_exceptions_is_evident_in_the_response);
    }
    
    [Test]
    public void provide_swagger_for_non_production_environments()
    {
        Given(an_api_not_in_production);
        When(calling_the_swagger_endpoint);
        Then(swagger_is_available);
    }
    
    [Test]
    public void not_provide_swagger_for_production_environments()
    {
        Given(an_api_in_production);
        When(calling_the_swagger_endpoint);
        Then(swagger_is_not_available);
    }

    [Test]
    // needs improving, but don't want to give option to pass loggers
    public void provide_logging()
    {
        Given(an_api);
        Then(logging_should_be_configured);
    }
}