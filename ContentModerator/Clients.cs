using Microsoft.Azure.CognitiveServices.ContentModerator;

namespace ContentModerator
{
	// Wraps the creation and configuration of a Content Moderator client.
	public static class Clients
	{
		// The region/location for your Content Moderator account, 
		// for example, westus.
		private static readonly string AzureRegion = "centralindia";

		// The base URL fragment for Content Moderator calls.
		private static readonly string AzureBaseURL =
			$"https://{AzureRegion}.api.cognitive.microsoft.com";

		// Your Content Moderator subscription key.
		private static readonly string CMSubscriptionKey = "7af6ae7c0df9497982623364ea4532c6";

		// Returns a new Content Moderator client for your subscription.
		public static ContentModeratorClient NewClient()
		{
			// Create and initialize an instance of the Content Moderator API wrapper.
			ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(CMSubscriptionKey));

			client.Endpoint = AzureBaseURL;
			return client;
		}
	}
}
