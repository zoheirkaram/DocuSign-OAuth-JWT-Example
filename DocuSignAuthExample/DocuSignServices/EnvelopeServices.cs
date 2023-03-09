using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;

namespace DocuSignAuthExample.DocuSignServices
{
	public class EnvelopeServices
	{
        public static EnvelopeSummary SendEnvelopeFromTemplate(string accessToken, string signerEmail, string signerName, string ccEmail, string ccName, string basePath, string accountId, string templateId)
		{
			var docuSignClient = new DocuSignClient(basePath);

			docuSignClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);

			var envelopesApi = new EnvelopesApi(docuSignClient);
			var envelope = MakeEnvelope(signerEmail, signerName, ccEmail, ccName, templateId);

			EnvelopeSummary result = envelopesApi.CreateEnvelope(accountId, envelope);

			return result;
		}

		public static Stream GetDocumentStreamByEnvelopeId(string accessToken, string basePath, string accountId, string envelopeId)
		{
			var apiClient = new DocuSignClient(basePath);

			apiClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);

			var envelopesApi = new EnvelopesApi(apiClient);
			var results = envelopesApi.GetDocument(accountId, envelopeId, "1");

			return results;
		}

		public static Envelope GetEnvelopeById(string accessToken, string basePath, string accountId, string envelopeId)
		{
			var apiClient = new DocuSignClient(basePath);

			apiClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);

			var envelopesApi = new EnvelopesApi(apiClient);
			var results = envelopesApi.GetEnvelope(accountId, envelopeId);

			return results;
		}

		private static EnvelopeDefinition MakeEnvelope(string signerEmail, string signerName, string ccEmail, string ccName, string templateId)
		{

			EnvelopeDefinition env = new EnvelopeDefinition();
			env.TemplateId = templateId;

			var contractorRole = new TemplateRole();
			var sellerRole = new TemplateRole();

			contractorRole.Email = signerEmail;
			contractorRole.Name = signerName;
			contractorRole.RoleName = "Contractor";
			contractorRole.Tabs = new Tabs();
			contractorRole.Tabs.TextTabs = new List<Text> {
				new Text() { TabLabel = "ContractName", Value = signerName }
			};

			sellerRole.Email = ccEmail;
			sellerRole.Name = ccName;
			sellerRole.RoleName = "Seller";
			sellerRole.Tabs = new Tabs();
			sellerRole.Tabs.TextTabs = new List<Text> {
				new Text() { TabLabel = "SellerName", Value = ccName }
			};

			env.TemplateRoles = new List<TemplateRole> { contractorRole, sellerRole };
			env.Status = "sent";

			return env;
		}


	}
}
