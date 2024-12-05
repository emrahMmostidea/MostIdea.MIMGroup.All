using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using MostIdea.MIMGroup.Queries.Container;
using System;

namespace MostIdea.MIMGroup.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}