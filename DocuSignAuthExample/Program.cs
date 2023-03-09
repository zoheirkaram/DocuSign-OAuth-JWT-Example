using System.Text;
using DocuSign.eSign.Client;
using DocuSignAuthExample.Utilities;
using DocuSignAuthExample.DocuSignServices;

namespace DocuSignAuth
{
	class Program
	{
		static void Main(string[] args)
		{
			#region ~~~~~~~~~~~~~~~~~~~~~~~~ Variable declarations for calling DocuSign OAuth ~~~~~~~~~~~~~~~~~~~~~~~
			var baseUri = "https://demo.docusign.com/restapi";
			var authServer = "account-d.docusign.com";
			var apiClient = new DocuSignClient(baseUri);

			var privateKey = @"-----BEGIN RSA PRIVATE KEY-----
					...
					...
					...
					...
					...
					...
					...
					...
					Type the private RSA key here
					...
					...
					...
					...
					...
					...
					...
					...
					-----END RSA PRIVATE KEY-----";
			var integratorKey = "43dxxxxx-6exx-4xxx-8xxx-9aacxxxxxxxx";
			var userId		  = "069xxxxx-exxx-4xxx-bxxx-df3exxxxxxxx";
			var templateId    = "b76xxxxx-5xxx-4xxx-axxx-f95bxxxxxxxx";

			var rsaPrivateKey = Encoding.UTF8.GetBytes(privateKey); 
			//var rsaPrivateKey = File.ReadAllBytes(@"./private.key"); // or you can include it in different ways like a file or other secure way
			var scopes = new List<string>() { "signature" };

			#endregion

			#region ~~~~~~~~~~~~~~~~~~~~~~~~~ Authentication Section ~~~~~~~~~~~~~~~~~~~~~~~~~

			var token = apiClient.RequestJWTUserToken(integratorKey, userId, authServer, rsaPrivateKey, 1, scopes);
			var accessToken = token.access_token;

			#endregion

			#region ~~~~~~~~~~~~~~~~~~~~~~~ Obtain account information ~~~~~~~~~~~~~~~~~~~~~~~

			var docuSignClient = new DocuSignClient();
			docuSignClient.SetOAuthBasePath(authServer);

			var userInfo = docuSignClient.GetUserInfo(accessToken);
			var account = userInfo.Accounts.FirstOrDefault();

			#endregion

			#region ~~~~~~~~~~~~~~~~~~~~~~~~ Sending template Envelope ~~~~~~~~~~~~~~~~~~~~~~~

			var envelopeSummaryResult = EnvelopeServices.SendEnvelopeFromTemplate(
				accessToken,
				"johndoe@gmail.com",
				"John Doe",
				"sarasmith@hotmail.com",
				"Sara Smith",
				$"{account.BaseUri}/restapi",
				account.AccountId,
				templateId);

			#endregion

			#region ~~~~~~~~~~~~~~~~~~~~~~~~ Retrieve envelope by EnvelopeId ~~~~~~~~~~~~~~~~~~~~~~~

			var envelopeResult = EnvelopeServices.GetEnvelopeById(accessToken, $"{account.BaseUri}/restapi", account.AccountId, envelopeSummaryResult.EnvelopeId);

			#endregion

			#region ~~~~~~~~~~~~~~~~~~~~~~~~ Display objects properties ~~~~~~~~~~~~~~~~~~~~~~~

			userInfo.DisplayProperties();
			account.DisplayProperties();
			envelopeSummaryResult.DisplayProperties();
			envelopeResult.DisplayProperties();

			#endregion
		}
	}
}
