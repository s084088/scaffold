using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiServer.Comm;

public static class CustomContractResolverExtension
{
    public static CustomContractResolver AddEmptyCollectionIgnore(this CustomContractResolver customContractResolver)
    {
        customContractResolver.AddPredicate((property, obj) => property.ValueProvider.GetValue(obj) is not ICollection collection || collection.Count != 0);
        return customContractResolver;
    }
}

public class CustomContractResolver : DefaultContractResolver
{
    public static CustomContractResolver Default => new();

    private readonly List<Func<JsonProperty, object, bool>> filters = new();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        if (filters == null || filters.Count == 0)
            return base.CreateProperty(member, memberSerialization);

        JsonProperty property = base.CreateProperty(member, memberSerialization);

        foreach (Func<JsonProperty, object, bool> predicate in filters)
            property.ShouldSerialize += obj => predicate(property, obj);

        return property;
    }

    public void AddPredicate(Func<JsonProperty, object, bool> filter)
    {
        if (!filters.Contains(filter))
            filters.Add(filter);
    }
}