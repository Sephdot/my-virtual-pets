# my-virtual-pets

![Website homepage](https://github.com/Sephdot/my-virtual-pets/blob/baf453fbd6738aab32a37bf7b9c68aeec9762642/my-virtual-pets/readme-images/homepage.gif)

## About:

<b>My Virtual Pets</b> is a fun web app that allows users to upload and collect 'virtual pets'.

Virtual Pets are created by users uploading an image, name, description, and personality for their pet. This information is then used to generate a pixel-style image of the pet, as well as assigning the pet a randomly-generated score based on their inputs. A user's complete collection of Virtual Pets can be viewed on the Collection page.

A Virtual Pet's score can be further influenced by other users, who can 'pet' other Virtual Pets. A 'petted' Virtual Pet is added to the user's list of favourite Virtual Pets, and that Virtual Pet's score is increased for each 'pet' it receives.

The app also contains a Leaderboards page, where users can view the top 10 highest-scoring Virtual Pets.

__________________________________

## Key Features: 

| Feature    | Image |
| -------- | ------- |
| Homepage carousel of recently created pets | ![Carousel](https://github.com/Sephdot/my-virtual-pets/blob/881910e0acd4136d36184b6ea698d3852479f5a0/my-virtual-pets/readme-images/carousel.gif) |
| Collection page where users can view and delete their pets | ![Collection page](https://github.com/Sephdot/my-virtual-pets/blob/881910e0acd4136d36184b6ea698d3852479f5a0/my-virtual-pets/readme-images/collection.gif) |
| State management to display authorized and unauthorized views of pages  | ![Authorized views](https://github.com/Sephdot/my-virtual-pets/blob/baf453fbd6738aab32a37bf7b9c68aeec9762642/my-virtual-pets/readme-images/auth%20page.png)    |
| OAuth with Google | ![OAuth demo](https://github.com/Sephdot/my-virtual-pets/blob/baf453fbd6738aab32a37bf7b9c68aeec9762642/my-virtual-pets/readme-images/OAuth.gif)   |
| 'Pet' virtual pets to increase their score    | ![Pet demo](https://github.com/Sephdot/my-virtual-pets/blob/baf453fbd6738aab32a37bf7b9c68aeec9762642/my-virtual-pets/readme-images/petting.gif)   |
| Leadership board | ![Leadership board](https://github.com/Sephdot/my-virtual-pets/blob/9b7f5efd6bcf020b579e2197c1363aaf3f6ec76e/my-virtual-pets/readme-images/leaderboard.gif) |

__________________________________

## Setting up the app: 

Clone this repository onto your local machine and follow the steps for setting up the database and API credentials set-out below. 

### Database set-up: 

Running in Development Mode: 

1. Go to launch settings and change the environment to 'Development'

Running in Production Mode: 
1. On a Windows Laptop, go to "Advanced System settings"
2. Click "Add New Environment Variables"
3. Populate the following template with your details:

Key: `ConnectionString__my_virtual_pets`

Value: `Server=<server_name>;Database=my_virtual_pets;User Id=sa;Password=<your_password>;TrustServerCertificate=True`

5. Go to Package Manager Console and run the command:

  `Update-Database -Context VPSqlServerContext`

6. Go to launch settings and change the environment to 'Production'

__________________________________

### Security: 

The app uses JWT tokens and Asp.Net core's Identity system for authoorisation, as well as OAuth2.0 with Google Cloud. To set this up: 

1. Generate a JWT secret below:

https://jwtsecret.com/generate

2. Create an account with Google Cloud and follow these steps to set up a project with OAuth2.0: 

https://developers.google.com/identity/protocols/oauth2

Required details: 
<br/>
Project name: my-virtual-pets
<br/>
Test email: <your-gmail>
<br/>
Authorized JavaScript origins: https://localhost:7247, http://localhost:5092, https://localhost:7091
<br/>
Authorized redirect URIs: https://localhost:7091/google-callback, http://localhost:5138/google-callback, https://localhost:7091/signin-google, http://localhost:5138/signin-google
<br/>

4. Generate your Client Id and Secret 


5. Populate the following template with your credentials and add this to your User Secrets. This can be done by navigating to the project's ` secrets.json ` file. 

`"Authentication": {
  "Google": {
    "ClientId": <your-client-id>,
    "ClientSecret":  <your-client-secret>  },
  "jwt": {
    "SecretKey" :  <your-jwt-key>
  }
}
`

__________________________________

### API Usage:

This app makes use of the following APIs in order to fulfill some of its functionality. Please follow the links below to create your own API keys to run the app:

### Dragoneye
https://dragoneye.ai/

### Picsart Remove Background
https://picsart.io/remove-background/

### AWS 
https://docs.aws.amazon.com/AmazonS3/latest/userguide/GetStartedWithS3.html

In order to run this app, valid API keys must be provided into the ` my-virtual-pets-api ` project's User Secrets. This can be done by navigating to the project's ` secrets.json ` file, and populating the following template.

`` {  "dragoneyeApiKey": "<your dragoneye api key>", 
  "BgRemoverApiKey": "<your bg remover api key>" } ``

__________________________________

## Running the app: 

<!-----info on running the app here backend & frontend ------>

1. Run both my-virtual-pets-api and my-virtual-pets-frontend

2. Navigate to http://localhost:5092/ (if running HTTP) or  https://localhost:7247 (if running HTTPS)

3. Create a new account or use the dummy logins below to get started

__________________________________

### 👥 Dummy Data: 

The following logins have been prepopulated:

` { "username": "PetLover123", "password": "hashedpassword123" } `
<br/>
` { "username": "FurryFriendFan", "password": "securepassword456" } `
<br/>
` { "username": "AnimalEnthusiast", "password": "randompassword789" } `
<br/>

__________________________________


## CREDITS: 

This app was built by the following developers: 

### Pixelate Function:

This app uses pixelation functionality inspired from custom pixelation software. The core pixelation process was identified and adapted into our project, enabling users to transform uploaded images into pixel-style art. Credit to PixelArt: https://github.com/BrandonHilde/PixelArt?tab=readme-ov-file
