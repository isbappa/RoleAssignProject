using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Authorization;
using Azure.ResourceManager.Authorization.Models;


class Program
{
    static async Task Main(string[] args)
    {
        // Define your tenant ID, client ID, and client secret
        string tenantId = "YOUR TENANT ID HERE";
        string clientId = "YOUR CLIENT ID HERE";
        string clientSecret = "YOUR CLIENT SECRET HERE";

        string subscriptionId = "YOUR SUBSCRIPTION ID HERE";
        string groupId = "YOUR GROUP ID HERE";
        string scope = $"/subscriptions/{subscriptionId}"; 


        // Create a ClientSecretCredential
        ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        // Create an ArmClient with the credential
        ArmClient client = new ArmClient(credential);

        // Get the collection of role definitions
        var roleDefinitionsOperations = client
            .GetAuthorizationRoleDefinitions(new ResourceIdentifier(string.Format("/{0}", scope)))
            .GetAllAsync()
            .GetAsyncEnumerator();

        string roleAssignmentIdToAssign = string.Empty;

        // Iterate through the collection of role definitions AND find the Contributor role definition ID
        while (await roleDefinitionsOperations.MoveNextAsync())
        {
            if (roleDefinitionsOperations.Current.Data.RoleName.Equals("Contributor"))
            {
                Console.WriteLine($"Role definition name: {roleDefinitionsOperations.Current.Data.RoleName}");
                Console.WriteLine($"Role definition ID: {roleDefinitionsOperations.Current.Data.Name}");
                Console.WriteLine($"Role definition type: {roleDefinitionsOperations.Current.Data.RoleType}");

                roleAssignmentIdToAssign = roleDefinitionsOperations.Current.Data.Name;

                break;
            }
        }

        // Define the role definition to assign
        string roleDefinitionIdToAssign = $"/subscriptions/{subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/{roleAssignmentIdToAssign}";

        // Define the name of the role assignment
        string roleAssignmentName = Guid.NewGuid().ToString();

        // Create the role assignment content
        RoleAssignmentCreateOrUpdateContent content = new RoleAssignmentCreateOrUpdateContent(new ResourceIdentifier(roleDefinitionIdToAssign), Guid.Parse(groupId))
        {
            PrincipalType = RoleManagementPrincipalType.Group,
        };

        // Define the scope of the role assignment
        ResourceIdentifier scopeId = new ResourceIdentifier(string.Format("/{0}", scope));

        // Get the collection of role assignments
        RoleAssignmentCollection collection = client.GetRoleAssignments(scopeId);

      
        // Invoke the operation to create the role assignment
        ArmOperation<RoleAssignmentResource> lro = await collection.CreateOrUpdateAsync(WaitUntil.Completed, roleAssignmentName, content);
        RoleAssignmentResource result = lro.Value;

        // Get the ID of the role assignment
        RoleAssignmentData resourceData = result.Data;
        Console.WriteLine($"Succeeded on id: {resourceData.Id}");

        // Get the collection of role assignments
        Console.WriteLine("Get the collection of role assignments");
        foreach (var item in collection)
        {
            Console.WriteLine($"item Id: {item.Data.Id}");
            Console.WriteLine($"item Name: {item.Data.Name}");
        }

        // Get the role assignment to delete by name
        var roleAssignmentToDelete = collection
            .Where(collection => collection.Data.Name == roleAssignmentName)
            .FirstOrDefault();

        // Invoke the operation to delete the role assignment
        ArmOperation<RoleAssignmentResource> lroDelete = await roleAssignmentToDelete.DeleteAsync(WaitUntil.Completed)!;

        // Get the collection of role assignments after delete
        Console.WriteLine("Get the collection of role assignments after delete");
        foreach (var item in collection)
        {
            Console.WriteLine($"item Id: {item.Data.Id}");
            Console.WriteLine($"item Name: {item.Data.Name}");
        }
    }
}