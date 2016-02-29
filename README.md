# aspnet-login-test

This project tests the ability to use Active Directory for authentication, yet also have the ability to log in as an arbitrary test user.

Possible approaches:
* Use AD test user accounts and start IE using "Run As..."
* Use a URL parameter to specify the user to log in as (this would need to be restricted to use only in the dev/test environment)
