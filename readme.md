# Securing Essential Data with Azure Key Vault

## Summary  
This repository exists to support a demo presented at the Global Azure Bootcamp 2019 in Denver, CO

## How To View  
Following along with the presentation slides, the first sample code will be based off of the Master branch.  

The next branch to view will be `removeConnectionString`, while the presentation demonstrates removing the connection string from source code and replacing it with a Secret stored in the Key Vault.

Next we will switch to the `addDataProtection` branch, which sets up the initial DPAPI protector to encrypt data before saving to the database.

Finally, we will switch to the `addKeyVaultDataProtection` branch, which will demonstrate saving IDs as key vault Secrets, setting up a Blob Storage account to persist keys, and using a Key Vault key to protect data with our stored keys.