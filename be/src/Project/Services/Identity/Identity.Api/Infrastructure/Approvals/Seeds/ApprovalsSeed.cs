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
                "Consent to receiving marketing information via newsletter",
                "NEWSLETTER - I want to receive information about news, promotions, products, or services of ELTAP Spółka z ograniczoną odpowiedzialnością Sp. k. under the terms specified in the privacy policy.",
                "Zgoda na wysyłanie informacji marketingowych za pomocą newslettera",
                "NEWSLETTER - Chcę otrzymywać informacje o nowościach, promocjach, produktach lub usługach ELTAP Spółka z ograniczoną odpowiedzialnością Sp. k. na zasadach określonych w polityce prywatności.",
                "Einwilligung zum Erhalt von Marketinginformationen per Newsletter",
                "NEWSLETTER – Ich möchte Informationen zu Neuigkeiten, Aktionen, Produkten oder Dienstleistungen von ELTAP Spółka z ograniczoną odpowiedzialnością Sp. k. gemäß den in den Datenschutz-Bestimmungen festgelegten Bedingungen erhalten.");

            SeedApproval(
                context,
                Guid.Parse("d34ec3d3-bb88-4de5-9391-d9d29609551a"),
                "Consent to receiving marketing information via phone",
                "PHONE - I consent to being contacted by phone and receiving SMS messages for the purpose of marketing ELTAP Spółka z ograniczoną odpowiedzialnością Sp. k. products or services under the terms specified in the privacy policy.",
                "Zgodana wysyłanie informacji marketingowych za pomocą telefonu",
                "TELEFON - Wyrażam zgodę na dzwonienie do mnie oraz wysyłanie wiadomości SMS w celu marketingu produktów lub usług ELTAP Spółka z ograniczoną odpowiedzialnością Sp. k. na zasadach określonych w polityce prywatności.",
                "Einwilligung zum Erhalt von Marketinginformationen per Telefon",
                "TELEFON – Ich stimme zu, dass ich für Marketingzwecke der Produkte oder Dienstleistungen von ELTAP Spółka z ograniczoną odpowiedzialnością Sp. k. gemäß den in den Datenschutz-Bestimmungen festgelegten Bedingungen angerufen und per SMS kontaktiert werde.");

            SeedApproval(
                context,
                Guid.Parse("5A4C4388-E991-4FCD-9CDA-24F60898C922"),
                "Consent to receiving confirmation emails regarding order placement",
                "I consent to receiving confirmation emails regarding order placement.",
                "Zgoda na wysyłanie maili potwierdzających złożenie zamówienia",
                "Wyrażam zgodę na wysyłanie maili potwierdzających złożenie zamówienia.",
                "Einwilligung zum Erhalt von Bestätigungs-E-Mails über die Auftragserteilung",
                "Ich stimme dem Erhalt von Bestätigungs-E-Mails über die Auftragserteilung zu.");
        }

        private static void SeedApproval(IdentityContext context, Guid id, string englishName, string englishDescription, string polishName, string polishDescription, string germanName, string germanDescription)
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
                    Description = englishDescription,
                    Language = "en"
                };

                var plApprovalTranslation = new ApprovalTranslation
                {
                    ApprovalId = id,
                    Name = polishName,
                    Description = polishDescription,
                    Language = "pl"
                };

                var deApprovalTranslation = new ApprovalTranslation
                {
                    ApprovalId = id,
                    Name = germanName,
                    Description = germanDescription,
                    Language = "de"
                };

                context.Approvals.Add(approval.FillCommonProperties());
                context.ApprovalTranslations.Add(enApprovalTranslation.FillCommonProperties());
                context.ApprovalTranslations.Add(plApprovalTranslation.FillCommonProperties());
                context.ApprovalTranslations.Add(deApprovalTranslation.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
