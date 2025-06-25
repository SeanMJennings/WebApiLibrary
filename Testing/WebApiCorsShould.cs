namespace Testing;

[TestFixture]
public partial class WebApiCorsShould
{
    [Test]
    public void allow_setting_restrictive_cors_policy()
    {
        Given(an_api_with_restricted_cors_policy);
        When(calling_the_api_from_a_different_origin);
        Then(restrictive_cors_policy_is_applied);
    }
    
    [Test]
    public void allow_setting_unrestricted_cors_policy()
    {
        Given(an_api_with_unrestricted_cors_policy);
        When(calling_the_api_from_a_different_origin);
        Then(unrestricted_cors_policy_is_applied);
    }
}