# my-virtual-pets  ૮･ﻌ･ა

## 📋 Database set-up: 

Running in Development Mode: 

1. Go to launch settings and change the environment to 'Development'
2. Run Program.cs 

Running in Production Mode: 
1. On a Windows Laptop, go to "Advanced System settings"
2. Click "Add New Environment Variables"
3. Populate the following template with your details:

Key: `ConnectionString__my_virtual_pets`

Value: `Server=<server_name>;Database=my_virtual_pets;User Id=sa;Password=<your_password>;TrustServerCertificate=True`

5. Go to Package Manager Console and run the command:

  `Update-Database -Context VPSqlServerContext`

6. Go to launch settings and change the environment to 'Production'
7. Run Program.cs 
__________________________________

## 📞 API Usage:

This app makes use of the following APIs in order to fulfill some of its functionality.

### Dragoneye
https://dragoneye.ai/

In order to run this app, valid API keys must be provided into the my-virtual-pets-api project's User Secrets. This can be done by navigating to the project's ` secrets.json ` file, and populating the following template.

`` {  "dragoneyeApiKey": "<your dragoneye api key>", 
  "BgRemoverApiKey": "<your bg remover api key>" } ``


__________________________________

## 👥 Dummy Data: 

The following logins have been prepopulated:

` { "username": "PetLover123", "password": "hashedpassword123" } `
<br/>
` { "username": "FurryFriendFan", "password": "securepassword456" } `
<br/>
` { "username": "AnimalEnthusiast", "password": "randompassword789" } `
<br/>
