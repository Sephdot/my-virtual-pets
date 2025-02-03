# /ᐠ - ˕ -マ  my-virtual-pets  ૮･ﻌ･ა

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

## 👥 Dummy Data: 

The following logins have been prepopulated:

` { "username": "PetLover123", "password": "hashedpassword123" } `
<br/>
` { "username": "FurryFriendFan", "password": "securepassword456" } `
<br/>
` { "username": "AnimalEnthusiast", "password": "randompassword789" } `
<br/>
