using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [CollectionDefinition(nameof(ApiCollection))]
    public class ApiCollection : ICollectionFixture<ApiFixture>
    {
    }
}
