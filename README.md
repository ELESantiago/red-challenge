# Challenge
## Swagger
The swagger page is public and available [here](http://redchallenge-dev.eba-wpfvidda.us-west-2.elasticbeanstalk.com/swagger/index.html)

For creating an Order you first have to create a User by making a POST request to `/Authentication/sign-up`
``` JSON
// BODY
{
    "username": "string",
    "password": "string"
}
```
This will return a token you can put in the Authorize header
and the a POST method could be make to `/Order`
``` JSON
// BODY
{
  "orderTypeId": int,
  "customerId": "GUID"
}
```
The `orderTypeId` can be any value from 1-5 that represents the following order type
| orderTypeId | value    |
|-------------|----------|
|     1       | Standard |
|     2       | Sales    |
|     3       | Purchase |
|     4       | Transfer |
|     5       | Return   |

I didn't create a way to get or create Customers, but you can use the following Guid ids
| Id | Name |
|----|------|
|AAE4F742-0EE7-433E-858C-3ABA51F45B2D | Test customer 2 |
|07DC1D31-4988-42BE-A842-3F735744119F | Test customer 3 |
|12724A7F-1EED-4C6F-93B7-69A4C3C22D00 | Test customer 4 |
|95D97496-8A72-4F63-BFCD-75C8FCA24C95 | Test customer 1 |
