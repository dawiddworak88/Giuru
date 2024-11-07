using Foundation.GenericRepository.Extensions;
using Identity.Api.Infrastructure.Approvals.Entities;
using System;
using System.Linq;

namespace Identity.Api.Infrastructure.Approvals.Seeds
{
    public static class ApprovalsSeed
    {
        public static void SeedApprovals(IdentityContext context)
        {
            SeedApproval(
                context,
                Guid.Parse("7addb70a-0942-488d-ba27-afa5b570ede5"),
                "I consent to receiving commercial information via electronic means, to the email address I have provided.",
                "Wyrażam zgodę na otrzymywanie informacji handlowych drogą elektroniczną, na wskazany przeze mnie adres e-mail.",
                "Ich stimme zu, kommerzielle Informationen auf elektronischem Wege an die von mir angegebene E-Mail-Adresse zu erhalten.");

            SeedApproval(
                context,
                Guid.Parse("d34ec3d3-bb88-4de5-9391-d9d29609551a"),
                "I consent to being contacted by phone for marketing purposes.",
                "Wyrażam zgodę na kontakt telefoniczny w celach marketingowych.",
                "Ich erteile meine Zustimmung zu telefonischem Kontakt zu Marketingzwecken.");

            SeedApproval(
                context,
                Guid.Parse("5A4C4388-E991-4FCD-9CDA-24F60898C922"),
                "I consent to receiving confirmation emails regarding order placement.",
                "Wyrażam zgodę na wysyłanie maili potwierdzających złożenie zamówienia.",
                "Ich stimme dem Erhalt von Bestätigungs-E-Mails über die Auftragserteilung zu.");
        }

        private static void SeedApproval(IdentityContext context, Guid id, string englishName, string polishName, string germanName)
        {
            if (context.Approvals.Any(x => x.Id == id) is false)
            {
                var approval = new Approval
                {
                    Id = id,
                };

                var enApprovalTranslation = new ApprovalTranslation
                {
                    ApprovalId = id,
                    Name = englishName,
                    Language = "en"
                };

                var plApprovalTranslation = new ApprovalTranslation
                {
                    ApprovalId = id,
                    Name = polishName,
                    Language = "pl"
                };

                var deApprovalTranslation = new ApprovalTranslation
                {
                    ApprovalId = id,
                    Name = germanName,
                    Language = "de"
                };

                context.Add(approval.FillCommonProperties());
                context.Add(enApprovalTranslation.FillCommonProperties());
                context.Add(plApprovalTranslation.FillCommonProperties());
                context.Add(deApprovalTranslation.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
