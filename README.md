# my-virtual-pets  ૮･ﻌ･ა
## 🐱 About:

<b>My Virtual Pets</b> is a fun web app that allows users to upload and collect 'virtual pets'.

Virtual Pets are created by users uploading a photo, name, description, and personality for their pet. This information is then used to generate a pixel-style image of the pet, as well as assigning the pet a randomly-generated score based on their inputs. A user's complete collection of Virtual Pets can be viewed on the Collection page.

A Virtual Pet's score can be further influenced by other users, who can 'pet' other Virtual Pets. A 'petted' Virtual Pet is added to the user's list of favourite Virtual Pets, and that Virtual Pet's score is increased for each 'pet' it receives.

The app also contains a Leaderboards page, where users can view the top 10 highest-scoring Virtual Pets.
__________________________________
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
### Picsart Remove Background
https://picsart.io/remove-background/

In order to run this app, valid API keys must be provided into the ` my-virtual-pets-api ` project's User Secrets. This can be done by navigating to the project's ` secrets.json ` file, and populating the following template.

`` {  "dragoneyeApiKey": "<your dragoneye api key>", 
  "BgRemoverApiKey": "<your bg remover api key>" } ``
## 🖼️ Pixelate Function:

This app uses pixelation functionality inspired from custom pixelation software. The core pixelation process was identified and adapted into our project, enabling users to transform uploaded images into pixel-style art.

### PixelArt
https://github.com/BrandonHilde/PixelArt?tab=readme-ov-file


__________________________________

## 👥 Dummy Data: 

The following logins have been prepopulated:

` { "username": "PetLover123", "password": "hashedpassword123" } `
<br/>
` { "username": "FurryFriendFan", "password": "securepassword456" } `
<br/>
` { "username": "AnimalEnthusiast", "password": "randompassword789" } `
<br/>
