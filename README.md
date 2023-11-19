# Project Description

This project showcases how to perform role assignments using Azure SDK for .NET.

## Prerequisites

Before running the code in this project, make sure you have the following prerequisites:
- Azure subscription
- Azure Active Directory tenant
- Client ID and Client Secret with appropriate permissions
- Subscription ID and Group ID

## Getting Started

Follow the steps below to get started with this project:

1. Clone the project repository.
2. Open the project in your preferred code editor.
3. Replace the following placeholders with your own values:
   - `YOUR TENANT ID HERE`: Replace with your Azure Active Directory tenant ID.
   - `YOUR CLIENT ID HERE`: Replace with your client ID.
   - `YOUR CLIENT SECRET HERE`: Replace with your client secret.
   - `YOUR SUBSCRIPTION ID HERE`: Replace with your Azure subscription ID.
   - `YOUR GROUP ID HERE`: Replace with your Azure group ID.

## Running the Code

To run the code in this project, perform the following steps:

1. Build the project to ensure all dependencies are resolved.
2. Run the `Main` method in the `Program` class.
3. The code will perform the following actions:
   - Connect to Azure using the provided credentials.
   - Get the collection of role definitions.
   - Find the Contributor role definition ID.
   - Create a role assignment using the Contributor role definition and group ID.
   - Retrieve the created role assignment ID.
   - Delete the role assignment.
   - Display the collection of role assignments after deletion.

## Conclusion

This project demonstrates how to perform role assignments using Azure SDK for .NET. Feel free to explore the code and customize it as per your requirements.