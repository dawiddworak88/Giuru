using System;
using System.Text;

namespace Foundation.Account.Services
{
    public class PasswordGenerationService : IPasswordGenerationService
    {
        private readonly char[] punctuations;

        private readonly char[] capitals;

        private readonly char[] lowercase;

        private readonly char[] numbers;

        public PasswordGenerationService()
        {
            this.punctuations = "!@#$%&*?".ToCharArray();
            this.capitals = "ABCDEFGHJKLMNOPQRSTUVWXYZ".ToCharArray();
            this.lowercase = "abcdefghijkmnopqrstuvwxyz".ToCharArray();
            this.numbers = "0123456789".ToCharArray();
        }

        public string GeneratePassword(int minlength)
        {
            if (minlength < 1 || minlength > 128)
            {
                throw new ArgumentException(nameof(minlength));
            }

            var rand = new Random(Guid.NewGuid().GetHashCode());

            var passwordBuilder = new StringBuilder();

            for (int i = 0; i < minlength; i++)
            {
                passwordBuilder.Append(this.lowercase[rand.Next(0, this.lowercase.Length)]);
            }

            passwordBuilder.Append(this.capitals[rand.Next(0, this.capitals.Length)]);

            passwordBuilder.Append(this.numbers[rand.Next(0, this.numbers.Length)]);

            passwordBuilder.Append(this.punctuations[rand.Next(0, this.punctuations.Length)]);

            return passwordBuilder.ToString();
        }
    }
}
