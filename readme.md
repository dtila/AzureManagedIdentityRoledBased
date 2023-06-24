# Azure Web API ManagedIdentity Roled based Authorization
.NET Core solution that exemplify securing 2 app in Azure using the Managed Identity.

Tutorial: https://medium.com/@dtila/f1e5243be855

PowerShell script:

`
$tenantId = '<your tenant id>'
$serverRoleId = '7fbe8499-b22e-47e0-bc51-9c29bd1f40f2'
$clientManagedIdentity = '0f38730f-a246-415f-9edd-52c05292eeab'
$serverEnterpriseApp = '9014273b-4c34-4b58-ad1d-8ed04172d414'


Connect-AzureAd -TenantId $tenantId

New-AzureADServiceAppRoleAssignment `
    -Id $serverRoleId `
    -PrincipalId $clientManagedIdentity `
    -ObjectId $clientManagedIdentity `
    -ResourceId $serverEnterpriseApp

`
    